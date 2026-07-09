using HamedSoft.Template.SharedKernel.Entities;

namespace HamedSoft.Template.Domain.Users;

public class User : AggregateRoot
{
    private User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }


    public User(
        string firstName,
        string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }


    public string FirstName { get; private set; }

    public string LastName { get; private set; }
}