using Microsoft.EntityFrameworkCore;
using Riton_Task.Data.Context;
using Riton_Task.Data.Models;

namespace Riton_Task.Repositories;

public class ProcessedFilesRepository : IProcessedFilesRepository
{
    private readonly StaffDbContext _dbContext;

    public ProcessedFilesRepository(StaffDbContext dbContext) => _dbContext = dbContext;

    public void Add(string fileName) 
        => _dbContext.Add(new ProcessedFiles() { Id = Guid.NewGuid(), FileName = fileName });

    public async Task<bool> IsExistAsync(string fileName)
        => await _dbContext.ProcessedFileNames.AnyAsync(x => x.FileName == fileName);
}