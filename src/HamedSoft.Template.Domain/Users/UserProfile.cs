using HamedSoft.Template.SharedKernel.Entities;

namespace HamedSoft.Template.Domain.Users;

public class UserProfile : AggregateRoot
{
    public Guid UserId { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    private UserProfile()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public UserProfile(Guid id, Guid userId, string firstName, string lastName)
    {
        Id = id;
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }
}