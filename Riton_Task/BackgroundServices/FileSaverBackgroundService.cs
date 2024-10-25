using Riton_Task.Data.Models;
using Riton_Task.Repositories;
using Riton_Task.Utilities;
using System.Data;

namespace Riton_Task.BackgroundServices;

public class FileSaverBackgroundService : BackgroundService
{
    private TimeSpan _period = TimeSpan.FromMinutes(1);
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FileSaverBackgroundService> _logger;

    public FileSaverBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<FileSaverBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var periodicTimer = new PeriodicTimer(_period);
        do
        {
            await ProcessFile();
        } while (!stoppingToken.IsCancellationRequested && await periodicTimer.WaitForNextTickAsync(stoppingToken));
    }



    private async Task ProcessFile()
    {
        _logger.LogInformation("Processing file...");

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>()!;


            var file = Directory.GetFiles(configuration["StoredFilesPath"]!, "*.xlsx").FirstOrDefault();
            if (file is null)
                return;


            var fileName = Path.GetFileNameWithoutExtension(file);
            if (await unitOfWork.ProcessedFilesRepository.IsExistAsync(fileName))
            {
                File.Delete(file);
                return;
            }


            using (Stream fs = File.Open(file, FileMode.Open))
            {
                var staffs = await ReadData(fs);
                await SaveData(unitOfWork, fileName, staffs);
            }

            File.Delete(file);

            _logger.LogInformation("Saving data is successful.  FileName: " + fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message + " " + ex.InnerException?.Message ?? "");
        }
    }

    private async Task SaveData(IUnitOfWork unitOfWork, string fileName, List<Staff> staffs)
    {
        staffs.ForEach(unitOfWork.StaffRepository.Add);
        unitOfWork.ProcessedFilesRepository.Add(fileName);
        await unitOfWork.SaveAsync();
    }

    private async Task<List<Staff>> ReadData(Stream fs)
    {
        var excelData = await FileConverter.ExcelToDataSetAsync(fs);
        var staffs = excelData.Tables[0]
            .AsEnumerable()
            .Select(x => new Staff()
            {
                Id = Guid.NewGuid(),
                FirstName = x.Field<string>("First Name"),
                LastName = x.Field<string>("Last Name"),
                Gender = x.Field<string>("Gender") == "Female",
                Country = x.Field<string>("Country"),
                Age = int.Parse(x.Field<string>("Age")),
                HireDate = DateOnly.ParseExact(x.Field<string>("Date"), "dd/MM/yyyy"),
                PersonalId = int.Parse(x.Field<string>("Id")),
            }).ToList();
        return staffs;
    }
}