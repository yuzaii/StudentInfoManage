using AutoMapper;
using StudentInfoManage.Stendents;
using StudentInfoManage.Students;

namespace StudentInfoManage;

public class StudentInfoManageApplicationAutoMapperProfile : Profile
{
    public StudentInfoManageApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Student, StudentDto>();
        CreateMap<CreateUpdateStudentDto, Student>();
    }
}
