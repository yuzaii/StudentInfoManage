using StudentInfoManage.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace StudentInfoManage.Permissions;

public class StudentInfoManagePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(StudentInfoManagePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(StudentInfoManagePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<StudentInfoManageResource>(name);
    }
}
