using HamedSoft.Template.Application.Contracts.Security;
using HamedSoft.Template.Domain.UserProfiles;
using HamedSoft.Template.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

public sealed class UserMenuViewComponent : ViewComponent
{
    private readonly ICurrentUser _currentUser;

    public UserMenuViewComponent(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public Task<IViewComponentResult> InvokeAsync()
    {
        var model = new UserProfileViewModel
        {
            UserId = _currentUser.UserId,
            UserName = _currentUser.UserName ?? string.Empty,
            DisplayName = _currentUser.DisplayName
                ?? _currentUser.UserName
                ?? string.Empty,
            Roles = _currentUser.Roles
        };

        return Task.FromResult<IViewComponentResult>(
            View(model));
    }
}