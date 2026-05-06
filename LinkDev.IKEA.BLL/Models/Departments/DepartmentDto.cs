namespace LinkDev.IKEA.BLL.Models.Departments
{
    public record DepartmentDto(int Id, string Name,string? Description, string Code, DateOnly CreationDate,string? Manager);
}
