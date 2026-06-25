using LinkDev.IKEA.BLL.Mapping;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.BLL.Services.Employees;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace LinkDev.IKEA.BLL
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            // Add AutoMapper with high version of Auttomapper based on the Offesial web site
            services.AddAutoMapper(M => { } , typeof(EmployeeProfile));

            return services;
        }
    }
}
