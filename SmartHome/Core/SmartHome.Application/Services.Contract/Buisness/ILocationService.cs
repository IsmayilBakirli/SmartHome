using SmartHome.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Services.Contract.Buisness
{
    public interface ILocationService
    {
        Task<List<LocationGetDto>> GetAllAsync();
        Task CreateAsync(LocationCreateDto entity);
        Task DeleteAsync(int id);
    }
}
