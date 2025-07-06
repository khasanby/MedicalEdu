using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MedicalEdu.Domain.DataAccess;

public interface IMedicalEduDbContext
{
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

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins tracking the given entity and entries reachable from the given entity using the EntityState.Added state.
    /// </summary>
    public EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Begins tracking the given entity and entries reachable from the given entity using the EntityState.Modified state.
    /// </summary>
    public EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Begins tracking the given entity and entries reachable from the given entity using the EntityState.Deleted state.
    /// </summary>
    public EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
}