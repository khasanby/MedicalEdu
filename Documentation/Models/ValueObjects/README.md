# Value Objects Toolkit

This directory contains a comprehensive set of value objects that provide strong typing and validation for primitive domain concepts in the MedicalEdu application.

## Overview

Value objects are immutable objects that represent domain concepts and encapsulate validation logic. They ensure that every instance is valid from the moment of creation, providing a rock-solid foundation for building domain aggregates.

## Value Objects

### Core Value Objects

| Value Object | Purpose | Validation Rules |
|-------------|---------|-----------------|
| **Email** | Email addresses | RFC-style formatting, lower-case storage |
| **PasswordHash** | Password hashes | Hides hashing algorithm, prevents accidental leaks |
| **TimeZoneId** | Time zone identifiers | Validates against IANA/Windows TZ database |
| **Currency** | Currency codes | Guarantees valid ISO-4217 codes |
| **Money** | Monetary amounts | Wraps money math, prevents currency mixing |
| **PhoneNumber** | Phone numbers | Enforces E.164 formatting, country code rules |
| **Url** | URLs | Validates scheme + host for any link fields |
| **SessionToken** | Session tokens | Encapsulates length/character rules for sessions |
| **Uuid** | UUIDs | Wraps Guid for extra safety and validation |

### Additional Value Objects

| Value Object | Purpose | Validation Rules |
|-------------|---------|-----------------|
| **RecurringSchedule** | Recurring schedules | JSON or CRON expression validation |
| **PromoCode** | Promotional codes | String format + validation rules |

## Usage Examples

### Email
```csharp
var email = Email.Create("user@example.com");
// Automatically converts to lowercase and validates RFC format
```

### Money with Currency
```csharp
var money = Money.Create(99.99m, Currency.Parse("USD"));
var total = money.Add(Money.Create(10.00m, Currency.Parse("USD")));
// Prevents mixing different currencies
```

### Phone Number
```csharp
var phone = PhoneNumber.Create("+1234567890");
var countryCode = phone.GetCountryCode(); // "1"
var nationalNumber = phone.GetNationalNumber(); // "234567890"
```

### Session Token
```csharp
var token = SessionToken.Create(); // Generates cryptographically secure token
var isValid = token.IsValid; // Validates format
```

### Promo Code
```csharp
var promoCode = PromoCode.Create("SAVE20");
var generatedCode = PromoCode.Generate(8); // Random generation
```

## Key Features

### Immutability
All value objects are immutable - once created, they cannot be modified.

### Validation
Each value object validates its input at creation time, ensuring only valid instances exist.

### Type Safety
Strong typing prevents accidental misuse of primitive types.

### Domain Logic
Value objects encapsulate domain-specific validation and behavior.

### Equality
All value objects implement proper equality comparison.

### Conversion
Implicit/explicit conversion operators where appropriate.

## Best Practices

1. **Always use value objects** for domain concepts instead of primitive types
2. **Validate at creation** - never allow invalid instances to exist
3. **Keep them simple** - value objects should be focused and lightweight
4. **Use meaningful names** - the name should clearly indicate the domain concept
5. **Implement proper equality** - value objects should be compared by value, not reference

## Integration with Aggregates

Value objects provide the foundation for building robust domain aggregates. They ensure that:

- Every email is properly formatted
- Every amount is in the correct currency
- Every phone number follows international standards
- Every URL is valid and secure
- Every session token is cryptographically secure

This toolkit provides a comprehensive set of value objects that cover all the "primitive" concepts that must always be valid in your domain, giving you a rock-solid foundation for building your domain aggregates. 