using StudentInfoManage.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace StudentInfoManage.Permissions;

public class StudentInfoManagePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        // var myGroup = context.AddGroup(StudentInfoManagePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(StudentInfoManagePermissions.MyPermission1, L("Permission:MyPermission1"));
        var studentInfoManageGroup = context.AddGroup(StudentInfoManagePermissions.GroupName, L("Permission:StudentInfoManage"));

        var studentsPermission = studentInfoManageGroup.AddPermission(StudentInfoManagePermissions.Students.Default, L("Permission:Students"));
        studentsPermission.AddChild(StudentInfoManagePermissions.Students.Create, L("Permission:Students.Create"));
        studentsPermission.AddChild(StudentInfoManagePermissions.Students.Edit, L("Permission:Students.Edit"));
        studentsPermission.AddChild(StudentInfoManagePermissions.Students.Delete, L("Permission:Students.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<StudentInfoManageResource>(name);
    }
}
