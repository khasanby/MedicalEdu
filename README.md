# MedicalEdu - Modern Medical Education Platform

## 📋 Project Summary

**MedicalEdu** is a comprehensive medical education platform designed to bridge the gap between healthcare professionals and students through interactive learning experiences. The platform facilitates secure user registration, course management, content delivery, booking systems, and payment processing to create a seamless educational ecosystem.

### 🎯 Mission
We are building a modern medical education website that empowers healthcare professionals to share their expertise while providing students with access to high-quality, structured learning experiences. The platform combines cutting-edge technology with healthcare education best practices to deliver an exceptional user experience.

## ✨ Key Features

### 🔐 **User Management & Authentication**
- **Multi-role System**: Support for Students, Instructors, and Administrators
- **Secure Registration**: Email verification with confirmation tokens
- **JWT Authentication**: Stateless authentication with role-based access control
- **Profile Management**: User profiles with timezone preferences and contact information

### 📚 **Course Management**
- **Course Creation**: Instructors can create and publish courses with rich content
- **Content Upload**: Support for multiple file types (PDF, video, documents)
- **Publishing Workflow**: Draft to published state management
- **Access Control**: Free and premium content with enrollment tracking

### 📅 **Booking & Scheduling System**
- **Availability Management**: Instructors can define available time slots
- **Smart Booking**: Students can browse and book available sessions
- **Time Zone Support**: Automatic conversion to user's local time
- **Booking Lifecycle**: Complete workflow from request to completion

### 💳 **Payment Integration**
- **Multi-Provider Support**: Stripe and PayPal integration
- **Secure Transactions**: Webhook-based payment confirmation
- **Refund Management**: Complete refund and cancellation handling
- **Financial Tracking**: Comprehensive payment history and reporting

### 🔔 **Communication & Notifications**
- **Email Notifications**: Automated reminders and confirmations
- **In-App Notifications**: Real-time updates and alerts
- **Reminder System**: 24-hour booking reminders
- **Status Updates**: Booking and payment status notifications

### 📊 **Analytics & Reporting**
- **User Analytics**: Registration, engagement, and completion metrics
- **Financial Reports**: Revenue tracking and payment analytics
- **Course Performance**: Enrollment and completion statistics
- **Audit Logging**: Complete system activity tracking

## 🏗️ Technical Architecture

### **Backend Stack**
- **Framework**: ASP.NET Core 8.0 with Clean Architecture
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT tokens with role-based authorization
- **File Storage**: Azure Blob Storage for course materials
- **Payment Processing**: Stripe and PayPal APIs
- **Email Service**: SMTP integration for notifications

### **Frontend Stack**
- **Framework**: Modern JavaScript/TypeScript framework
- **UI Components**: Responsive design with accessibility features
- **State Management**: Client-side state management
- **Real-time Updates**: WebSocket integration for live notifications

### **Infrastructure**
- **Cloud Platform**: Azure hosting with scalability
- **Database**: SQL Server with optimized indexing
- **Caching**: Redis for performance optimization
- **Monitoring**: Application insights and logging
- **Security**: HTTPS, data encryption, and audit trails

## 📁 Project Structure

```
MedicalEdu/
├── MedicalEdu.Api/           # Web API Controllers & Configuration
├── MedicalEdu.Application/   # Business Logic & Services
├── MedicalEdu.Domain/        # Entities, Enums & Domain Logic
├── MedicalEdu.Infrastructure/# Data Access & External Services
├── MedicalEdu.Bootstrap/     # Dependency Injection Setup
├── MedicalEdu.Migrations/    # Database Migration Tool
└── Documentation/            # Project Documentation
    ├── Database/            # Database & Migration Documentation
    ├── API/                # API Documentation
    ├── Architecture/       # System Architecture
    └── Features/           # Feature Documentation
```

## 🗄️ Database Schema

The platform features a comprehensive database design with 15 core entities:

### Core Entities
- **Users**: Multi-role user management with email verification
- **UserSessions**: User session management and authentication
- **Courses**: Course creation and publishing workflow
- **CourseMaterials**: File upload and content management
- **CourseRatings**: Student course ratings and reviews
- **Enrollments**: Student course enrollment tracking
- **CourseProgresses**: Student progress tracking per course material

### Booking & Scheduling
- **AvailabilitySlots**: Instructor scheduling system
- **Bookings**: Student booking lifecycle management
- **BookingPromoCodes**: Promotional code application to bookings

### Payment & Financial
- **Payments**: Multi-provider payment processing
- **PromoCodes**: Discount and promotional code management

### Communication & Analytics
- **Notifications**: System-wide notification management
- **InstructorRatings**: Student ratings for instructors
- **AuditLogs**: Comprehensive activity tracking

### Database Features
- **UUID Primary Keys**: All entities use UUID primary keys for security
- **Audit Trail**: Comprehensive audit logging with user tracking
- **Soft Deletes**: Support for soft deletion where applicable
- **Optimized Indexes**: Performance-optimized database indexes
- **Foreign Key Constraints**: Proper referential integrity
- **Time Zone Support**: UTC timestamps with timezone awareness

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server (Local or Azure)
- Azure Storage Account (for file uploads)
- Stripe/PayPal Developer Accounts

### Installation
1. Clone the repository
2. Configure connection strings in `appsettings.json`
3. Run Entity Framework migrations
4. Set up Azure Blob Storage
5. Configure payment provider credentials
6. Start the application

### Development Setup
```bash
# Clone repository
git clone https://github.com/your-org/MedicalEdu.git

# Navigate to project
cd MedicalEdu

# Restore dependencies
dotnet restore

# Run database migrations
cd MedicalEdu.Migrations
dotnet run

# Start development server
cd ..
dotnet run --project MedicalEdu.Api
```

### Database Migration Tool

The project includes a standalone migration tool (`MedicalEdu.Migrations`) that handles database creation and migration application. For detailed documentation, see [Database Documentation](Documentation/Database/README.md).

#### Features
- **Standalone Console Application**: Runs independently of the main API
- **Environment-Specific Configuration**: Supports Development, Staging, and Production environments
- **Comprehensive Logging**: Detailed logging of migration progress and database operations
- **Error Handling**: Graceful handling of connection issues and migration conflicts
- **Database Creation**: Automatically creates the database if it doesn't exist

#### Usage
```bash
# Navigate to migrations directory
cd MedicalEdu.Migrations

# Run migrations (uses appsettings.json by default)
dotnet run

# Run with specific environment
dotnet run --environment Production

# Run with custom configuration
dotnet run --configuration Release
```

#### Configuration
The migration tool uses the same configuration structure as the main API:
- `appsettings.json` - Default configuration
- `appsettings.Development.json` - Development environment
- `appsettings.Production.json` - Production environment

#### Migration Process
1. **Connection**: Establishes database connection
2. **Database Creation**: Creates database if it doesn't exist
3. **Migration History**: Creates `__EFMigrationsHistory` table
4. **Migration Application**: Applies pending migrations in order
5. **Verification**: Confirms all migrations are applied successfully

#### Troubleshooting
- **Database Already Exists**: If tables exist from previous runs, drop the database manually or use EF Core CLI
- **Connection Issues**: Verify connection string in `appsettings.json`
- **Migration Conflicts**: Ensure no pending migrations exist before running the tool

> 📖 **For comprehensive database documentation, including troubleshooting, best practices, and CI/CD integration, see [Database Documentation](Documentation/Database/README.md)**

## 🔧 Configuration

### Environment Variables
- `ConnectionStrings:DefaultConnection`: Database connection string
- `AzureStorage:ConnectionString`: Blob storage connection
- `Stripe:SecretKey`: Stripe API secret key
- `PayPal:ClientId`: PayPal client identifier
- `Email:SMTP`: Email service configuration
- `JWT:SecretKey`: JWT token signing key

### App Settings
- Database connection strings
- External service configurations
- Feature flags and toggles
- Logging and monitoring settings

## 🧪 Testing

### Test Coverage
- Unit tests for business logic
- Integration tests for API endpoints
- Database migration testing
- Payment flow testing
- User workflow testing

### Test Commands
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test MedicalEdu.Tests

# Generate coverage report
dotnet test --collect:"XPlat Code Coverage"
```

## 📈 Deployment

### Production Deployment
- Azure App Service hosting
- SQL Database with geo-replication
- CDN for static content delivery
- Application Insights monitoring
- Automated CI/CD pipeline

### Environment Management
- Development environment
- Staging environment
- Production environment
- Feature branch deployments

## 🔒 Security Features

- **Data Protection**: Encryption at rest and in transit
- **Authentication**: Multi-factor authentication support
- **Authorization**: Role-based access control
- **Audit Logging**: Complete system activity tracking
- **Input Validation**: Comprehensive data validation
- **SQL Injection Prevention**: Parameterized queries
- **XSS Protection**: Output encoding and validation

## 🤝 Contributing

We welcome contributions from developers passionate about healthcare technology:

1. Fork the repository
2. Create a feature branch
3. Implement your changes
4. Add comprehensive tests
5. Submit a pull request

### Development Guidelines
- Follow Clean Architecture principles
- Write comprehensive unit tests
- Use meaningful commit messages
- Update documentation as needed
- Follow C# coding conventions

## 📞 Support & Contact

For technical support or questions:
- **Email**: support@medicaledu.com
- **Documentation**: [docs.medicaledu.com](https://docs.medicaledu.com)
- **Issues**: [GitHub Issues](https://github.com/your-org/MedicalEdu/issues)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Healthcare professionals who provided domain expertise
- Open source community for excellent tools and libraries
- Medical education institutions for feedback and guidance

---

**MedicalEdu** - Empowering the future of medical education through technology.