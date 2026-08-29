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
        Task<Hall> CreateHallAsync(Hall hall);
        Task<Hall> UpdateHallAsync(Hall hall);
    }
}
