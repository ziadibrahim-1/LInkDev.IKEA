using LinkDev.IKEA.BLL.Models.Departments;

namespace LinkDev.IKEA.BLL.Services.Departments
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDto> GetDepartments();

        DepartmentDetailsDto? GetDepartmentById(int id);

        int CreateDepartment(CreateDepartmentDto department);
        int UpdateDepartment(UpdateDepartmentDto department);
        bool DeleteDepartment(int id);
    }
}
