using System;
using System.Threading.Tasks;
using StudentInfoManage.Stendents;
using StudentInfoManage.Students;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
namespace StudentInfoManage.Data;

public class StudentInfoManageDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Student, Guid> _stuRepository;

    public StudentInfoManageDataSeederContributor(IRepository<Student, Guid> stuRepository)
    {
        _stuRepository = stuRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _stuRepository.GetCountAsync() <= 0)
        {
            await _stuRepository.InsertAsync(
                new Student
                {
                    Stu_Name = "周良宇",
                    Stu_Major = Major.SoftwareEngineering,
                    Stu_BirthDate = new DateTime(2000, 5, 26),
                    Stu_Grade = "19",
                    Stu_Class = "1"
                },
                autoSave: true
            );

            await _stuRepository.InsertAsync(
                new Student
                {
                    Stu_Name = "徐京",
                    Stu_Major = Major.MechanicalEngineering,
                    Stu_BirthDate = new DateTime(1998, 12, 11),
                    Stu_Grade = "19",
                    Stu_Class = "1"
                },
                autoSave: true
            );
        }
    }
}