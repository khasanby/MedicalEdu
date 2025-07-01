# Domain-Driven Design (DDD) Implementation

## Overview
MedicalEdu implements Domain-Driven Design principles with aggregates, value objects, and domain events to create a robust, business-focused domain model.

## 🏗️ DDD Architecture

### Value Objects
Immutable objects that represent concepts in the domain without identity.

#### **Email Value Object**
```csharp
public sealed class Email : IEquatable<Email>
{
    public string Value { get; }
    
    public static Email Create(string email)
    {
        // Validation logic
        return new Email(email.ToLowerInvariant());
    }
}
```

**Benefits:**
- Encapsulates email validation logic
- Ensures email format consistency
- Prevents invalid email states

#### **Password Value Object**
```csharp
public sealed class Password : IEquatable<Password>
{
    public string Hash { get; }
    
    public static Password Create(string plainTextPassword)
    {
        // Password complexity validation
        return new Password(hashedPassword);
    }
    
    public bool Verify(string plainTextPassword)
    {
        // Password verification logic
    }
}
```

**Benefits:**
- Enforces password complexity rules
- Handles password hashing and verification
- Prevents weak password usage

#### **Money Value Object**
```csharp
public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money Add(Money other)
    {
        // Currency validation and arithmetic
    }
}
```

**Benefits:**
- Prevents currency mixing errors
- Encapsulates monetary calculations
- Ensures financial data integrity

#### **TimeSlot Value Object**
```csharp
public sealed class TimeSlot : IEquatable<TimeSlot>
{
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }
    
    public bool OverlapsWith(TimeSlot other)
    {
        // Overlap detection logic
    }
}
```

**Benefits:**
- Validates time slot constraints
- Prevents scheduling conflicts
- Encapsulates time-related business rules

### Aggregates
Clusters of domain objects that can be treated as a single unit.

#### **UserAggregate**
The User aggregate root manages user-related business logic and ensures data consistency.

```csharp
public sealed class UserAggregate : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public UserRole Role { get; private set; }
    
    public static UserAggregate Create(string name, Email email, Password password, UserRole role)
    {
        // Business validation and creation logic
    }
    
    public void ConfirmEmail()
    {
        // Email confirmation business logic
        AddDomainEvent(new UserEmailConfirmedEvent(Id, Email.Value));
    }
    
    public bool CanCreateCourse()
    {
        return IsActive && EmailConfirmed && Role == UserRole.Instructor;
    }
}
```

**Key Features:**
- **Business Logic Encapsulation**: All user-related business rules are contained within the aggregate
- **Invariant Enforcement**: Ensures data consistency through private setters and validation
- **Domain Events**: Raises events for important state changes
- **Access Control**: Provides methods to check user permissions

#### **CourseAggregate**
The Course aggregate root manages course lifecycle and related business operations.

```csharp
public sealed class CourseAggregate : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public bool IsPublished { get; private set; }
    public Money? Price { get; private set; }
    
    public void Publish()
    {
        if (Materials.Count == 0)
            throw new InvalidOperationException("Cannot publish course without materials");
            
        IsPublished = true;
        AddDomainEvent(new CoursePublishedEvent(Id, Title, InstructorId));
    }
    
    public void AddMaterial(CourseMaterial material)
    {
        // Material addition logic with validation
    }
    
    public bool CanBeAccessedBy(Guid userId, UserRole userRole)
    {
        // Access control logic
    }
}
```

**Key Features:**
- **Lifecycle Management**: Controls course publishing and content management
- **Content Validation**: Ensures courses have required materials before publishing
- **Access Control**: Manages who can access course content
- **Revenue Tracking**: Provides methods for financial calculations

### Domain Events
Events that represent important state changes in the domain.

#### **User Events**
```csharp
public class UserCreatedEvent : UserEvent
{
    public string Email { get; }
    public UserRole Role { get; }
}

public class UserEmailConfirmedEvent : UserEvent
{
    public string Email { get; }
}
```

#### **Course Events**
```csharp
public class CoursePublishedEvent : CourseEvent
{
    public string Title { get; }
    public Guid InstructorId { get; }
}

public class CourseMaterialAddedEvent : CourseEvent
{
    public Guid MaterialId { get; }
    public string MaterialTitle { get; }
}
```

## 🔄 Domain Event Handling

### Event Publishing
Domain events are raised when important state changes occur:

```csharp
private void AddDomainEvent(IDomainEvent domainEvent)
{
    _domainEvents.Add(domainEvent);
}

public void ClearDomainEvents()
{
    _domainEvents.Clear();
}
```

### Event Handling
Events can be handled by:
- **Email Services**: Send confirmation emails
- **Notification Services**: Create in-app notifications
- **Audit Services**: Log important actions
- **Integration Services**: Sync with external systems

## 📊 Benefits of DDD Implementation

### **Business Logic Centralization**
- All business rules are contained within domain objects
- Prevents business logic leakage into application services
- Makes the domain model self-documenting

### **Data Integrity**
- Value objects prevent invalid states
- Aggregates enforce consistency boundaries
- Domain events ensure proper state transitions

### **Testability**
- Domain logic can be tested in isolation
- Value objects are easily unit testable
- Aggregates can be tested without external dependencies

### **Maintainability**
- Clear separation of concerns
- Business rules are explicit and centralized
- Changes to business logic are localized

### **Scalability**
- Domain events enable loose coupling
- Aggregates provide clear boundaries for data consistency
- Value objects reduce complexity in business logic

## 🏗️ Implementation Guidelines

### **Value Object Best Practices**
1. **Immutability**: Value objects should be immutable
2. **Validation**: Include validation in factory methods
3. **Equality**: Implement proper equality comparison
4. **Implicit Conversion**: Provide implicit operators where appropriate

### **Aggregate Best Practices**
1. **Single Responsibility**: Each aggregate should have a clear purpose
2. **Invariant Enforcement**: Protect data integrity through private setters
3. **Domain Events**: Raise events for important state changes
4. **Business Logic**: Encapsulate all related business rules

### **Domain Event Best Practices**
1. **Naming**: Use past tense for event names
2. **Data**: Include relevant data for event handlers
3. **Granularity**: Don't make events too fine-grained
4. **Handling**: Ensure events are handled appropriately

## 🔧 Integration with EF Core

### **Value Object Configuration**
```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value));
    }
}
```

### **Aggregate Configuration**
```csharp
public class UserAggregateConfiguration : IEntityTypeConfiguration<UserAggregate>
{
    public void Configure(EntityTypeBuilder<UserAggregate> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).HasConversion(
            email => email.Value,
            value => Email.Create(value));
    }
}
```

## 📈 Future Enhancements

### **Saga Pattern**
Implement sagas for complex business processes:
- Course enrollment workflow
- Payment processing workflow
- Booking confirmation workflow

### **Specification Pattern**
Add specifications for complex queries:
- Course search specifications
- User filtering specifications
- Booking criteria specifications

### **Domain Services**
Create domain services for cross-aggregate logic:
- Course recommendation service
- Scheduling conflict resolution
- Payment calculation service

---

**DDD Implementation Documentation** - Complete guide to Domain-Driven Design patterns in MedicalEdu. 