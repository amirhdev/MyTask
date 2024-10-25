using Microsoft.EntityFrameworkCore;
using Riton_Task.Data.Context;
using Riton_Task.Data.Models;

namespace Riton_Task.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly StaffDbContext _dbContext;

    public StaffRepository(StaffDbContext dbContext) => _dbContext = dbContext;

    public void Add(Staff staff) => _dbContext.Staffs.Add(staff);

    public async Task<List<Staff>> GetAllAsync() => await _dbContext.Staffs.ToListAsync();
}