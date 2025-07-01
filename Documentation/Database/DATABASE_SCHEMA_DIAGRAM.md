erDiagram
    User {
        Guid Id PK
        string Name
        string Email
        string PasswordHash
        UserRole Role
        DateTime CreatedAt
        DateTime UpdatedAt
        bool IsActive
        bool EmailConfirmed
        string EmailConfirmationToken
        DateTime EmailConfirmationTokenExpiry
        string TimeZone
        string PhoneNumber
        string ProfilePictureUrl
    }

    Course {
        Guid Id PK
        Guid InstructorId FK
        string Title
        string Description
        bool IsPublished
        decimal Price
        string ThumbnailUrl
        DateTime CreatedAt
        DateTime UpdatedAt
    }

    CourseMaterial {
        Guid Id PK
        Guid CourseId FK
        string Title
        string Description
        string FileUrl
        string FileName
        string ContentType
        long FileSizeBytes
        int Order
        bool IsFree
        DateTime CreatedAt
        DateTime UpdatedAt
    }

    AvailabilitySlot {
        Guid Id PK
        Guid CourseId FK
        Guid InstructorId FK
        DateTime StartTimeUtc
        DateTime EndTimeUtc
        bool IsBooked
        decimal Price
        string Notes
        DateTime CreatedAt
        DateTime UpdatedAt
    }

    Booking {
        Guid Id PK
        Guid CourseId FK
        Guid StudentId FK
        Guid InstructorId FK
        Guid AvailabilitySlotId FK
        BookingStatus Status
        decimal Amount
        string StudentNotes
        string InstructorNotes
        DateTime CreatedAt
        DateTime UpdatedAt
        DateTime ConfirmedAt
        DateTime CancelledAt
        string CancellationReason
    }

    Payment {
        Guid Id PK
        Guid BookingId FK
        Guid UserId FK
        decimal Amount
        string Currency
        PaymentStatus Status
        PaymentProvider Provider
        string ProviderTransactionId
        string ProviderPaymentIntentId
        string ProviderMetadata
        string FailureReason
        DateTime CreatedAt
        DateTime ProcessedAt
        DateTime RefundedAt
        decimal RefundAmount
    }

    Enrollment {
        Guid Id PK
        Guid StudentId FK
        Guid CourseId FK
        DateTime EnrolledAt
        bool IsActive
        int ProgressPercentage
        DateTime CompletedAt
        DateTime UpdatedAt
    }

    Notification {
        Guid Id PK
        Guid UserId FK
        NotificationType Type
        string Title
        string Message
        bool IsRead
        bool EmailSent
        DateTime EmailSentAt
        Guid RelatedEntityId
        string RelatedEntityType
        string Metadata
        DateTime CreatedAt
        DateTime ReadAt
    }

    AuditLog {
        Guid Id PK
        string EntityName
        string EntityId
        AuditActionType Action
        Guid UserId FK
        string IpAddress
        string UserAgent
        string OldValues
        string NewValues
        string Metadata
        DateTime CreatedAt
    }

    User ||--o{ Course : "instructs"
    User ||--o{ Booking : "books as student"
    User ||--o{ Booking : "receives as instructor"
    User ||--o{ Payment : "makes"
    User ||--o{ Enrollment : "enrolls in"
    User ||--o{ Notification : "receives"
    User ||--o{ AuditLog : "performs action"
    User ||--o{ AvailabilitySlot : "owns slots"
    
    Course ||--o{ CourseMaterial : "contains"
    Course ||--o{ AvailabilitySlot : "has slots"
    Course ||--o{ Booking : "booked for"
    Course ||--o{ Enrollment : "enrolled by students"
    
    AvailabilitySlot ||--o| Booking : "reserved by"
    
    Booking ||--o{ Payment : "paid through"