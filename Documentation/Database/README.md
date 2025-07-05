# Database Documentation

This folder contains comprehensive documentation for the MedicalEdu database design, schema, and implementation.

## 📁 Database Documentation Files

### Core Schema Documentation
- **[DATABASE_SCHEMA.md](DATABASE_SCHEMA.md)** - Complete database schema documentation
  - Entity descriptions and relationships
  - Business rules and constraints
  - Performance considerations and indexing
  - Security requirements
  - Migration strategies

### Visual Schema
- **[database_schema_optimized.dbml](database_schema_optimized.dbml)** - Optimized dbdiagram.io schema file
  - Copy-paste into https://dbdiagram.io/d
  - Interactive visual representation
  - Export to PNG, PDF, or SQL formats

### Schema Optimization
- **[SCHEMA_OPTIMIZATION_SUMMARY.md](SCHEMA_OPTIMIZATION_SUMMARY.md)** - Detailed summary of schema improvements
  - Key issues identified and fixed
  - New features and capabilities
  - Performance optimizations
  - Business flow coverage

## 🗄️ Database Overview

The MedicalEdu platform uses a comprehensive optimized database design with **15 core entities**:

### Core Entities
1. **Users** - Multi-role user management with enhanced security
2. **Courses** - Rich course metadata with categorization and pricing
3. **CourseMaterials** - File upload and content management with progress tracking
4. **AvailabilitySlots** - Advanced scheduling with group sessions and recurring patterns
5. **Bookings** - Complete booking lifecycle with rescheduling and virtual sessions
6. **Payments** - Multi-provider payment processing with partial refunds
7. **Enrollments** - Student course enrollment with detailed progress tracking
8. **CourseProgress** - Granular progress tracking per course material
9. **Notifications** - Multi-channel notification system with scheduling
10. **AuditLogs** - Comprehensive activity tracking and security
11. **PromoCodes** - Flexible discount system with usage limits
12. **BookingPromoCodes** - Promo code application tracking
13. **UserSessions** - Session management and security
14. **InstructorRatings** - Instructor feedback system
15. **CourseRatings** - Course feedback system

### Key Features
- **GUID Primary Keys** for better distribution and security
- **UTC Timestamps** for consistency across time zones
- **Soft Deletes** using `deleted_at` timestamps
- **Comprehensive Audit Trail** for all critical operations
- **Multi-Provider Payment Support** (Stripe, PayPal)
- **Role-Based Access Control** with enhanced security
- **Multi-Currency Support** throughout the system
- **Group Session Support** with participant tracking
- **Recurring Availability** with JSON pattern support
- **Promotional Features** with flexible discount system
- **Rating System** for both instructors and courses
- **Progress Tracking** with detailed analytics

## 🚀 Quick Start

### View Complete Documentation
Read [DATABASE_SCHEMA.md](DATABASE_SCHEMA.md) for detailed information about:
- Entity relationships and constraints
- Business rules and validation
- Performance optimization strategies
- Security considerations
- Migration and deployment guidelines

### View Optimization Summary
Read [SCHEMA_OPTIMIZATION_SUMMARY.md](SCHEMA_OPTIMIZATION_SUMMARY.md) for:
- Key improvements and fixes
- New features and capabilities
- Performance enhancements
- Business flow coverage

### Generate Visual Diagram
1. Go to https://dbdiagram.io/d
2. Copy the content from `database_schema_optimized.dbml`
3. Paste into the editor
4. Save to generate your interactive diagram

### Database Setup
```bash
# Run Entity Framework migrations
dotnet ef database update

# Verify database creation
dotnet ef database info
```

## 📊 Schema Highlights

### Enhanced User Management
- Multi-role system (Admin, Instructor, Student)
- Email verification workflow with password reset
- Account locking and failed login tracking
- Session management with activity tracking
- Timezone support for booking display
- Profile management with contact information

### Advanced Course System
- Instructor course creation and publishing
- Rich metadata (tags, categories, duration)
- Content upload with file metadata and duration
- Access control (free vs. premium content)
- Enrollment tracking with detailed progress monitoring
- Video intro support and thumbnail management

### Comprehensive Booking & Payment
- Availability slot management with group sessions
- Complete booking lifecycle with rescheduling
- Virtual session support with meeting URLs
- Multi-provider payment processing
- Partial refunds and refund reasons
- Promo code integration

### Advanced Notifications & Audit
- Multi-channel notification system (Email, SMS, Push)
- Scheduled notifications for reminders
- System-wide notification management
- Email delivery tracking
- Comprehensive audit logging
- Security and compliance features

### New Features
- **Progress Tracking**: Granular completion tracking per material
- **Rating System**: Instructor and course feedback
- **Promotional Features**: Flexible discount codes
- **Group Sessions**: Multi-participant slot management
- **Recurring Slots**: JSON-based recurring patterns
- **Session Management**: Secure session handling

## 🔧 Technical Details

### Database Technology
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Migration Tool**: EF Core Migrations
- **Connection**: Connection string configuration

### Performance Optimizations
- Strategic indexing for common queries
- Composite indexes for complex operations
- Covering indexes for frequently accessed data
- Soft delete filtering optimization
- Time-based query optimization
- Query optimization recommendations

### Security Features
- Password hashing and encryption
- Account locking and failed login tracking
- Session management with expiration
- Role-based authorization
- Comprehensive audit trail for compliance
- Input validation and sanitization

## 📈 Future Enhancements

### Planned Features
- Database partitioning for audit logs
- Read replicas for reporting
- Advanced caching strategies
- Data archiving policies
- Real-time analytics dashboard
- Advanced reporting capabilities

### Monitoring & Maintenance
- Database performance monitoring
- Automated backup strategies
- Index maintenance schedules
- Data retention policies
- Session cleanup automation
- Audit log rotation

---

**Database Documentation** - Comprehensive guide to the optimized MedicalEdu data architecture. 