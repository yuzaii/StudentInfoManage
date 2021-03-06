using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentInfoManage.Localization;
using StudentInfoManage.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace StudentInfoManage.Blazor.Menus;

public class StudentInfoManageMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public StudentInfoManageMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private async  Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<StudentInfoManageResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                StudentInfoManageMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home"
            )
        );
        
        // context.Menu.AddItem(
        //     new ApplicationMenuItem(
        //         "StudentInfoManage",
        //         l["Menu:StudentInfoManage"],
        //         icon: "fa fa-user"
        //     ).AddItem(
        //         new ApplicationMenuItem(
        //             "StudentInfoManage.Students",
        //             l["Menu:Students"],
        //             url: "/students"
        //         )
        //     )
        // );
        
        
        var studentInfoManageMenu = new ApplicationMenuItem(
            "StudentInfoManage",
            l["Menu:StudentInfoManage"],
            icon: "fa fa-users"
        );

        context.Menu.AddItem(studentInfoManageMenu);

//CHECK the PERMISSION
        if (await context.IsGrantedAsync(StudentInfoManagePermissions.Students.Default))
        {
            studentInfoManageMenu.AddItem(new ApplicationMenuItem(
                "StudentInfoManage.Students",
                l["Menu:Students"],
                url: "/students"
            ));
        }


        // return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();

        var identityServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{identityServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
            icon: "fa fa-cog",
            order: 1000,
            null).RequireAuthenticated());

        return Task.CompletedTask;
    }
}
