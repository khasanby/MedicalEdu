using Microsoft.EntityFrameworkCore;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.ValueObjects;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.DataAccess;

namespace MedicalEdu.Infrastructure.DataAccess;

/// <summary>
/// Entity Framework Core DbContext for MedicalEdu application.
/// </summary>
public class MedicalEduDbContext : DbContext, IMedicalEduDbContext
{
    public MedicalEduDbContext(DbContextOptions<MedicalEduDbContext> options) : base(options)
    {
    }

    // DbSets for aggregate roots and standalone entities
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    // Supporting entities
    public DbSet<CourseMaterial> CourseMaterials { get; set; }
    public DbSet<CourseRating> CourseRatings { get; set; }
    public DbSet<CourseProgress> CourseProgresses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<InstructorRating> InstructorRatings { get; set; }
    public DbSet<BookingPromoCode> BookingPromoCodes { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        ConfigureUser(modelBuilder);
        
        // Configure Course entity
        ConfigureCourse(modelBuilder);
        
        // Configure Booking entity
        ConfigureBooking(modelBuilder);
        
        // Configure AvailabilitySlot entity
        ConfigureAvailabilitySlot(modelBuilder);
        
        // Configure Payment entity
        ConfigurePayment(modelBuilder);
        
        // Configure PromoCode entity
        ConfigurePromoCode(modelBuilder);
        
        // Configure Notification entity
        ConfigureNotification(modelBuilder);
        
        // Configure AuditLog entity
        ConfigureAuditLog(modelBuilder);
        
        // Configure supporting entities
        ConfigureCourseMaterial(modelBuilder);
        ConfigureCourseRating(modelBuilder);
        ConfigureCourseProgress(modelBuilder);
        ConfigureEnrollment(modelBuilder);
        ConfigureInstructorRating(modelBuilder);
        ConfigureBookingPromoCode(modelBuilder);
        ConfigureUserSession(modelBuilder);
    }

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<User>();

        entity.ToTable("Users");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        
        // Value object conversions
        entity.OwnsOne(e => e.Email, email =>
        {
            email.Property(e => e.Value).HasColumnName("Email").IsRequired().HasMaxLength(255);
        });
        
        entity.OwnsOne(e => e.PasswordHash, password =>
        {
            password.Property(p => p.Hash).HasColumnName("PasswordHash").IsRequired().HasMaxLength(255);
        });
        
        entity.OwnsOne(e => e.Zone, zone =>
        {
            zone.Property(z => z.Value).HasColumnName("TimeZone").IsRequired().HasMaxLength(50);
        });
        
        entity.OwnsOne(e => e.PhoneNumber, phone =>
        {
            phone.Property(p => p.Value).HasColumnName("PhoneNumber").HasMaxLength(20);
        });
        
        entity.OwnsOne(e => e.ProfilePictureUrl, url =>
        {
            url.Property(u => u.Value).HasColumnName("ProfilePictureUrl").HasMaxLength(500);
        });

        entity.Property(e => e.Role).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.DeletedAt);
        entity.Property(e => e.IsActive).IsRequired();
        entity.Property(e => e.EmailConfirmed).IsRequired();
        entity.Property(e => e.EmailConfirmationToken).HasMaxLength(255);
        entity.Property(e => e.EmailConfirmationTokenExpiry);
        entity.Property(e => e.PasswordResetToken).HasMaxLength(255);
        entity.Property(e => e.PasswordResetTokenExpiry);
        entity.Property(e => e.LastLoginAt);
        entity.Property(e => e.FailedLoginAttempts).IsRequired();
        entity.Property(e => e.LockedUntil);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Indexes
        entity.HasIndex(e => e.Email.Value).IsUnique();
        entity.HasIndex(e => e.EmailConfirmationToken);
        entity.HasIndex(e => e.PasswordResetToken);
    }

    private void ConfigureCourse(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Course>();

        entity.ToTable("Courses");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
        entity.Property(e => e.Description).HasMaxLength(2000);
        entity.Property(e => e.ShortDescription).HasMaxLength(500);
        
        // Value object conversions
        entity.OwnsOne(e => e.Price, price =>
        {
            price.Property(p => p.Amount).HasColumnName("Price").IsRequired();
            price.OwnsOne(p => p.Currency, currency =>
            {
                currency.Property(c => c.Code).HasColumnName("CurrencyCode").IsRequired().HasMaxLength(3);
            });
        });
        
        entity.OwnsOne(e => e.ThumbnailUrl, url =>
        {
            url.Property(u => u.Value).HasColumnName("ThumbnailUrl").HasMaxLength(500);
        });

        entity.Property(e => e.InstructorId).IsRequired();
        entity.Property(e => e.Category).IsRequired();
        entity.Property(e => e.DifficultyLevel).IsRequired();
        entity.Property(e => e.Duration).IsRequired();
        entity.Property(e => e.MaxStudents).IsRequired();
        entity.Property(e => e.CurrentStudents).IsRequired();
        entity.Property(e => e.IsPublished).IsRequired();
        entity.Property(e => e.IsActive).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.PublishedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne(e => e.Instructor)
            .WithMany(u => u.InstructorCourses)
            .HasForeignKey(e => e.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.InstructorId);
        entity.HasIndex(e => e.Category);
        entity.HasIndex(e => e.IsPublished);
        entity.HasIndex(e => e.IsActive);
    }

    private void ConfigureBooking(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Booking>();

        entity.ToTable("Bookings");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.AvailabilitySlotId).IsRequired();
        entity.Property(e => e.Status).IsRequired();
        entity.Property(e => e.Amount).IsRequired();
        
        // Value object conversion
        entity.OwnsOne(e => e.Currency, currency =>
        {
            currency.Property(c => c.Code).HasColumnName("CurrencyCode").IsRequired().HasMaxLength(3);
        });

        entity.Property(e => e.StudentNotes).HasMaxLength(1000);
        entity.Property(e => e.InstructorNotes).HasMaxLength(1000);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.ConfirmedAt);
        entity.Property(e => e.CancelledAt);
        entity.Property(e => e.CancellationReason).HasMaxLength(500);
        entity.Property(e => e.RescheduledFromBookingId);
        entity.Property(e => e.MeetingUrl).HasMaxLength(500);
        entity.Property(e => e.MeetingNotes).HasMaxLength(1000);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne(e => e.Student)
            .WithMany(u => u.StudentBookings)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<AvailabilitySlot>()
            .WithMany(s => s.Bookings)
            .HasForeignKey(e => e.AvailabilitySlotId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.AvailabilitySlotId);
        entity.HasIndex(e => e.Status);
        entity.HasIndex(e => e.CreatedAt);
    }

    private void ConfigureAvailabilitySlot(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AvailabilitySlot>();

        entity.ToTable("AvailabilitySlots");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.CourseId).IsRequired();
        entity.Property(e => e.InstructorId).IsRequired();
        entity.Property(e => e.StartTimeUtc).IsRequired();
        entity.Property(e => e.EndTimeUtc).IsRequired();
        entity.Property(e => e.IsBooked).IsRequired();
        entity.Property(e => e.Price).IsRequired();
        
        // Value object conversion
        entity.OwnsOne(e => e.Currency, currency =>
        {
            currency.Property(c => c.Code).HasColumnName("CurrencyCode").IsRequired().HasMaxLength(3);
        });

        entity.Property(e => e.MaxParticipants).IsRequired();
        entity.Property(e => e.CurrentParticipants).IsRequired();
        entity.Property(e => e.Notes).HasMaxLength(1000);
        entity.Property(e => e.IsRecurring).IsRequired();
        entity.Property(e => e.RecurringPattern).HasMaxLength(500);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne(e => e.Course)
            .WithMany(c => c.AvailabilitySlots)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Instructor)
            .WithMany(u => u.InstructorAvailabilitySlots)
            .HasForeignKey(e => e.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.CourseId);
        entity.HasIndex(e => e.InstructorId);
        entity.HasIndex(e => e.StartTimeUtc);
        entity.HasIndex(e => e.IsBooked);
    }

    private void ConfigurePayment(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Payment>();

        entity.ToTable("Payments");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.BookingId).IsRequired();
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.Amount).IsRequired();
        
        // Value object conversion
        entity.OwnsOne(e => e.Currency, currency =>
        {
            currency.Property(c => c.Code).HasColumnName("CurrencyCode").IsRequired().HasMaxLength(3);
        });

        entity.Property(e => e.Status).IsRequired();
        entity.Property(e => e.Provider).IsRequired();
        entity.Property(e => e.TransactionId).HasMaxLength(255);
        entity.Property(e => e.ProviderTransactionId).HasMaxLength(255);
        entity.Property(e => e.FailureReason).HasMaxLength(500);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.ProcessedAt);
        entity.Property(e => e.FailedAt);
        entity.Property(e => e.RefundedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne<Booking>()
            .WithMany(b => b.Payments)
            .HasForeignKey(e => e.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<User>()
            .WithMany(u => u.Payments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.BookingId);
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.Status);
        entity.HasIndex(e => e.TransactionId);
        entity.HasIndex(e => e.ProviderTransactionId);
    }

    private void ConfigurePromoCode(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PromoCode>();

        entity.ToTable("PromoCodes");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Description).HasMaxLength(500);
        entity.Property(e => e.DiscountType).IsRequired();
        entity.Property(e => e.DiscountValue).IsRequired();
        entity.Property(e => e.MaxUses).IsRequired();
        entity.Property(e => e.CurrentUses).IsRequired();
        entity.Property(e => e.IsActive).IsRequired();
        entity.Property(e => e.ValidFrom).IsRequired();
        entity.Property(e => e.ValidUntil);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Indexes
        entity.HasIndex(e => e.Code).IsUnique();
        entity.HasIndex(e => e.IsActive);
        entity.HasIndex(e => e.ValidFrom);
        entity.HasIndex(e => e.ValidUntil);
    }

    private void ConfigureNotification(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Notification>();

        entity.ToTable("Notifications");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.UserId).IsRequired();
        entity.Property(e => e.Type).IsRequired();
        entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
        entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
        entity.Property(e => e.IsRead).IsRequired();
        entity.Property(e => e.ReadAt);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne<User>()
            .WithMany(u => u.Notifications)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.Type);
        entity.HasIndex(e => e.IsRead);
        entity.HasIndex(e => e.CreatedAt);
    }

    private void ConfigureAuditLog(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AuditLog>();

        entity.ToTable("AuditLogs");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.EntityName).IsRequired().HasMaxLength(100);
        entity.Property(e => e.EntityId).IsRequired();
        entity.Property(e => e.Action).IsRequired();
        entity.Property(e => e.OldValues).HasMaxLength(4000);
        entity.Property(e => e.NewValues).HasMaxLength(4000);
        entity.Property(e => e.UserId).HasMaxLength(100);
        entity.Property(e => e.IpAddress).HasMaxLength(45);
        entity.Property(e => e.UserAgent).HasMaxLength(500);
        entity.Property(e => e.CreatedAt).IsRequired();

        // Indexes
        entity.HasIndex(e => e.EntityName);
        entity.HasIndex(e => e.EntityId);
        entity.HasIndex(e => e.Action);
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.CreatedAt);
    }

    private void ConfigureCourseMaterial(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CourseMaterial>();

        entity.ToTable("CourseMaterials");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.CourseId).IsRequired();
        entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
        entity.Property(e => e.Description).HasMaxLength(1000);
        entity.Property(e => e.Type).IsRequired();
        entity.Property(e => e.Order).IsRequired();
        entity.Property(e => e.IsActive).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne<Course>()
            .WithMany(c => c.Materials)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        entity.HasIndex(e => e.CourseId);
        entity.HasIndex(e => e.Type);
        entity.HasIndex(e => e.Order);
    }

    private void ConfigureCourseRating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CourseRating>();

        entity.ToTable("CourseRatings");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.CourseId).IsRequired();
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.Rating).IsRequired();
        entity.Property(e => e.Comment).HasMaxLength(1000);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne<Course>()
            .WithMany(c => c.Ratings)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<User>()
            .WithMany(u => u.CourseRatings)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.CourseId);
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.Rating);
    }

    private void ConfigureCourseProgress(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CourseProgress>();

        entity.ToTable("CourseProgresses");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.CourseId).IsRequired();
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.ProgressPercentage).IsRequired();
        entity.Property(e => e.CompletedAt);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne<Course>()
            .WithMany(c => c.Progresses)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<User>()
            .WithMany(u => u.CourseProgresses)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.CourseId);
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.ProgressPercentage);
    }

    private void ConfigureEnrollment(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Enrollment>();

        entity.ToTable("Enrollments");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.CourseId).IsRequired();
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.Status).IsRequired();
        entity.Property(e => e.EnrolledAt).IsRequired();
        entity.Property(e => e.CompletedAt);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne<Course>()
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<User>()
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.CourseId);
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.Status);
        entity.HasIndex(e => e.EnrolledAt);
    }

    private void ConfigureInstructorRating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<InstructorRating>();

        entity.ToTable("InstructorRatings");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.InstructorId).IsRequired();
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.Rating).IsRequired();
        entity.Property(e => e.Comment).HasMaxLength(1000);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne<User>()
            .WithMany(u => u.InstructorRatingsGiven)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<User>()
            .WithMany(u => u.InstructorRatingsReceived)
            .HasForeignKey(e => e.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.InstructorId);
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.Rating);
    }

    private void ConfigureBookingPromoCode(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<BookingPromoCode>();

        entity.ToTable("BookingPromoCodes");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.BookingId).IsRequired();
        entity.Property(e => e.PromoCodeId).IsRequired();
        entity.Property(e => e.DiscountAmount).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.CreatedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne<Booking>()
            .WithMany(b => b.BookingPromoCodes)
            .HasForeignKey(e => e.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<PromoCode>()
            .WithMany()
            .HasForeignKey(e => e.PromoCodeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.BookingId);
        entity.HasIndex(e => e.PromoCodeId);
    }

    private void ConfigureUserSession(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<UserSession>();

        entity.ToTable("UserSessions");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.UserId).IsRequired();
        entity.Property(e => e.SessionToken).IsRequired().HasMaxLength(255);
        entity.Property(e => e.ExpiresAt).IsRequired();
        entity.Property(e => e.IsActive).IsRequired();
        entity.Property(e => e.IpAddress).HasMaxLength(45);
        entity.Property(e => e.UserAgent).HasMaxLength(500);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.LastActivityAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);

        // Relationships
        entity.HasOne<User>()
            .WithMany(u => u.UserSessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.SessionToken).IsUnique();
        entity.HasIndex(e => e.ExpiresAt);
        entity.HasIndex(e => e.IsActive);
    }
} 