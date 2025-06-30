using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.DTOs.Device;
using SmartHome.Application.Exceptions;
using SmartHome.Application.Repositories.Contract;
using SmartHome.Application.Services.Contract;
using SmartHome.Application.Services.Contract.Buisness;
using SmartHome.Domain.Entities;
using SmartHome.Domain.Entities.Identity;
using System.Security.Claims;
using System.Text;

namespace SmartHome.Persistence.Services.Buisness
{
    public class DeviceUserService : IDeviceUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IServiceManager _serviceManager;
        public DeviceUserService(IRepositoryManager repositoryManager,
                                 UserManager<AppUser> userManager,
                                 IServiceManager serviceManager)
        {
            _repositoryManager = repositoryManager;
            _userManager = userManager;
            _serviceManager = serviceManager;
        }
        public async Task AssignDeviceToHost(AssignHostDto dto)
        {
            var device=await _repositoryManager.DeviceRepository.FindByIdAsync(dto.DeviceId);
            var errors = new StringBuilder();
            if (device == null)
            {
                errors.Append($"Device with Id {dto.DeviceId} not found.");
            }
            var user=await _userManager.FindByIdAsync(dto.HostId);
            if(user == null)
            {
                errors.Append($"User with Id {dto.HostId} not found.");
            }

            if (errors.Length > 0)
            {
                throw new NotFoundException(errors.ToString().Trim());
            }
            bool isInRole = await _userManager.IsInRoleAsync(user, "Host");
            
            if (!isInRole)
            {
                throw new ForbiddenException();
            }
            DeviceUser deviceUser = new DeviceUser()
            {
                DeviceId = dto.DeviceId,
                UserId = dto.HostId,
            };
            var data = await _repositoryManager.DeviceUserRepository.FindByCondition(n => n.UserId == dto.HostId && n.DeviceId == dto.DeviceId).FirstOrDefaultAsync();
            if(data != null)
            {
                throw new ConflictException("Device is already assigned to this user.");
            }
            await _repositoryManager.DeviceUserRepository.CreateAsync(deviceUser);
        }

        public async Task AssignDeviceToMember(AssignMemberDto dto)
        {
            string hostId = _serviceManager.CurrentUserService.GetUserId();

            var errors = new StringBuilder();

            var device = await _repositoryManager.DeviceRepository.FindByIdAsync(dto.DeviceId);
            if (device == null)
                errors.AppendLine($"Device with Id {dto.DeviceId} not found.");

            var member = await _userManager.FindByIdAsync(dto.MemberId);
            if (member == null)
                errors.AppendLine($"User with Id {dto.MemberId} not found.");

            var host = await _userManager.FindByIdAsync(hostId);
            if (host == null)
                errors.AppendLine($"User with Id {hostId} not found.");

            if (errors.Length > 0)
                throw new NotFoundException(errors.ToString().Trim());

            if (!await _userManager.IsInRoleAsync(member, "Member"))
                throw new ForbiddenException("Only users with 'Member' role can be assigned to a device.");

            if (!await _userManager.IsInRoleAsync(host, "Host"))
                throw new ForbiddenException("Only users with 'Host' role can assign devices to members.");

            if (member.HostId != host.Id)
                throw new ForbiddenException("You can only assign devices to members under your supervision.");

            var hostHasDevice = await _repositoryManager.DeviceUserRepository
                .FindByCondition(x => x.UserId == hostId && x.DeviceId == dto.DeviceId)
                .AnyAsync();

            if (!hostHasDevice)
                throw new ForbiddenException("You can only assign your own devices to members.");

            var alreadyAssigned = await _repositoryManager.DeviceUserRepository
                .FindByCondition(x => x.UserId == dto.MemberId && x.DeviceId == dto.DeviceId)
                .FirstOrDefaultAsync();

            if (alreadyAssigned != null)
                throw new ConflictException("Device is already assigned to this member.");

            var deviceUser = new DeviceUser
            {
                DeviceId = dto.DeviceId,
                UserId = dto.MemberId
            };

            await _repositoryManager.DeviceUserRepository.CreateAsync(deviceUser);
        }
    }
}
