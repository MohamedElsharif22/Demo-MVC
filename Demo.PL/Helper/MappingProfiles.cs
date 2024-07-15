using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles() 
        { 
            CreateMap<Employee,EmployeeViewModel>().ReverseMap();
            CreateMap<Department,DepartmentViewModel>().ReverseMap();
        }
    }
}
