using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configures a GUID primary key with ValueGeneratedNever for entities that implement IEntity<Guid>.
    /// </summary>
    public static EntityTypeBuilder<TEntity> HasGuidKey<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IEntity<Guid>
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();
        return builder;
    }

    /// <summary>
    /// Configures common audit properties (CreatedAt, UpdatedAt, CreatedBy, LastModifiedBy).
    /// </summary>
    public static EntityTypeBuilder<TEntity> HasAuditProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class
    {
        builder.Property("CreatedAt").IsRequired();
        builder.Property("UpdatedAt");
        builder.Property("CreatedBy").HasMaxLength(255);
        builder.Property("LastModified");
        builder.Property("LastModifiedBy").HasMaxLength(255);
        return builder;
    }

    /// <summary>
    /// Configures soft delete properties (DeletedAt).
    /// </summary>
    public static EntityTypeBuilder<TEntity> HasSoftDelete<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class
    {
        builder.Property("DeletedAt");
        return builder;
    }

    /// <summary>
    /// Configures common string properties with max length.
    /// </summary>
    public static PropertyBuilder<string> HasMaxLength(this PropertyBuilder<string> builder, int maxLength)
        => builder.HasMaxLength(maxLength);

    /// <summary>
    /// Configures decimal properties with precision.
    /// </summary>
    public static PropertyBuilder<decimal> HasPrecision(this PropertyBuilder<decimal> builder, int precision = 18, int scale = 2)
        => builder.HasPrecision(precision, scale);

    /// <summary>
    /// Configures nullable decimal properties with precision.
    /// </summary>
    public static PropertyBuilder<decimal?> HasPrecision(this PropertyBuilder<decimal?> builder, int precision = 18, int scale = 2)
        => builder.HasPrecision(precision, scale);

    /// <summary>
    /// Configures Email value object conversion with proper validation and constraints.
    /// </summary>
    public static PropertyBuilder<Email> HasEmailConversion(this PropertyBuilder<Email> builder)
        => builder.HasConversion(
            vo => vo.Value,
            str => Email.Create(str))
            .HasMaxLength(255);

    /// <summary>
    /// Configures PasswordHash value object conversion.
    /// </summary>
    public static PropertyBuilder<PasswordHash> HasPasswordHashConversion(this PropertyBuilder<PasswordHash> builder)
        => builder.HasConversion(
            vo => vo.Hash,
            str => PasswordHash.FromHash(str));

    /// <summary>
    /// Configures Currency value object conversion with proper validation.
    /// </summary>
    public static PropertyBuilder<Currency> HasCurrencyConversion(this PropertyBuilder<Currency> builder)
        => builder.HasConversion(
            vo => vo.Code,
            str => Currency.Parse(str));

    /// <summary>
    /// Configures PhoneNumber value object conversion with proper validation and constraints.
    /// </summary>
    public static PropertyBuilder<PhoneNumber> HasPhoneNumberConversion(this PropertyBuilder<PhoneNumber> builder)
        => builder.HasConversion(
            vo => vo.Value,
            str => PhoneNumber.Create(str))
            .HasMaxLength(20);

    /// <summary>
    /// Configures nullable PhoneNumber value object conversion with proper validation and constraints.
    /// </summary>
    public static PropertyBuilder<PhoneNumber?> HasNullablePhoneNumberConversion(this PropertyBuilder<PhoneNumber?> builder)
        => builder.HasConversion(
            vo => vo != null ? vo.Value : null,
            str => str != null ? PhoneNumber.Create(str) : null)
            .HasMaxLength(20);

    /// <summary>
    /// Configures Url value object conversion with proper validation and constraints.
    /// </summary>
    public static PropertyBuilder<Url> HasUrlConversion(this PropertyBuilder<Url> builder)
        => builder.HasConversion(
            vo => vo.Value,
            str => Url.Create(str))
            .HasMaxLength(500);

    /// <summary>
    /// Configures nullable Url value object conversion with proper validation and constraints.
    /// </summary>
    public static PropertyBuilder<Url?> HasNullableUrlConversion(this PropertyBuilder<Url?> builder)
        => builder.HasConversion(
            vo => vo != null ? vo.Value : null,
            str => str != null ? Url.Create(str) : null)
            .HasMaxLength(500);

    /// <summary>
    /// Configures TimeZoneId value object conversion with proper validation.
    /// </summary>
    public static PropertyBuilder<TimeZoneId> HasTimeZoneConversion(this PropertyBuilder<TimeZoneId> builder)
        => builder.HasConversion(
            vo => vo.Id,
            str => TimeZoneId.Create(str));


    /// <summary>
    /// Configures Uuid value object conversion.
    /// </summary>
    public static PropertyBuilder<Uuid> HasUuidConversion(this PropertyBuilder<Uuid> builder)
        => builder.HasConversion(
            vo => vo.Value,
            guid => Uuid.Create(guid));

    /// <summary>
    /// Configures SessionToken value object conversion with proper constraints.
    /// </summary>
    public static PropertyBuilder<SessionToken> HasSessionTokenConversion(this PropertyBuilder<SessionToken> builder)
        => builder.HasConversion(
            vo => vo.Value,
            str => SessionToken.Create(str))
            .HasMaxLength(500);
}