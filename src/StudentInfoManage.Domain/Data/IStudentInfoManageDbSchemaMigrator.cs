using System.Threading.Tasks;

namespace StudentInfoManage.Data;

public interface IStudentInfoManageDbSchemaMigrator
{
    Task MigrateAsync();
}
