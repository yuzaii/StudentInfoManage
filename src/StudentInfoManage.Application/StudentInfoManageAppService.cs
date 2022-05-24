using System;
using System.Collections.Generic;
using System.Text;
using StudentInfoManage.Localization;
using Volo.Abp.Application.Services;

namespace StudentInfoManage;

/* Inherit your application services from this class.
 */
public abstract class StudentInfoManageAppService : ApplicationService
{
    protected StudentInfoManageAppService()
    {
        LocalizationResource = typeof(StudentInfoManageResource);
    }
}
