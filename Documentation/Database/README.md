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
- **[database_schema.dbml](database_schema.dbml)** - dbdiagram.io schema file
  - Copy-paste into https://dbdiagram.io/d
  - Interactive visual representation
  - Export to PNG, PDF, or SQL formats

## 🗄️ Database Overview

The MedicalEdu platform uses a comprehensive database design with **9 core entities**:

### Core Entities
1. **Users** - Multi-role user management (Admin, Instructor, Student)
2. **Courses** - Course creation and publishing workflow
3. **CourseMaterials** - File upload and content management
4. **AvailabilitySlots** - Instructor scheduling system
5. **Bookings** - Student booking lifecycle management
6. **Payments** - Multi-provider payment processing
7. **Enrollments** - Student course enrollment tracking
8. **Notifications** - System-wide notification management
9. **AuditLogs** - Comprehensive activity tracking

### Key Features
- **GUID Primary Keys** for better distribution and security
- **UTC Timestamps** for consistency across time zones
- **Soft Deletes** using status flags rather than hard deletes
- **Comprehensive Audit Trail** for all critical operations
- **Multi-Provider Payment Support** (Stripe, PayPal)
- **Role-Based Access Control** with JWT authentication

## 🚀 Quick Start

### View Complete Documentation
Read [DATABASE_SCHEMA.md](DATABASE_SCHEMA.md) for detailed information about:
- Entity relationships and constraints
- Business rules and validation
- Performance optimization strategies
- Security considerations
- Migration and deployment guidelines

### Generate Visual Diagram
1. Go to https://dbdiagram.io/d
2. Copy the content from `database_schema.dbml`
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

### User Management
- Multi-role system (Admin, Instructor, Student)
- Email verification workflow
- Timezone support for booking display
- Profile management with contact information

### Course System
- Instructor course creation and publishing
- Content upload with file metadata
- Access control (free vs. premium content)
- Enrollment tracking and progress monitoring

### Booking & Payment
- Availability slot management
- Complete booking lifecycle
- Multi-provider payment processing
- Refund and cancellation handling

### Notifications & Audit
- System-wide notification management
- Email delivery tracking
- Comprehensive audit logging
- Security and compliance features

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
- Query optimization recommendations

### Security Features
- Password hashing and encryption
- Role-based authorization
- Audit trail for compliance
- Input validation and sanitization

## 📈 Future Enhancements

### Planned Features
- Database partitioning for audit logs
- Read replicas for reporting
- Advanced caching strategies
- Data archiving policies

### Monitoring & Maintenance
- Database performance monitoring
- Automated backup strategies
- Index maintenance schedules
- Data retention policies

---

**Database Documentation** - Comprehensive guide to the MedicalEdu data architecture. 