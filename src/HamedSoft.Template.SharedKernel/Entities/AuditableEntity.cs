namespace HamedSoft.Template.SharedKernel.Entities;

public abstract class AuditableEntity : Entity
{
    public DateTime CreatedOnUtc { get; protected set; }

    public Guid? CreatedBy { get; protected set; }

    public DateTime? LastModifiedOnUtc { get; protected set; }

    public Guid? LastModifiedBy { get; protected set; }

    public DateTime? DeletedOnUtc { get; protected set; }

    public Guid? DeletedBy { get; protected set; }

    public bool IsDeleted { get; protected set; }

    public byte[] RowVersion { get; protected set; } = Array.Empty<byte>();
}