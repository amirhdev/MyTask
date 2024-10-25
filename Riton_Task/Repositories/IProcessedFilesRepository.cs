namespace Riton_Task.Repositories;

public interface IProcessedFilesRepository
{
    Task<bool> IsExistAsync(string fileName);
    void Add(string fileName);
}