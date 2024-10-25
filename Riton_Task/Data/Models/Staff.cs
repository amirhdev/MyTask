namespace Riton_Task.Data.Models;

public class Staff
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool Gender { get; set; }
    public string? Country { get; set; }
    public int? Age { get; set; }
    public DateOnly? HireDate { get; set; }
    public int? PersonalId { get; set; }
}
