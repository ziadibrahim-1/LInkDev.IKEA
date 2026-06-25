using AutoMapper;
using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.DAL.Entities.Departments;
using LinkDev.IKEA.DAL.Entities.Employees;

namespace LinkDev.IKEA.BLL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.FirstName, options => options.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.DepartmentName, options => options.MapFrom(src => src.Department!.Name))
                .ForMember(dest => dest.DepartmentName, options =>
                {
                    options.PreCondition(src => src.Department != null);
                    options.MapFrom(src => src.Department!.Name);
                })
                .ForMember(dest=>dest.HireDate , options => options.MapFrom(src => src.HireDate.ToString("MMMMM d, yyyy")));


            CreateMap<CreateEmployeeDto, Employee>()
                .ForMember(dest => dest.HireDate, options => options.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.IsActive, options => options.MapFrom(src => true))
                .ForMember(dest => dest.CreatedBy, options => options.MapFrom(src => string.Empty))
                .ForMember(dest => dest.LastModifiedBy, options => options.MapFrom(src => string.Empty));
            ;



            CreateMap<Department , DepartmentDto>()
                .ForMember(dest => dest.Manager, options =>
                {
                    options.PreCondition(src => src.Manager != null);
                    options.MapFrom(src => $"{src.Manager!.FirstName} {src.Manager!.LastName}");
                });

        }
    }
}
