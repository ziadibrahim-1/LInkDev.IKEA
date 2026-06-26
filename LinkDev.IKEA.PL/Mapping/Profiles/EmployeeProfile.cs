using AutoMapper;
using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.PL.Models.Employee;

namespace LinkDev.IKEA.PL.Mapping.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeDto, EmployeeViewModel>()
                .ForMember(dest=> dest.Department,
                    ops=> ops.MapFrom(src => src.DepartmentName));


            CreateMap<EmployeeDetailsDto, EmployeeDetailsViewModel>()
            // From EmployeeDto
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Employee.Id))
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.Employee.FirstName))
            .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src => src.Employee.LastName))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Employee.Email))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.Employee.PhoneNumber))
            .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.Employee.Age))
            .ForMember(dest => dest.Gender,
                opt => opt.MapFrom(src => src.Employee.Gender))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Employee.Address))
            .ForMember(dest => dest.FormattedHireDate,
                opt => opt.MapFrom(src => src.Employee.FormattedHIreDate))
            .ForMember(dest => dest.EmployeeType,
                opt => opt.MapFrom(src => src.Employee.EmployeeType))
            .ForMember(dest => dest.Salary,
                opt => opt.MapFrom(src => src.Employee.Salary))
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(src => src.Employee.IsActive))
            .ForMember(dest => dest.DepartmentId,
                opt => opt.MapFrom(src => src.Employee.DepartmentId))
            .ForMember(dest => dest.CreatedBy,
                opt => opt.MapFrom(src => src.Employee.CreatedBy))
            .ForMember(dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.Employee.CreatedOn))
            .ForMember(dest => dest.LastModifiedBy,
                opt => opt.MapFrom(src => src.Employee.LastModifiedBy))
            .ForMember(dest => dest.LastModifiedDate,
                opt => opt.MapFrom(src => src.Employee.LastModifiedOn))

            // YearsOfExperience → YearsOfService
            .ForMember(dest => dest.YearsOfService,
                opt => opt.MapFrom(src => src.YearsOfExperience))

            // From DepartmentDto
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.DepartmentCode,
                opt => opt.MapFrom(src => src.Department.Code))
            .ForMember(dest => dest.DepartmentDescription,
                opt => opt.MapFrom(src => src.Department.Description))
            .ForMember(dest => dest.ManagerDepartmentName,
                opt => opt.MapFrom(src => src.Department.Manager));

            CreateMap<EmployeeCreateViewModel, CreateEmployeeDto>();
            CreateMap<EmployeeDto, EmployeeEditViewModel>();
            CreateMap<EmployeeEditViewModel, UpdateEmployeeDto>();
        }
    }
}
