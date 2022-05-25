using System;
using StudentInfoManage.Permissions;
using StudentInfoManage.Stendents;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace StudentInfoManage.Students
{
    public class StudentAppService :
        CrudAppService<
            Student, //The Book entity
            StudentDto, //Used to show books
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateStudentDto>, //Used to create/update a book
        IStudentAppService //implement the IBookAppService
    {
        public StudentAppService(IRepository<Student, Guid> repository)
            : base(repository)
        {
            GetPolicyName = StudentInfoManagePermissions.Students.Default;
            GetListPolicyName = StudentInfoManagePermissions.Students.Default;
            CreatePolicyName = StudentInfoManagePermissions.Students.Create;
            UpdatePolicyName = StudentInfoManagePermissions.Students.Edit;
            DeletePolicyName = StudentInfoManagePermissions.Students.Delete;
        }
    }
}