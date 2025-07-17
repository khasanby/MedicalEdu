using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Infrastructure.DataAccess.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace MedicalEdu.Infrastructure.DataAccess;

public sealed class MedicalEduDbContext : DbContext, IMedicalEduDbContext
{
    private readonly AuditSaveChangesInterceptor _auditSaveChangesInterceptor;

    public MedicalEduDbContext(
        DbContextOptions<MedicalEduDbContext> options,
        AuditSaveChangesInterceptor auditSaveChangesInterceptor) : base(options)
    {
        _auditSaveChangesInterceptor = auditSaveChangesInterceptor;
    }

    // Transaction Management Implementation
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
        {
            await Database.CurrentTransaction.CommitAsync(cancellationToken);
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
        {
            await Database.CurrentTransaction.RollbackAsync(cancellationToken);
        }
    }

    // Entity Sets
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    public DbSet<CourseProgress> CourseProgresses { get; set; } = null!;
    public DbSet<CourseRating> CourseRatings { get; set; } = null!;
    public DbSet<InstructorRating> InstructorRatings { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<PromoCode> PromoCodes { get; set; } = null!;
    public DbSet<BookingPromoCode> BookingPromoCodes { get; set; } = null!;
    public DbSet<CourseMaterial> CourseMaterials { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<UserSession> UserSessions { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditSaveChangesInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MedicalEduDbContext).Assembly);
        modelBuilder.ApplyGlobalFilters();
    }
}