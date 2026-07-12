namespace HamedSoft.Template.Application.Contracts.Authentication
{
    public sealed record RegisterRequest(string Identifier, string Password, string FirstName, string LastName);
}
