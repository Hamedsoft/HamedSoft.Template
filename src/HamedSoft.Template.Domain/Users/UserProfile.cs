using HamedSoft.Template.SharedKernel.Entities;

namespace HamedSoft.Template.Domain.Users;

public class UserProfile : AggregateRoot
{
    private UserProfile()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public UserProfile(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }
}