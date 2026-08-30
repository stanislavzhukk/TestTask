using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IHallRepository
    {
        Task<IReadOnlyList<Hall>> GetAllHallsAsync();
        Task<Hall?> GetHallByIdAsync(Guid hallId);
        Task<List<Hall>> SearchHallsAsync(DateTime? startTime, DateTime? endTime, int? minCapacity);
        Task<Hall> CreateHallAsync(Hall hall);
        Task<Hall> UpdateHallAsync(Hall hall);
    }
}
