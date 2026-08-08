using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Contracts.UnitOfWork;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace HamedSoft.Template.Infrastructure.Identity.Services;

internal sealed class RoleManagementService : IRoleManagementService
{
    private readonly IRoleReadRepository _roleReadRepository;
    private readonly IRoleWriteRepository _roleWriteRepository;
    private readonly IApplicationUnitOfWork _unitOfWork;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleManagementService(
        IRoleReadRepository roleReadRepository,
        IRoleWriteRepository roleWriteRepository,
        IApplicationUnitOfWork unitOfWork,
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager)
    {
        _roleReadRepository = roleReadRepository;
        _roleWriteRepository = roleWriteRepository;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
    public async Task<Result> AssignPermissionsAsync(
    Guid roleId,
    IReadOnlyCollection<Guid> permissionIds,
    CancellationToken cancellationToken = default)
    {
        if (!await _roleReadRepository.ExistsAsync(roleId, cancellationToken))
            return Result.Failure("نقش یافت نشد.");

        await _roleWriteRepository.ReplacePermissionsAsync(
            roleId,
            permissionIds,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<RoleDto>>> GetAllAsync(
       CancellationToken cancellationToken = default)
    {
        var roles = await _roleReadRepository
            .GetAllAsync(cancellationToken);


        return Result<IReadOnlyList<RoleDto>>
            .Success(roles);
    }


    public async Task<Result<RolePermissionsDto>> GetByIdAsync(
     Guid roleId,
     CancellationToken cancellationToken = default)
    {
        var role = await _roleReadRepository
            .GetByIdAsync(
                roleId,
                cancellationToken);


        if (role is null)
            return Result<RolePermissionsDto>.Failure(
                "نقش یافت نشد.");


        return Result<RolePermissionsDto>.Success(role);
    }
    public async Task<Result<Guid>> CreateAsync(
    string roleName,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return Result<Guid>.Failure("نام نقش الزامی است.");

        roleName = roleName.Trim();

        var existingRole = await _roleManager.FindByNameAsync(roleName);

        if (existingRole is not null)
            return Result<Guid>.Failure("نقشی با این نام قبلاً وجود دارد.");

        var role = new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = roleName,
            NormalizedName = roleName.ToUpperInvariant()
        };

        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var error = string.Join(
                " ",
                result.Errors.Select(x => x.Description));

            return Result<Guid>.Failure(error);
        }

        return Result<Guid>.Success(role.Id);
    }
    public async Task<Result> UpdateAsync(
    Guid roleId,
    string roleName,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return Result.Failure("نام نقش الزامی است.");

        roleName = roleName.Trim();

        var role = await _roleManager.FindByIdAsync(
            roleId.ToString());

        if (role is null)
            return Result.Failure("نقش یافت نشد.");

        var existingRole = await _roleManager.FindByNameAsync(roleName);

        if (existingRole is not null &&
            existingRole.Id != role.Id)
        {
            return Result.Failure(
                "نقش دیگری با این نام وجود دارد.");
        }

        role.Name = roleName;

        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded)
        {
            var error = string.Join(
                " ",
                result.Errors.Select(x => x.Description));

            return Result.Failure(error);
        }

        return Result.Success();
    }
    public async Task<Result> DeleteAsync(
    Guid roleId,
    CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByIdAsync(
            roleId.ToString());

        if (role is null)
            return Result.Failure("نقش یافت نشد.");

        if (string.Equals(
            role.Name,
            SystemRoles.Admin,
            StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure(
                "نقش Admin قابل حذف نیست.");
        }

        var users = await _userManager.GetUsersInRoleAsync(
            role.Name!);

        if (users.Count > 0)
        {
            return Result.Failure(
                "این نقش به یک یا چند کاربر اختصاص داده شده و قابل حذف نیست.");
        }

        var result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
        {
            var error = string.Join(
                " ",
                result.Errors.Select(x => x.Description));

            return Result.Failure(error);
        }

        return Result.Success();
    }

}