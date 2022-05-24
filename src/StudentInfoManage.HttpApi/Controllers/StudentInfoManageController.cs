using StudentInfoManage.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace StudentInfoManage.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class StudentInfoManageController : AbpControllerBase
{
    protected StudentInfoManageController()
    {
        LocalizationResource = typeof(StudentInfoManageResource);
    }
}
