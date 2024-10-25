using Riton_Task.Data.Models;

namespace Riton_Task.Repositories;

public interface IStaffRepository
{
    Task<List<Staff>> GetAllAsync();
    void Add(Staff staff);
}