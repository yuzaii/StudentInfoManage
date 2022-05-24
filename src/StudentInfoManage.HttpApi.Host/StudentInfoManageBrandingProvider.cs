using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace StudentInfoManage;

[Dependency(ReplaceServices = true)]
public class StudentInfoManageBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "StudentInfoManage";
}
