using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Domain.Users;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using HamedSoft.Template.SharedKernel.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Services;

public sealed class IdentityService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _dbContext;


    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
    }


    public async Task<Result<LoginResult>> LoginAsync(
        string identifier,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(
                x => x.UserName == identifier ||
                     x.Email == identifier ||
                     x.PhoneNumber == identifier,
                cancellationToken);


        if (user == null)
        {
            return Result<LoginResult>.Failure(
                "Invalid username or password");
        }


        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            password,
            false);


        if (!result.Succeeded)
        {
            return Result<LoginResult>.Failure(
                "Invalid username or password");
        }


        var roles = await _userManager.GetRolesAsync(user);


        return Result<LoginResult>.Success(
            new LoginResult(
                user.Id,
                user.UserName ?? string.Empty,
                roles.ToList()));
    }



    public async Task<Result<RegisterResult>> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _dbContext.Database.BeginTransactionAsync(
                cancellationToken);


        try
        {
            var exists = await _userManager.Users
                .AnyAsync(
                    x => x.UserName == request.Identifier,
                    cancellationToken);


            if (exists)
            {
                return Result<RegisterResult>.Failure(
                    "User already exists");
            }


            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = request.Identifier
            };


            var createResult =
                await _userManager.CreateAsync(
                    user,
                    request.Password);


            if (!createResult.Succeeded)
            {
                return Result<RegisterResult>.Failure(
                    string.Join(
                        ", ",
                        createResult.Errors.Select(x => x.Description)));
            }


            var profile = new UserProfile(
                user.Id,
                request.FirstName,
                request.LastName);


            await _dbContext.UserProfiles.AddAsync(
                profile,
                cancellationToken);


            await _dbContext.SaveChangesAsync(
                cancellationToken);


            await transaction.CommitAsync(
                cancellationToken);


            return Result<RegisterResult>.Success(
                new RegisterResult(user.Id));
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }
    }
}