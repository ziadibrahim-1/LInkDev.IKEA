namespace LinkDev.IKEA.BLL.Models.Departments
{
    
    public record DepartmentDetailsDto(int Id, string Name, string? Description, string Code, DateOnly CreationDate, string CreatedBy, DateTime CreatedOn, string LastModifiedBy, DateTime LastModifiedOn);
}
