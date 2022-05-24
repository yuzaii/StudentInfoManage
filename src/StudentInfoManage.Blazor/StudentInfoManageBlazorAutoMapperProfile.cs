using AutoMapper;
using StudentInfoManage.Students;

namespace StudentInfoManage.Blazor;

public class StudentInfoManageBlazorAutoMapperProfile : Profile
{
    public StudentInfoManageBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.
        CreateMap<StudentDto, CreateUpdateStudentDto>();
    }
}
