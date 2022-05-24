using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace StudentInfoManage.Data;

/* This is used if database provider does't define
 * IStudentInfoManageDbSchemaMigrator implementation.
 */
public class NullStudentInfoManageDbSchemaMigrator : IStudentInfoManageDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
