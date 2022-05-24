using System;
using Volo.Abp.Application.Dtos;
namespace StudentInfoManage.Students;

public class StudentDto: AuditedEntityDto<Guid>
{
    public string Stu_Name { get; set; }

    public Major Stu_Major { get; set; }

    public DateTime Stu_BirthDate { get; set; }

    public String Stu_Grade { get; set; }
        
    public String Stu_Class { get; set; }
}