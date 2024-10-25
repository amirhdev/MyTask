using Riton_Task.Data.Context;

namespace Riton_Task.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly StaffDbContext _dbContext;
    public IStaffRepository StaffRepository { get; }
    public IProcessedFilesRepository ProcessedFilesRepository { get; }

    public UnitOfWork(
        StaffDbContext dbContext, 
        IStaffRepository staffRepository,
        IProcessedFilesRepository processedFilesRepository)
    {
        _dbContext = dbContext;
        StaffRepository = staffRepository;
        ProcessedFilesRepository = processedFilesRepository;
    }

    public async Task SaveAsync(CancellationToken cancellation = default) => await _dbContext.SaveChangesAsync(cancellation);
}
