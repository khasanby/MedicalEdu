# Domain Aggregates

This directory contains the domain aggregates for the MedicalEdu application. Aggregates are the core building blocks of Domain-Driven Design (DDD) that encapsulate business logic and ensure data consistency.

## CourseAggregate

The `CourseAggregate` is a comprehensive aggregate that manages the lifecycle and business rules of a course in the MedicalEdu system.

### Overview

The CourseAggregate encapsulates:
- Course creation and management
- Publishing/unpublishing workflows
- Course material management
- Availability slot management
- Business rules and validations
- Domain event generation

### Key Features

#### 1. **Factory Method**
```csharp
var course = CourseAggregate.Create(
    instructorId: Guid.NewGuid(),
    title: "Advanced Medical Procedures",
    price: Money.Create(99.99m, Currency.Parse("USD")),
    createdBy: "instructor@example.com"
);
```

#### 2. **Course Management**
- **UpdateTitle**: Updates course title with validation
- **UpdateDescription**: Updates course description
- **UpdateShortDescription**: Updates short description
- **UpdatePrice**: Updates price using Money value object
- **UpdateThumbnail**: Updates thumbnail using Url value object
- **UpdateVideoIntro**: Updates video intro URL
- **UpdateDuration**: Updates course duration
- **UpdateMaxStudents**: Updates maximum student capacity
- **UpdateCategory**: Updates course category
- **UpdateTags**: Updates course tags

#### 3. **Publishing Workflow**
```csharp
// Publish the course
course.Publish("instructor@example.com");

// Unpublish the course
course.Unpublish("instructor@example.com");
```

#### 4. **Course Materials Management**
```csharp
// Add course material
course.AddCourseMaterial(material);

// Remove course material
course.RemoveCourseMaterial(materialId);

// Reorder materials
course.ReorderCourseMaterials(new[] { material1Id, material2Id, material3Id });
```

#### 5. **Availability Slots Management**
```csharp
// Add availability slot
course.AddAvailabilitySlot(slot);

// Remove availability slot
course.RemoveAvailabilitySlot(slotId);
```

#### 6. **Business Logic**
- **GetAverageRating**: Calculates average course rating
- **GetEnrollmentCount**: Gets total enrollments
- **IsFull**: Checks if course is at capacity
- **CanEnroll**: Validates if a student can enroll
- **GetPrice**: Returns price as Money value object

### Domain Events

The CourseAggregate raises the following domain events:

| Event | Triggered When | Data |
|-------|----------------|------|
| `CourseCreatedEvent` | Course is created | CourseId, InstructorId, Title |
| `CourseUpdatedEvent` | Course is updated | CourseId, Title |
| `CourseThumbnailUpdatedEvent` | Thumbnail is updated | CourseId |
| `CoursePublishedEvent` | Course is published | CourseId, Title, InstructorId |
| `CourseUnpublishedEvent` | Course is unpublished | CourseId, Title |
| `CourseMaterialAddedEvent` | Material is added | CourseId, MaterialId, MaterialTitle |
| `CourseMaterialRemovedEvent` | Material is removed | CourseId, MaterialId |
| `CourseMaterialsReorderedEvent` | Materials are reordered | CourseId |
| `CourseAvailabilitySlotAddedEvent` | Slot is added | CourseId, SlotId, StartTime |
| `CourseAvailabilitySlotRemovedEvent` | Slot is removed | CourseId, SlotId |

### Business Rules

#### Course Creation
- Instructor ID must be valid
- Title cannot be empty
- Price must be non-negative
- Currency must be valid

#### Course Updates
- Title cannot be empty
- Duration cannot be negative
- Max students must be positive
- All updates track modification metadata

#### Publishing
- Cannot publish already published course
- Cannot unpublish unpublished course
- Publishing sets PublishedAt timestamp

#### Enrollment Validation
- Course must be published
- Student cannot be already enrolled
- Course must not be at capacity

#### Material Management
- Cannot add duplicate materials
- Cannot remove non-existent materials
- Reordering preserves existing materials

### Value Objects Integration

The CourseAggregate leverages the value object toolkit:

- **Money**: For price management with currency safety
- **Currency**: For ISO-4217 compliant currency codes
- **Url**: For thumbnail and video URL validation
- **Email**: For user identification (in events)

### Usage Example

```csharp
// Create a course
var course = CourseAggregate.Create(
    instructorId: instructorId,
    title: "Cardiology Fundamentals",
    price: Money.Create(149.99m, Currency.Parse("USD")),
    createdBy: "dr.smith@medical.edu"
);

// Update course details
course.UpdateDescription("Comprehensive course on cardiology basics");
course.UpdateThumbnail(Url.Create("https://example.com/thumbnail.jpg"));
course.UpdateDuration(480); // 8 hours
course.UpdateMaxStudents(50);

// Add course materials
course.AddCourseMaterial(new CourseMaterial(/* ... */));
course.AddCourseMaterial(new CourseMaterial(/* ... */));

// Publish the course
course.Publish("dr.smith@medical.edu");

// Check business rules
if (course.CanEnroll(studentId))
{
    // Proceed with enrollment
}

// Get course statistics
var averageRating = course.GetAverageRating();
var enrollmentCount = course.GetEnrollmentCount();
var isFull = course.IsFull();
```

### Benefits

1. **Encapsulation**: All course-related logic is contained within the aggregate
2. **Consistency**: Business rules are enforced at the aggregate level
3. **Event-Driven**: Domain events enable loose coupling with other parts of the system
4. **Type Safety**: Value objects prevent invalid data
5. **Auditability**: All changes are tracked with metadata
6. **Testability**: Clear boundaries make testing straightforward

The CourseAggregate provides a robust foundation for course management in the MedicalEdu system, ensuring data integrity and business rule compliance. 