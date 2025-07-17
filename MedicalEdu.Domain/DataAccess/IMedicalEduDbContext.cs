using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalEdu.Domain.DataAccess;

/// <summary>
/// Database context interface for MedicalEdu application.
/// </summary>
public interface IMedicalEduDbContext
{
    // Transaction Management
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // Entity Sets
    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<CourseProgress> CourseProgresses { get; set; }
    public DbSet<CourseRating> CourseRatings { get; set; }
    public DbSet<InstructorRating> InstructorRatings { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<BookingPromoCode> BookingPromoCodes { get; set; }
    public DbSet<CourseMaterial> CourseMaterials { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
}