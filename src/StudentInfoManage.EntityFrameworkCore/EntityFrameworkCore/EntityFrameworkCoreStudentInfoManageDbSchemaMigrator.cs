using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentInfoManage.Data;
using Volo.Abp.DependencyInjection;

namespace StudentInfoManage.EntityFrameworkCore;

public class EntityFrameworkCoreStudentInfoManageDbSchemaMigrator
    : IStudentInfoManageDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreStudentInfoManageDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the StudentInfoManageDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<StudentInfoManageDbContext>()
            .Database
            .MigrateAsync();
    }
}
