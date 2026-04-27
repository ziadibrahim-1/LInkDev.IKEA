using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.DAL.Contracts;
using LinkDev.IKEA.DAL.Entities.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services.Departments
{
    internal class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DepartmentDetailsDto? GetDepartmentById(int id)
        {
            var department = _unitOfWork.DepartmentRepository.Get(id);
            if (department == null)
                return null;
            return new DepartmentDetailsDto(department.Id, department.Name, department.Description,
                                            department.Code, department.CreationDate,
                                            department.CreatedBy, department.CreatedOn,
                                            department.LastModifiedBy, department.LastModifiedOn);
        }

        public IEnumerable<DepartmentDto> GetDepartments()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            foreach (var department in departments)
            {
                yield return new DepartmentDto(department.Id, department.Name, department.Code, department.CreationDate);
            }

        }

        public int CreateDepartment(CreateDepartmentDto department)
        {
            var DepartmentToCreate = new Department()
            {
                Name = department.Name,
                Description = department.Description,
                Code = department.Code,
                CreationDate = department.CreationDate,
                CreatedBy = "",
                LastModifiedBy = ""
            };

            _unitOfWork.DepartmentRepository.Add(DepartmentToCreate);   
            return _unitOfWork.Complete();
        }

        public int UpdateDepartment(UpdateDepartmentDto department)
        {
            var departmentToCreate = new Department()
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Code = department.Code,
                CreationDate = department.CreationDate,
                CreatedBy = "",
                LastModifiedBy = ""
            };
            _unitOfWork.DepartmentRepository.Update(departmentToCreate);
            return _unitOfWork.Complete();
        }
        public bool DeleteDepartment(int id)
        {
            _unitOfWork.DepartmentRepository.Delete(id);
            var deleted = _unitOfWork.Complete() > 0;
            return deleted;

        }

        
    }
}
