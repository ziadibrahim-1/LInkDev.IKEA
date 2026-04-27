namespace LinkDev.IKEA.BLL.Models.Departments
{
    internal record CreateDepartmentDto(string Name, string? Description, string Code, DateOnly CreationDate);
}
