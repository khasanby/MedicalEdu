# MedicalEdu Database Schema - Optimized Version

## Overview

This document describes the complete optimized database schema for the MedicalEdu platform, designed to support medical education with advanced course management, instructor-student bookings, payments, progress tracking, ratings, and comprehensive audit logging.

## Database Design Principles

- **GUID Primary Keys**: All entities use GUIDs for better distribution and security
- **Soft Deletes**: Entities use `deleted_at` timestamps for data preservation
- **Audit Trail**: Comprehensive logging of all critical operations with before/after JSON
- **UTC Timestamps**: All dates stored in UTC for consistency across time zones
- **Navigation Properties**: Full Entity Framework relationships defined
- **Multi-Currency Support**: Flexible pricing with currency tracking
- **Security First**: Account locking, session management, and comprehensive audit

## Core Entities

### Users Table

Stores all platform users (Students, Instructors, Admins) with enhanced security features.

**Key Fields:**
- `Id` (PK): Unique identifier
- `Email`: Must be unique across the platform
- `EmailConfirmed`: Email verification status
- `Role`: UserRole enum (Admin=0, Instructor=1, Student=2)
- `TimeZone`: User's preferred timezone for booking display
- `DeletedAt`: Soft delete timestamp
- `PasswordResetToken`: For password reset functionality
- `LastLoginAt`: Last login tracking
- `FailedLoginAttempts`: Account security
- `LockedUntil`: Account locking mechanism

**Indexes:**
- Unique index on `Email`
- Index on `Role` for role-based queries
- Index on `EmailConfirmed` for verification workflows
- Index on `DeletedAt` for soft delete filtering
- Index on `IsActive` for active user queries
- Index on `LastLoginAt` for activity tracking

### Courses Table

Stores course information with rich metadata and categorization.

**Key Fields:**
- `Id` (PK): Unique identifier
- `InstructorId` (FK): Links to Users table
- `IsPublished`: Controls course visibility to students
- `Price`: Course price with currency support
- `Currency`: ISO currency code (default: USD)
- `ShortDescription`: Brief course description
- `VideoIntroUrl`: Course introduction video
- `DurationMinutes`: Course duration
- `MaxStudents`: Maximum enrollment limit
- `Category`: Course categorization
- `Tags`: JSON array of course tags
- `PublishedAt`: Publication timestamp
- `DeletedAt`: Soft delete timestamp

**Indexes:**
- Index on `InstructorId` for instructor course listings
- Index on `IsPublished` for public course queries
- Composite index on `(IsPublished, CreatedAt)` for sorted listings
- Index on `DeletedAt` for soft delete filtering
- Index on `Category` for category-based queries

### CourseMaterials Table

Stores uploaded course content with enhanced metadata and progress tracking.

**Key Fields:**
- `Id` (PK): Unique identifier
- `CourseId` (FK): Links to Courses table
- `FileUrl`: Blob storage URL
- `IsFree`: Determines if payment/enrollment is required
- `SortIndex`: Display sequence within course
- `DurationMinutes`: Video content duration
- `IsRequired`: Whether material is required for completion

**Indexes:**
- Index on `CourseId` for course material queries
- Composite index on `(CourseId, SortIndex)` for ordered retrieval
- Index on `IsFree` for free content filtering

### AvailabilitySlots Table

Stores instructor availability with advanced scheduling features.

**Key Fields:**
- `Id` (PK): Unique identifier
- `CourseId` (FK): Associated course
- `StartTimeUtc`/`EndTimeUtc`: Slot time window
- `IsBooked`: Booking status
- `Price`: Slot-specific pricing
- `Currency`: ISO currency code
- `MaxParticipants`: Group session capacity
- `CurrentParticipants`: Current participant count
- `IsRecurring`: Recurring slot flag
- `RecurringPattern`: JSON pattern for recurring slots

**Indexes:**
- Index on `CourseId` for course-specific slots
- Composite index on `(IsBooked, StartTimeUtc)` for available slot queries
- Index on `StartTimeUtc` for time-based queries
- Composite index on `(CourseId, StartTimeUtc, EndTimeUtc)` [unique]

### Bookings Table

Stores student booking requests with complete lifecycle management.

**Key Fields:**
- `Id` (PK): Unique identifier
- `StudentId` (FK): Booking student
- `AvailabilitySlotId` (FK): Reserved slot
- `Status`: BookingStatus enum (Pending, Confirmed, Cancelled, Completed, NoShow, Rescheduled)
- `Amount`: Booking price
- `Currency`: ISO currency code
- `RescheduledFromBookingId`: For rescheduled bookings
- `MeetingUrl`: Virtual session URL
- `MeetingNotes`: Post-session documentation

**Business Rules:**
- Only one booking per availability slot
- Status transitions: Pending → Confirmed/Cancelled/Rescheduled
- Confirmed bookings require successful payment
- Rescheduled bookings maintain history

**Indexes:**
- Index on `StudentId` for student booking history
- Index on `AvailabilitySlotId` for slot-booking relationship
- Index on `Status` for status-based queries
- Index on `CreatedAt` for booking timeline
- Index on `RescheduledFromBookingId` for reschedule tracking

### Payments Table

Stores payment transaction records with enhanced lifecycle support.

**Key Fields:**
- `Id` (PK): Unique identifier
- `BookingId` (FK): Associated booking
- `UserId` (FK): Payment user
- `Status`: PaymentStatus enum (Pending, Succeeded, Failed, Cancelled, Refunded, PartiallyRefunded)
- `Provider`: PaymentProvider enum (Stripe, PayPal)
- `ProviderTransactionId`: External payment reference
- `Currency`: ISO currency code
- `RefundAmount`: Partial refund amount
- `RefundReason`: Refund justification

**Business Rules:**
- One primary payment per booking (refunds create new records)
- Webhook processing updates payment status
- Failed payments don't confirm bookings
- Partial refunds supported

**Indexes:**
- Index on `BookingId` for booking-payment lookup
- Index on `ProviderTransactionId` for webhook processing
- Index on `Status` for payment status queries
- Index on `UserId` for user payment history

### Enrollments Table

Tracks student enrollment with detailed progress monitoring.

**Key Fields:**
- `Id` (PK): Unique identifier
- `StudentId` (FK): Enrolled student
- `CourseId` (FK): Enrolled course
- `ProgressPercentage`: Course completion (0-100)
- `IsActive`: Enrollment status
- `LastAccessedAt`: Last course access timestamp

**Business Rules:**
- One enrollment record per student per course
- Progress tracking for course materials
- Active enrollments grant access to paid content

**Indexes:**
- Unique composite index on `(StudentId, CourseId)`
- Index on `StudentId` for student enrollment queries
- Index on `CourseId` for course enrollment metrics
- Index on `IsActive` for active enrollment filtering

### CourseProgress Table

Granular progress tracking per course material.

**Key Fields:**
- `Id` (PK): Unique identifier
- `EnrollmentId` (FK): Associated enrollment
- `CourseMaterialId` (FK): Material being tracked
- `IsCompleted`: Completion status
- `CompletedAt`: Completion timestamp
- `TimeSpentSeconds`: Time tracking

**Business Rules:**
- One progress record per material per enrollment
- Tracks both completion and time spent
- Supports detailed analytics

**Indexes:**
- Index on `EnrollmentId` for enrollment progress
- Index on `CourseMaterialId` for material analytics
- Index on `IsCompleted` for completion queries

### Notifications Table

Multi-channel notification system with scheduling support.

**Key Fields:**
- `Id` (PK): Unique identifier
- `UserId` (FK): Notification recipient
- `Type`: NotificationType enum
- `IsRead`: Read status
- `EmailSent`/`SmsSent`/`PushSent`: Multi-channel delivery
- `ScheduledFor`: Scheduled notification timestamp

**Business Rules:**
- Notifications can be in-app, email, SMS, or push
- Scheduled notifications for reminders
- Related entity tracking for contextual notifications

**Indexes:**
- Index on `UserId` for user notification queries
- Index on `Type` for notification type filtering
- Index on `IsRead` for unread notification counts
- Index on `ScheduledFor` for scheduled notifications

### AuditLogs Table

Comprehensive audit trail for all critical operations.

**Key Fields:**
- `Id` (PK): Unique identifier
- `EntityName`: Modified entity type
- `EntityId`: Modified entity identifier
- `Action`: AuditActionType enum
- `UserId` (FK): User who performed action
- `OldValues`/`NewValues`: JSON change tracking
- `IpAddress`/`UserAgent`: Security context

**Business Rules:**
- Immutable records (no updates/deletes)
- Captures all CRUD operations on critical entities
- IP address and user agent logging for security

**Indexes:**
- Index on `EntityName` for entity-specific audit queries
- Index on `EntityId` for entity change history
- Index on `UserId` for user activity tracking
- Index on `CreatedAt` for time-based audit queries
- Index on `Action` for action-based filtering

### PromoCodes Table

Flexible discount system with usage limits and targeting.

**Key Fields:**
- `Id` (PK): Unique identifier
- `Code`: Promo code string
- `DiscountType`: DiscountType enum (Percentage, FixedAmount)
- `DiscountValue`: Discount amount
- `Currency`: ISO currency code
- `MaxUses`/`CurrentUses`: Usage tracking
- `ValidFrom`/`ValidUntil`: Validity period
- `ApplicableCourseIds`: JSON array of course restrictions

**Business Rules:**
- Unique promo codes across platform
- Usage limits and validity periods
- Course-specific or platform-wide discounts
- Percentage or fixed amount discounts

**Indexes:**
- Unique index on `Code`
- Index on `IsActive` for active promo codes
- Index on `ValidUntil` for expiration queries

### BookingPromoCodes Table

Tracks promo code applications to bookings.

**Key Fields:**
- `Id` (PK): Unique identifier
- `BookingId` (FK): Associated booking
- `PromoCodeId` (FK): Applied promo code
- `DiscountAmount`: Applied discount amount

**Business Rules:**
- One promo code application per booking
- Tracks actual discount applied

**Indexes:**
- Index on `BookingId` for booking discount lookup
- Index on `PromoCodeId` for promo code usage

### UserSessions Table

Session management and security tracking.

**Key Fields:**
- `Id` (PK): Unique identifier
- `UserId` (FK): Session owner
- `SessionToken`: Unique session identifier
- `IpAddress`/`UserAgent`: Security context
- `ExpiresAt`: Session expiration
- `LastActivityAt`: Activity tracking

**Business Rules:**
- Unique session tokens
- Automatic expiration
- Activity tracking for security

**Indexes:**
- Index on `UserId` for user session queries
- Unique index on `SessionToken`
- Index on `ExpiresAt` for cleanup queries

### InstructorRatings Table

Instructor feedback system with reviews.

**Key Fields:**
- `Id` (PK): Unique identifier
- `InstructorId` (FK): Rated instructor
- `StudentId` (FK): Rating student
- `BookingId` (FK): Associated booking
- `Rating`: 1-5 star rating
- `Review`: Review text
- `IsPublic`: Public visibility flag

**Business Rules:**
- One rating per booking
- Rating range 1-5 stars
- Public/private review options

**Indexes:**
- Index on `InstructorId` for instructor ratings
- Index on `StudentId` for student ratings
- Index on `BookingId` for booking-specific ratings
- Index on `Rating` for rating analytics

### CourseRatings Table

Course feedback system with reviews.

**Key Fields:**
- `Id` (PK): Unique identifier
- `CourseId` (FK): Rated course
- `StudentId` (FK): Rating student
- `Rating`: 1-5 star rating
- `Review`: Review text
- `IsPublic`: Public visibility flag

**Business Rules:**
- One rating per student per course
- Rating range 1-5 stars
- Public/private review options

**Indexes:**
- Index on `CourseId` for course ratings
- Index on `StudentId` for student ratings
- Index on `Rating` for rating analytics

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
- `Rescheduled = 5`: Booking was rescheduled

### PaymentStatus
- `Pending = 0`: Payment processing
- `Succeeded = 1`: Payment successful
- `Failed = 2`: Payment failed
- `Cancelled = 3`: Payment cancelled
- `Refunded = 4`: Full refund processed
- `PartiallyRefunded = 5`: Partial refund processed

### PaymentProvider
- `Stripe = 0`: Stripe payment processing
- `PayPal = 1`: PayPal payment processing

### NotificationType
- `BookingConfirmation = 0`: Booking confirmed
- `BookingReminder = 1`: Session reminder
- `BookingCancellation = 2`: Booking cancelled
- `PaymentConfirmation = 3`: Payment successful
- `PaymentFailed = 4`: Payment failed
- `CoursePublished = 5`: Course published
- `EmailVerification = 6`: Email verification
- `PasswordReset = 7`: Password reset
- `GeneralAnnouncement = 8`: System announcement
- `BookingRescheduled = 9`: Booking rescheduled
- `CourseUpdated = 10`: Course updated
- `SessionReminder = 11`: Session reminder
- `CourseCompleted = 12`: Course completed
- `InstructorRating = 13`: Instructor rated

### AuditActionType
- `Create = 0`: Entity created
- `Update = 1`: Entity updated
- `Delete = 2`: Entity deleted
- `Login = 3`: User login
- `Logout = 4`: User logout
- `EmailConfirmation = 5`: Email confirmed
- `PasswordReset = 6`: Password reset
- `BookingCreated = 7`: Booking created
- `BookingUpdated = 8`: Booking updated
- `PaymentProcessed = 9`: Payment processed

### DiscountType
- `Percentage = 0`: Percentage discount
- `FixedAmount = 1`: Fixed amount discount

## Performance Optimizations

### Strategic Indexing
- **Composite Indexes**: Multi-column indexes for common query patterns
- **Covering Indexes**: Include frequently accessed columns
- **Soft Delete Optimization**: Efficient filtering of deleted records
- **Time-based Queries**: Optimized for date range searches

### Query Optimization
- **Foreign Key Indexes**: All foreign keys indexed
- **Status-based Queries**: Indexes on status fields
- **Time-based Filtering**: Indexes on timestamp fields
- **Unique Constraints**: Prevent data integrity issues

## Security Features

### User Security
- **Account Locking**: Failed login attempt tracking
- **Session Management**: Secure session handling with expiration
- **Password Reset**: Secure token-based password reset
- **Soft Deletes**: Data preservation with deletion tracking

### Audit & Compliance
- **Comprehensive Logging**: All critical operations logged
- **Change Tracking**: Before/after JSON for all modifications
- **Security Context**: IP address and user agent tracking
- **Immutable Records**: Audit logs cannot be modified

## Migration Strategy

### Backward Compatibility
- All existing fields preserved
- Sensible defaults for new required fields
- Gradual migration path for existing data
- Non-blocking index creation for production

### Data Migration
- **Phase 1**: Add new tables and fields
- **Phase 2**: Migrate existing data to new structure
- **Phase 3**: Enable new features
- **Phase 4**: Clean up deprecated fields

---

**Optimized Database Schema** - Comprehensive guide to the enhanced MedicalEdu data architecture with advanced features and performance optimizations. 