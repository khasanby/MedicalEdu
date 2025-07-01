# MedicalEdu Database Schema

## Overview

This document describes the complete database schema for the MedicalEdu platform, designed to support medical education with course management, instructor-student bookings, payments, and comprehensive audit logging.

## Database Design Principles

- **GUID Primary Keys**: All entities use GUIDs for better distribution and security
- **Soft Deletes**: Where applicable, entities use status flags rather than hard deletes
- **Audit Trail**: Comprehensive logging of all critical operations
- **UTC Timestamps**: All dates stored in UTC for consistency across time zones
- **Navigation Properties**: Full Entity Framework relationships defined

## Core Entities

### Users Table

Stores all platform users (Students, Instructors, Admins).

**Key Fields:**
- `Id` (PK): Unique identifier
- `Email`: Must be unique across the platform
- `EmailConfirmed`: Email verification status
- `Role`: UserRole enum (Admin=0, Instructor=1, Student=2)
- `TimeZone`: User's preferred timezone for booking display

**Indexes:**
- Unique index on `Email`
- Index on `Role` for role-based queries
- Index on `EmailConfirmed` for verification workflows

### Courses Table

Stores course information created by instructors.

**Key Fields:**
- `Id` (PK): Unique identifier
- `InstructorId` (FK): Links to Users table
- `IsPublished`: Controls course visibility to students
- `Price`: Optional course price for paid content

**Indexes:**
- Index on `InstructorId` for instructor course listings
- Index on `IsPublished` for public course queries
- Composite index on `(IsPublished, CreatedAt)` for sorted listings

### CourseMaterials Table

Stores uploaded course content (PDFs, videos, etc.).

**Key Fields:**
- `Id` (PK): Unique identifier
- `CourseId` (FK): Links to Courses table
- `FileUrl`: Blob storage URL
- `IsFree`: Determines if payment/enrollment is required
- `Order`: Display sequence within course

**Indexes:**
- Index on `CourseId` for course material queries
- Composite index on `(CourseId, Order)` for ordered retrieval

### AvailabilitySlots Table

Stores instructor availability for bookings.

**Key Fields:**
- `Id` (PK): Unique identifier
- `CourseId` (FK): Associated course
- `InstructorId` (FK): Slot owner
- `StartTimeUtc`/`EndTimeUtc`: Slot time window
- `IsBooked`: Booking status
- `Price`: Slot-specific pricing

**Indexes:**
- Index on `InstructorId` for instructor availability
- Index on `CourseId` for course-specific slots
- Composite index on `(IsBooked, StartTimeUtc)` for available slot queries
- Index on `StartTimeUtc` for time-based queries

### Bookings Table

Stores student booking requests and their status.

**Key Fields:**
- `Id` (PK): Unique identifier
- `StudentId` (FK): Booking student
- `InstructorId` (FK): Booked instructor
- `AvailabilitySlotId` (FK): Reserved slot
- `Status`: BookingStatus enum (Pending, Confirmed, Cancelled, etc.)
- `Amount`: Booking price

**Business Rules:**
- Only one booking per availability slot
- Status transitions: Pending → Confirmed/Cancelled
- Confirmed bookings require successful payment

**Indexes:**
- Index on `StudentId` for student booking history
- Index on `InstructorId` for instructor booking management
- Index on `Status` for status-based queries
- Index on `AvailabilitySlotId` for slot-booking relationship

### Payments Table

Stores payment transaction records.

**Key Fields:**
- `Id` (PK): Unique identifier
- `BookingId` (FK): Associated booking
- `Status`: PaymentStatus enum (Pending, Succeeded, Failed, etc.)
- `Provider`: PaymentProvider enum (Stripe, PayPal)
- `ProviderTransactionId`: External payment reference
- `Currency`: ISO currency code

**Business Rules:**
- One primary payment per booking (refunds create new records)
- Webhook processing updates payment status
- Failed payments don't confirm bookings

**Indexes:**
- Index on `BookingId` for booking-payment lookup
- Index on `ProviderTransactionId` for webhook processing
- Index on `Status` for payment status queries

### Enrollments Table

Tracks student enrollment in courses (for course access control).

**Key Fields:**
- `Id` (PK): Unique identifier
- `StudentId` (FK): Enrolled student
- `CourseId` (FK): Enrolled course
- `ProgressPercentage`: Course completion (0-100)
- `IsActive`: Enrollment status

**Business Rules:**
- One enrollment record per student per course
- Progress tracking for course materials
- Active enrollments grant access to paid content

**Indexes:**
- Unique composite index on `(StudentId, CourseId)`
- Index on `StudentId` for student enrollment queries
- Index on `CourseId` for course enrollment metrics

### Notifications Table

Stores system notifications and email records.

**Key Fields:**
- `Id` (PK): Unique identifier
- `UserId` (FK): Notification recipient
- `Type`: NotificationType enum
- `IsRead`: Read status
- `EmailSent`: Email delivery status

**Business Rules:**
- Notifications can be in-app only or trigger emails
- Related entity tracking for contextual notifications
- Read status for user notification management

**Indexes:**
- Index on `UserId` for user notification queries
- Index on `Type` for notification type filtering
- Index on `IsRead` for unread notification counts

### AuditLog Table

Comprehensive audit trail for all critical operations.

**Key Fields:**
- `Id` (PK): Unique identifier
- `EntityName`: Modified entity type
- `EntityId`: Modified entity identifier
- `Action`: AuditActionType enum
- `UserId` (FK): User who performed action
- `OldValues`/`NewValues`: JSON change tracking

**Business Rules:**
- Immutable records (no updates/deletes)
- Captures all CRUD operations on critical entities
- IP address and user agent logging for security

**Indexes:**
- Index on `EntityName` for entity-specific audit queries
- Index on `EntityId` for entity change history
- Index on `UserId` for user activity tracking
- Index on `CreatedAt` for time-based audit queries

## Enums

### UserRole
- `Admin = 0`: System administrators
- `Instructor = 1`: Course creators and teachers
- `Student = 2`: Course consumers and booking clients

### BookingStatus
- `Pending = 0`: Initial booking request
- `Confirmed = 1`: Payment successful, booking active
- `Cancelled = 2`: Booking cancelled
- `Completed = 3`: Session completed
- `NoShow = 4`: Student didn't attend

### PaymentStatus
- `Pending = 0`: Payment initiated
- `Succeeded = 1`: Payment completed successfully
- `Failed = 2`: Payment failed
- `Cancelled = 3`: Payment cancelled
- `Refunded = 4`: Payment refunded

### PaymentProvider
- `Stripe = 0`: Stripe payment processor
- `PayPal = 1`: PayPal payment processor

### NotificationType
- `BookingConfirmation = 0`: Booking confirmed notification
- `BookingReminder = 1`: Upcoming booking reminder
- `BookingCancellation = 2`: Booking cancelled notification
- `PaymentConfirmation = 3`: Payment successful
- `PaymentFailed = 4`: Payment failed
- `CoursePublished = 5`: New course available
- `EmailVerification = 6`: Email verification required
- `PasswordReset = 7`: Password reset request
- `GeneralAnnouncement = 8`: System announcements
- `BookingRescheduled = 9`: Booking time changed
- `CourseUpdated = 10`: Course content updated

### AuditActionType
- `Create = 0`: Entity created
- `Update = 1`: Entity updated
- `Delete = 2`: Entity deleted
- `Login = 3`: User login
- `Logout = 4`: User logout
- `EmailConfirmation = 5`: Email confirmed
- `PasswordReset = 6`: Password reset

## Key Relationships

1. **User → Course**: One instructor can create many courses
2. **Course → CourseMaterial**: One course can have many materials
3. **Course → AvailabilitySlot**: One course can have many time slots
4. **AvailabilitySlot → Booking**: One slot can have one booking
5. **Booking → Payment**: One booking can have multiple payments (refunds)
6. **User → Enrollment**: One student can enroll in many courses
7. **User → Notification**: One user can have many notifications
8. **User → AuditLog**: One user can perform many logged actions

## Performance Considerations

### Indexing Strategy
- Primary keys on all tables (clustered)
- Foreign key indexes for join performance
- Composite indexes for common query patterns
- Covering indexes for frequently accessed columns

### Query Optimization
- Pagination for large result sets
- Filtered indexes for boolean columns
- Partitioning consideration for audit logs by date

### Caching Strategy
- Cache published courses for public browsing
- Cache user sessions and permissions
- Cache availability slots for booking interface
- Invalidate caches on relevant updates

## Data Integrity Rules

### Referential Integrity
- All foreign keys enforced at database level
- Cascade rules defined for dependent records
- Check constraints on enum values

### Business Rules
- Email uniqueness across all users
- One booking per availability slot
- Booking confirmation requires successful payment
- Instructor can only manage their own courses/slots

### Temporal Constraints
- Availability slots must have EndTime > StartTime
- Booking times must be in the future when created
- Payment timestamps must be logical (created ≤ processed)

## Security Considerations

### Data Protection
- Password hashes never stored in plain text
- Sensitive audit data encrypted at rest
- PII data minimization and anonymization options

### Access Control
- Role-based permissions enforced in application layer
- Row-level security for multi-tenant scenarios
- API key and JWT token validation

### Audit Requirements
- All data modifications logged
- User actions tracked with IP and user agent
- Compliance with data protection regulations

## Migration Strategy

### Initial Setup
1. Create database and schema
2. Apply all table creation scripts
3. Create indexes and constraints
4. Seed reference data (admin users, etc.)

### Future Changes
- Use Entity Framework migrations
- Version control all schema changes
- Test migrations in staging environment
- Plan for zero-downtime deployments

This schema provides a robust foundation for the MedicalEdu platform, supporting all the required functionality while maintaining data integrity, performance, and security standards. 