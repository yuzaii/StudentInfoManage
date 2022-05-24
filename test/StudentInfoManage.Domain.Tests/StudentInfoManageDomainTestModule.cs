using StudentInfoManage.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace StudentInfoManage;

[DependsOn(
    typeof(StudentInfoManageEntityFrameworkCoreTestModule)
    )]
public class StudentInfoManageDomainTestModule : AbpModule
{

}
