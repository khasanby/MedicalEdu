# Database Schema Diagram

## Overview

This document provides information about the visual database schema diagram for the MedicalEdu platform.

## Diagram Files

### Primary Schema File
- **[database_schema_optimized.dbml](database_schema_optimized.dbml)** - Complete optimized database schema
  - 15 core entities with advanced features
  - Comprehensive relationships and constraints
  - Performance-optimized indexes
  - Multi-currency and security features

## How to View the Diagram

### Using dbdiagram.io
1. Go to https://dbdiagram.io/d
2. Copy the entire content from `database_schema_optimized.dbml`
3. Paste into the dbdiagram.io editor
4. The diagram will automatically render with all entities and relationships

### Diagram Features
- **Interactive Relationships**: Click on entities to see connections
- **Export Options**: PNG, PDF, or SQL formats
- **Real-time Updates**: Changes reflect immediately
- **Zoom and Pan**: Navigate large schemas easily

## Schema Overview

### Core Entities (15 Total)
1. **Users** - Multi-role user management with security features
2. **Courses** - Rich course metadata with categorization
3. **CourseMaterials** - Content management with progress tracking
4. **AvailabilitySlots** - Advanced scheduling with group sessions
5. **Bookings** - Complete booking lifecycle with rescheduling
6. **Payments** - Multi-provider payment processing
7. **Enrollments** - Student enrollment with progress tracking
8. **CourseProgress** - Granular progress tracking per material
9. **Notifications** - Multi-channel notification system
10. **AuditLogs** - Comprehensive activity tracking
11. **PromoCodes** - Flexible discount system
12. **BookingPromoCodes** - Promo code application tracking
13. **UserSessions** - Session management and security
14. **InstructorRatings** - Instructor feedback system
15. **CourseRatings** - Course feedback system

### Key Relationships
- **User → Course**: One instructor can create many courses
- **Course → CourseMaterial**: One course can have many materials
- **Course → AvailabilitySlot**: One course can have many time slots
- **AvailabilitySlot → Booking**: One slot can have multiple bookings (group sessions)
- **Booking → Payment**: One booking can have multiple payments (refunds)
- **User → Enrollment**: One student can enroll in many courses
- **Enrollment → CourseProgress**: One enrollment tracks progress per material
- **User → Notification**: One user can have many notifications
- **User → AuditLog**: One user can perform many logged actions
- **Booking → PromoCode**: Many-to-many through BookingPromoCode
- **User → Rating**: Users can rate instructors and courses

### Advanced Features
- **Soft Deletes**: `deleted_at` timestamps for data preservation
- **Multi-Currency**: Currency support throughout the system
- **Group Sessions**: Multi-participant slot management
- **Recurring Slots**: JSON pattern support for recurring availability
- **Progress Tracking**: Detailed completion tracking per material
- **Rating System**: Both instructor and course ratings
- **Promotional Features**: Flexible discount codes
- **Session Management**: Secure session handling
- **Security Features**: Account locking, failed login tracking

## Performance Optimizations

### Strategic Indexing
- **Composite Indexes**: Multi-column indexes for common queries
- **Covering Indexes**: Include frequently accessed columns
- **Soft Delete Optimization**: Efficient filtering of deleted records
- **Time-based Queries**: Optimized for date range searches

### Query Optimization
- **Foreign Key Indexes**: All foreign keys indexed
- **Status-based Queries**: Indexes on status fields
- **Time-based Filtering**: Indexes on timestamp fields
- **Unique Constraints**: Prevent data integrity issues

## Migration Notes

### Schema Evolution
- **Backward Compatible**: All existing fields preserved
- **Gradual Migration**: Non-blocking index creation
- **Default Values**: Sensible defaults for new required fields
- **Data Preservation**: Soft deletes maintain data integrity

### Key Changes from Previous Version
- **Removed Redundancy**: Eliminated redundant `instructor_id` from availability slots
- **Enhanced Security**: Added account locking and session management
- **Progress Tracking**: New granular progress tracking per material
- **Rating System**: Instructor and course feedback capabilities
- **Promotional Features**: Flexible discount code system
- **Multi-Currency**: Currency support throughout the system

## Export Options

### Available Formats
- **PNG**: High-resolution image for documentation
- **PDF**: Vector format for printing and sharing
- **SQL**: Database creation scripts
- **JSON**: Schema metadata for tooling

### Recommended Usage
- **Documentation**: Use PNG for README files
- **Presentations**: Use PDF for slides and reports
- **Development**: Use SQL for database setup
- **Tooling**: Use JSON for schema analysis

---

**Database Schema Diagram** - Visual representation of the optimized MedicalEdu data architecture with comprehensive features and performance optimizations.