using Microsoft.AspNetCore.Mvc;
using Riton_Task.Repositories;

namespace Riton_Task.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileSaver : ControllerBase
{
    private readonly IStaffRepository _staffRepository;
    private readonly IConfiguration _configuration;

    public FileSaver(IStaffRepository staffRepository, IConfiguration configuration)
    {
        _configuration = configuration;
        _staffRepository = staffRepository;
    }

    [HttpPost]
    [Route("import-excel")]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
        var path = Path.Combine(_configuration["StoredFilesPath"]!, $"{Path.GetRandomFileName()}.xlsx");

        using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(file);
    }
}