namespace Riton_Task.Repositories
{
    public interface IUnitOfWork
    {
        IStaffRepository StaffRepository { get; }
        IProcessedFilesRepository ProcessedFilesRepository { get; }
        Task SaveAsync(CancellationToken cancellation = default);
    }
}
