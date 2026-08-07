using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Contracts.UnitOfWork;
using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Services;

internal sealed class RoleManagementService : IRoleManagementService
{
    private readonly IRoleReadRepository _roleReadRepository;
    private readonly IRoleWriteRepository _roleWriteRepository;
    private readonly IApplicationUnitOfWork _unitOfWork;

    public RoleManagementService(
        IRoleReadRepository roleReadRepository,
        IRoleWriteRepository roleWriteRepository,
        IApplicationUnitOfWork unitOfWork)
    {
        _roleReadRepository = roleReadRepository;
        _roleWriteRepository = roleWriteRepository;
        _unitOfWork = unitOfWork;
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

    public async Task<Result<IReadOnlyList<RolePermissionsDto>>> GetAllAsync(
       CancellationToken cancellationToken = default)
    {
        var roles = await _roleReadRepository
            .GetAllAsync(cancellationToken);


        return Result<IReadOnlyList<RolePermissionsDto>>
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
}