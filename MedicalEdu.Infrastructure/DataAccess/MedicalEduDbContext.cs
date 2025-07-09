using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalEdu.Infrastructure.DataAccess;

public class MedicalEduDbContext : DbContext, IMedicalEduDbContext
{
    public MedicalEduDbContext(DbContextOptions<MedicalEduDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

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
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MedicalEduDbContext).Assembly);
    }
}