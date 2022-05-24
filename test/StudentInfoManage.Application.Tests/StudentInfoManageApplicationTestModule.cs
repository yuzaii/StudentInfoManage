using Volo.Abp.Modularity;

namespace StudentInfoManage;

[DependsOn(
    typeof(StudentInfoManageApplicationModule),
    typeof(StudentInfoManageDomainTestModule)
    )]
public class StudentInfoManageApplicationTestModule : AbpModule
{

}
