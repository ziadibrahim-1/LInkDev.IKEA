namespace LinkDev.IKEA.BLL.Models.Departments
{
    public record CreateDepartmentDto(string Name, string? Description, string Code, DateOnly CreationDate);
}
