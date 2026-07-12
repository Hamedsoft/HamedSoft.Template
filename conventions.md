# Coding Conventions

## Naming

- PascalCase for public members.
- camelCase for local variables.
- Private fields start with "_".

## Entity

- Use AggregateRoot for aggregate roots.
- Use Entity for child entities.
- Use ValueObject for immutable objects.

## Date & Time

Always use DateTimeOffset in UTC.

Never use DateTime.Now.

Use IDateTimeProvider instead.

## Soft Delete

Entities are never physically deleted.

Use IsDeleted and DeletedOnUtc.

## Auditing

Every auditable entity contains:

- CreatedOnUtc
- CreatedBy
- LastModifiedOnUtc
- LastModifiedBy
- DeletedOnUtc
- DeletedBy
- RowVersion

## Result Pattern

Business operations return Result instead of throwing exceptions whenever possible.