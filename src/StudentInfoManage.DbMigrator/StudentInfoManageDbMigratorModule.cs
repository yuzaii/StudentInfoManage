using StudentInfoManage.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace StudentInfoManage.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(StudentInfoManageEntityFrameworkCoreModule),
    typeof(StudentInfoManageApplicationContractsModule)
    )]
public class StudentInfoManageDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
