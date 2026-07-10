namespace HamedSoft.Template.SharedKernel.Entities;

public abstract class AuditableEntity : Entity
{
    public DateTimeOffset CreatedOnUtc { get; protected set; }

    public Guid? CreatedBy { get; protected set; }

    public DateTimeOffset? LastModifiedOnUtc { get; protected set; }

    public Guid? LastModifiedBy { get; protected set; }

    public DateTimeOffset? DeletedOnUtc { get; protected set; }

    public Guid? DeletedBy { get; protected set; }

    public bool IsDeleted { get; protected set; }

    public byte[] RowVersion { get; protected set; } = [];
}