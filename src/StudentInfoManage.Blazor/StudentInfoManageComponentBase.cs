using StudentInfoManage.Localization;
using Volo.Abp.AspNetCore.Components;

namespace StudentInfoManage.Blazor;

public abstract class StudentInfoManageComponentBase : AbpComponentBase
{
    protected StudentInfoManageComponentBase()
    {
        LocalizationResource = typeof(StudentInfoManageResource);
    }
}
