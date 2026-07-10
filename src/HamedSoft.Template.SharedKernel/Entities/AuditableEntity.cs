namespace HamedSoft.Template.SharedKernel.Entities;

public abstract class AuditableEntity : Entity
{
    public DateTimeOffset CreatedOnUtc { get; private set; }

    public Guid? CreatedBy { get; private set; }

    public DateTimeOffset? LastModifiedOnUtc { get; private set; }

    public Guid? LastModifiedBy { get; private set; }

    public DateTimeOffset? DeletedOnUtc { get; private set; }

    public Guid? DeletedBy { get; private set; }

    public bool IsDeleted { get; private set; }

    public byte[] RowVersion { get; private set; } = [];


    public void SetCreated(DateTimeOffset date, Guid? userId)
    {
        CreatedOnUtc = date;
        CreatedBy = userId;
    }


    public void SetModified(DateTimeOffset date, Guid? userId)
    {
        LastModifiedOnUtc = date;
        LastModifiedBy = userId;
    }


    public void SetDeleted(DateTimeOffset date, Guid? userId)
    {
        IsDeleted = true;
        DeletedOnUtc = date;
        DeletedBy = userId;
    }
}