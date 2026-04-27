namespace LinkDev.IKEA.BLL.Models.Departments
{
    public record UpdateDepartmentDto(int Id, string Name, string? Description, string Code, DateOnly CreationDate);
}
