## 开发环境：

- 系统：win10
- 编译器：rider
- 环境：.net 6.0、nodejs 12.0
  ## 

### 创建解决方案

使用 ABP CLI 的 new 命令创建一个新项目:

```shell
abp new StudentInfoManage -u blazor
```

![image.png](https://cdn.nlark.com/yuque/0/2022/png/28598806/1653409282422-58b25773-b5ac-4372-a4eb-1de1d5c84bfb.png)
下载好之后会弹出一个网页，说明你的项目创建成功
![image.png](https://cdn.nlark.com/yuque/0/2022/png/28598806/1653409323788-e2f1bd0f-747b-4f46-bec7-3ccfa6b0a9bc.png?x-oss-process=image%2Fresize%2Cw_510%2Climit_0)

#### 1.打开项目解决方案

打开项目之后可以看到项目结构如下图所示
![image.png](https://cdn.nlark.com/yuque/0/2022/png/28598806/1653409499665-cc28dc13-fcb1-48d0-8cfe-11d576ee8390.png)

#### 2.迁移数据库

终端打开StudentInfoManage.DbMigrato输入

```csharp
dotnet run
```

当出现数据库中StudentInfoManage时说明数据库迁移成功
![image.png](https://cdn.nlark.com/yuque/0/2022/png/28598806/1653410685057-fa521c2a-6f56-48f4-aef5-ff7e155f6c8a.png)

### 创建Student实体

启动模板中的**领域层**分为两个项目:

- StudentInfoManage.Domain包含你的[实体](https://docs.abp.io/zh-Hans/abp/latest/Entities), [领域服务](https://docs.abp.io/zh-Hans/abp/latest/Domain-Services)和其他核心域对象.
- StudentInfoManage.Domain.Shared包含可与客户共享的常量,枚举或其他域相关对象.

我们在的**领域层**(StudentInfoManage.Domain项目)中定义实体.
该应用程序的主要实体是student. 在StudentInfoManage.Domain项目中创建一个 Students 文件夹(命名空间),并在其中添加名为 Student 的类,如下所示:
属性有姓名、性别、出生日期、专业、年级、班级

```csharp
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace StudentInfoManage.Stendents;
public class Student : AuditedAggregateRoot<Guid>
    {
        public string Stu_Name { get; set; }
        
        public Major Stu_Major { get; set; }
        
        public DateTime Stu_BirthDate { get; set; }
        
        public String Stu_Grade { get; set; }
        
        public String Stu_Class { get; set; }
    }

```

#### 1.Major枚举

Student实体使用了Major枚举(专业). 在StudentInfoManage.Domain.Shared项目中创建Students文件夹(命名空间),并在其中添加Major:

```csharp
namespace StudentInfoManage.Students;

public enum Major
{
    Undefined,
    SoftwareEngineering,
    ComputerScience,
    MechanicalEngineering,
    Adventure,
    Biography
   
}
```

#### 2.将Student实体添加到DbContext中

EF Core需要你将实体和 DbContext 建立关联.最简单的做法是在StudentInfoManage.EntityFrameworkCore项目的StudentInfoManageDbContext类中添加DbSet属性.如下所示

```csharp
public class StudentInfoManageDbContext : AbpDbContext<StudentInfoManageDbContext>
{
 /* Add DbSet properties for your Aggregate Roots / Entities here. */
   public DbSet<Student> Students { get; set; }
    //...
}

```

#### 3.将Student实体映射到数据库表

打开StudentInfoManageDbContext类的OnModelCreating方法,为Srudent实体添加映射代码:

```csharp
protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            ...

            /* Configure your own tables/entities inside here */

             builder.Entity<Student>(b =>
        {
            b.ToTable(StudentInfoManageConsts.DbTablePrefix + "Students",
                StudentInfoManageConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Stu_Name).IsRequired().HasMaxLength(128);
        });
        }
    }
}

```

#### 4.添加数据迁移

在StudentInfoManage.EntityFrameworkCore 目录打开命令行终端输入以下命令:

```shell
dotnet ef migrations add Created_Student_Entity
dotnet ef database update
```

出现以下信息即为迁移成功
![image.png](https://cdn.nlark.com/yuque/0/2022/png/28598806/1653413722498-118c0f39-991c-4621-9861-25ec3ed65dd8.png)
我们也能在数据库中找到这张表
![image.png](https://cdn.nlark.com/yuque/0/2022/png/28598806/1653413755897-5e89c2cb-5181-4f46-814a-4c6addb99524.png)

#### 5.添加种子数据

在 *.Domain 项目下的Data 创建 IDataSeedContributor 的派生类:

```csharp
using System;
using System.Threading.Tasks;
using StudentInfoManage.Stendents;
using StudentInfoManage.Students;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
namespace StudentInfoManage.Data;

public class StudentInfoManageDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Student, Guid> _stuRepository;

    public StudentInfoManageDataSeederContributor(IRepository<Student, Guid> stuRepository)
    {
        _stuRepository = stuRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _stuRepository.GetCountAsync() <= 0)
        {
            await _stuRepository.InsertAsync(
                new Student
                {
                    Stu_Name = "周良宇",
                    Stu_Major = Major.SoftwareEngineering,
                    Stu_BirthDate = new DateTime(2000, 5, 26),
                    Stu_Grade = "19",
                    Stu_Class = "1"
                },
                autoSave: true
            );

            await _stuRepository.InsertAsync(
                new Student
                {
                    Stu_Name = "徐京",
                    Stu_Major = Major.MechanicalEngineering,
                    Stu_BirthDate = new DateTime(1998, 12, 11),
                    Stu_Grade = "19",
                    Stu_Class = "1"
                },
                autoSave: true
            );
        }
    }
}
```

- 如果数据库中当前没有学生,则此代码使用 IRepository<Student, Guid>(默认[repository](https://docs.abp.io/zh-Hans/abp/latest/Repositories))将两个学生信息插入数据库.

#### 6.更新数据库

终端打开StudentInfoManage.DbMigrato输入

```csharp
dotnet run
```

![image.png](https://cdn.nlark.com/yuque/0/2022/png/28598806/1653414687315-67bd1585-98dd-4279-8868-21413c667229.png)
打开数据库发现成功插入两条学生信息
![image.png](https://cdn.nlark.com/yuque/0/2022/png/28598806/1653414707646-d2d24a1e-c745-4320-a0be-83054cfb4e19.png)

### 创建应用程序

应用程序层由两个分离的项目组成:

- StudentInfoManage.Application.Contracts 包含你的[DTO](https://docs.abp.io/zh-Hans/abp/latest/Data-Transfer-Objects)和[应用服务](https://docs.abp.io/zh-Hans/abp/latest/Application-Services)接口.
- StudentInfoManage.Application 包含你的应用服务实现.

在本部分中,你将创建一个应用程序服务,使用ABP Framework的 CrudAppService 基类来获取,创建,更新和删除书籍.

#### 1.StudentDto

CrudAppService 基类需要定义实体的基本DTO. 在StudentInfoManage.Application.Contracts 项目中创建 Studnets 文件夹(命名空间), 并在其中添加名为 StudentDto 的DTO类:

```csharp
using System;
using Volo.Abp.Application.Dtos;
namespace StudentInfoManage.Students;

public class StudentDto: AuditedEntityDto<Guid>
{
    public string Stu_Name { get; set; }

    public Major Stu_Major { get; set; }

    public DateTime Stu_BirthDate { get; set; }

    public String Stu_Grade { get; set; }
        
    public String Stu_Class { get; set; }
}
```

在将书籍返回到表示层时,需要将Studnet实体转换为StudnetDto对象. [AutoMapper](https://automapper.org/)库可以在定义了正确的映射时自动执行此转换. 启动模板配置了AutoMapper,因此你只需在StudentInfoManage.Application项目的StudentInfoManageApplicationAutoMapperProfile类中定义映射:

```csharp
using AutoMapper;
using StudentInfoManage.Stendents;
using StudentInfoManage.Students;

namespace StudentInfoManage;

public class StudentInfoManageApplicationAutoMapperProfile : Profile
{
    public StudentInfoManageApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Student, StudentDto>();
        CreateMap<CreateUpdateStudentDto, Student>();
    }
}


```

#### 2.CreateUpdateStudentDto

在StudentInfoManage.Application.Contracts项目中创建 Students 文件夹(命名空间),并在其中添加名为 CreateUpdateStudentDto 的DTO类:

```csharp
using System;
using System.ComponentModel.DataAnnotations;
namespace StudentInfoManage.Students;

public class CreateUpdateStudentDto
{
    [Required]
    [StringLength(128)]
    public string Stu_Name { get; set; }
    
    [Required]
    public Major Stu_Major { get; set; }

    [Required]
    [DataType(DataType.Date)]
      public DateTime Stu_BirthDate { get; set; }= DateTime.Now;

    [Required]
    public String Stu_Grade { get; set; }
        
    [Required]
    public String Stu_Class { get; set; }
}
```

- 这个DTO类被用于在创建或更新书籍的时候从用户界面获取图书信息.
- 它定义了数据注释特性(如[Required])来定义属性的验证规则. DTO由ABP框架[自动验证](https://docs.abp.io/zh-Hans/abp/latest/Validation).

就像上面的StudentDto一样,创建一个从CreateUpdateStudentDto对象到Student实体的映射,最终映射配置类如下:

```csharp
using AutoMapper;
using StudentInfoManage.Stendents;
using StudentInfoManage.Students;

namespace StudentInfoManage;

public class StudentInfoManageApplicationAutoMapperProfile : Profile
{
    public StudentInfoManageApplicationAutoMapperProfile()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<CreateUpdateStudentDto, Student>();
    }
}

```

#### 3.IStudentAppService

下一步是为应用程序定义接口,在StudentInfoManage.Application.Contracts项目创建 Students 文件夹(命名空间),并在其中添加名为IStudentAppService的接口:

```csharp
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace StudentInfoManage.Students
{
    public interface IStudentAppService :

        ICrudAppService< //Defines CRUD methods
            StudentDto, //Used to show books
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateStudentDto> //Used to create/update a book
    {

    }
}
```

- 框架定义应用程序服务的接口**不是必需的**. 但是,它被建议作为最佳实践.
- ICrudAppService定义了常见的**CRUD**方法:GetAsync,GetListAsync,CreateAsync,UpdateAsync和DeleteAsync. 从这个接口扩展不是必需的,你可以从空的IApplicationService接口继承并手动定义自己的方法(将在下一部分中完成).
- ICrudAppService有一些变体, 你可以在每个方法中使用单独的DTO(例如使用不同的DTO进行创建和更新).

#### 4.StudentAppService

实现IStudentAppService接口了.在StudentInfoManage.Application项目中创建 Students 文件夹(命名空间),并在其中添加名为 StudentAppService 的类:

```csharp
using System;
using StudentInfoManage.Stendents;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace StudentInfoManage.Students
{
    public class StudentAppService :
        CrudAppService<
            Student, //The Book entity
            StudentDto, //Used to show books
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateStudentDto>, //Used to create/update a book
        IStudentAppService //implement the IBookAppService
    {
        public StudentAppService(IRepository<Student, Guid> repository)
            : base(repository)
        {

        }
    }
}
```

- StudentAppService继承了CrudAppService<...>.它实现了 ICrudAppService 定义的CRUD方法.
- StudentAppService注入IRepository <Student,Guid>,这是Student实体的默认仓储. ABP自动为每个聚合根(或实体)创建默认仓储. 
- StudentAppService使用[IObjectMapper](https://docs.abp.io/zh-Hans/abp/latest/Object-To-Object-Mapping)将Student对象转换为StudentDto对象, 将CreateUpdateStudentDto对象转换为Student对象. 启动模板使用[AutoMapper](http://automapper.org/)库作为对象映射提供程序. 我们之前定义了映射, 因此它将按预期工作.

#### 5.自动生成API Controllers

在典型的ASP.NET Core应用程序中,你创建**API Controller**以将应用程序服务公开为**HTTP API**端点. 这将允许浏览器或第三方客户端通过HTTP调用它们.
ABP可以[自动](https://docs.abp.io/zh-Hans/abp/latest/API/Auto-API-Controllers)按照约定将你的应用程序服务配置为MVC API控制器.

#### 6.Swagger UI

启动模板配置为使用[Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)运行[swagger UI](https://swagger.io/tools/swagger-ui/). 运行应用程序并在浏览器中输入https://localhost:XXXX/swagger/(用你自己的端口替换XXXX)作为URL. 使用CTRL+F5运行应用程序 (Acme.BookStore.HttpApi.Host)并使用浏览器访问https://localhost:<port>/swagger/ on your browser. 使用你自己的端口号替换 <port>.
你会看到一些内置的服务端点和Book服务,它们都是REST风格的端点:
Swagger有一个很好的UI来测试API.
你可以尝试执行[GET] /api/app/book API来获取书籍列表, 服务端会返回以下JSON结果:

```csharp
{
  "totalCount": 2,
  "items": [
    {
      "stu_Name": "徐京",
      "stu_Major": 3,
      "stu_BirthDate": "1998-12-11T00:00:00",
      "stu_Grade": "19",
      "stu_Class": "1",
      "lastModificationTime": null,
      "lastModifierId": null,
      "creationTime": "2022-05-25T01:50:53.5536237",
      "creatorId": null,
      "id": "1a62c833-1856-410d-5fd9-3a04095e4671"
    },
    {
      "stu_Name": "周良宇",
      "stu_Major": 1,
      "stu_BirthDate": "2000-05-26T00:00:00",
      "stu_Grade": "19",
      "stu_Class": "1",
      "lastModificationTime": null,
      "lastModifierId": null,
      "creationTime": "2022-05-25T01:50:53.350252",
      "creatorId": null,
      "id": "ce702aa5-334b-3a5e-99db-3a04095e4579"
    }
  ]
}
```

### 学生列表页面

#### 1.本地化

打开en.json（_英文翻译_）文件，修改内容如下图：

```json
{
  "Culture": "en",
  "Texts": {
    "Menu:Home": "Home",
    "Welcome": "Welcome",
    "LongWelcomeMessage": "Welcome to the application. This is a startup project based on the ABP framework. For more information, visit abp.io.",
    "Menu:StudentInfoManage": "Student Info Manage",
    "Menu:Students": "Student Info",
    "StudentsInfo": "Students Info",
    "NewStudent": "New Student",
    "Actions": "Actions",
    "Close": "Close",
    "Delete": "Delete",
    "Edit": "Edit",
    "Name": "Name",
    "Major": "Major",
    "Grade": "Grade",
    "Class": "Class",
    "BirthDate": "Birth Date",
    "CreationTime": "Creation time",
    "AreYouSure": "Are you sure?",
    "AreYouSureToDelete": "Are you sure you want to delete this item?",
    "Enum:Major:0": "Undefined",
    "Enum:Major:1": "SoftwareEngineering",
    "Enum:Major:2": "ComputerScience",
    "Enum:Major:3": "MechanicalEngineering",
    "Enum:Major:4": "Adventure",
    "Enum:Major:5": "Biography"
  }
}

```

- 本地化键名是任意的。您可以设置任何名称。我们更喜欢针对特定文本类型的一些约定；
  - Menu:为菜单项添加前缀。
  - 使用Enum:<enum-type>:<enum-value>命名约定来本地化枚举成员。当你这样做时，ABP 可以在某些适当的情况下自动本地化枚举。

#### 2.创建学生信息页面

右键单击项目StudentInfoManage.Blazor下的Pages文件夹，添加一个新的**组件**，命名为Students.razor：

```csharp
@page "/Students"
    <h3>Students</h3>
    
    @code {
    
}
```

#### 3.将学生信息页面添加到主菜单

在项目中打开StudentInfoManage.Blazor下的Menus文件夹中的StudentInfoManageMenuContributor类，在方法ConfigureMainMenuAsync末尾添加如下代码：

```csharp
 context.Menu.AddItem(
            new ApplicationMenuItem(
                "StudentInfoManage",
                l["Menu:StudentInfoManage"],
                icon: "fa fa-user"
            ).AddItem(
                new ApplicationMenuItem(
                    "StudentInfoManage.Students",
                    l["Menu:Students"],
                    url: "/students"
                )
            )
        );
```

#### 4.学生列表

使用[Blazorise library](https://blazorise.com/)作为UI组件.它是一个强大的库,支持主要的HTML/CSS框架,包括Bootstrap.
ABP提供了一个通用的基类,AbpCrudPageBase<...>,用来创建CRUD风格的页面.这个基类兼容用来构建IStudentAppService的ICrudAppService.所以我们从AbpCrudPageBase继承,获得标准CRUD的默认实现.
打开Students.razor 并把内容修改成下面这样:

```xml
@page "/Students"
@using Volo.Abp.Application.Dtos
@using StudentInfoManage.Students
@using StudentInfoManage.Localization
@using Microsoft.Extensions.Localization
@inject IStringLocalizer<StudentInfoManageResource> L
  @inherits AbpCrudPageBase<IStudentAppService, StudentDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateStudentDto>
  
  <Card>
    <CardHeader>
      <h2>@L["Students"]</h2>
    </CardHeader>
    <CardBody>
      <DataGrid TItem="StudentDto"
                Data="Entities"
                ReadData="OnDataGridReadAsync"
                TotalItems="TotalCount"
                ShowPager="true"
                PageSize="PageSize">
        <DataGridColumns>
          <DataGridColumn TItem="StudentDto"
                          Field="@nameof(StudentDto.Stu_Name)"
                          Caption="@L["Name"]">
            
          </DataGridColumn>
          <DataGridColumn TItem="StudentDto"
                          Field="@nameof(StudentDto.Stu_Major)"
                          Caption="@L["Major"]">
            <DisplayTemplate>
              @L[$"Enum:Major:{(int)context.Stu_Major}"]
            </DisplayTemplate>
          </DataGridColumn>
          <DataGridColumn TItem="StudentDto"
                          Field="@nameof(StudentDto.Stu_Grade)"
                          Caption="@L["Grade"]">
          </DataGridColumn>
          <DataGridColumn TItem="StudentDto"
                          Field="@nameof(StudentDto.Stu_Class)"
                          Caption="@L["Class"]">
          </DataGridColumn>
          <DataGridColumn TItem="StudentDto"
                          Field="@nameof(StudentDto.Stu_BirthDate)"
                          Caption="@L["BirthDate"]">
            <DisplayTemplate>
              @context.Stu_BirthDate.ToShortDateString()
            </DisplayTemplate>
          </DataGridColumn>
          
          <DataGridColumn TItem="StudentDto"
                          Field="@nameof(StudentDto.CreationTime)"
                          Caption="@L["CreationTime"]">
            <DisplayTemplate>
              @context.CreationTime.ToLongDateString()
            </DisplayTemplate>
          </DataGridColumn>
        </DataGridColumns>
      </DataGrid>
    </CardBody>
  </Card>

```

- AbpCrudPageBase<IStudentAppService, StudentDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateStudentDto>实现了所有的CRUD细节,我们从它继承.
- Entities, TotalCount, PageSize, OnDataGridReadAsync定义在基类中.
- 注入IStringLocalizer<StudentInfoManageStoreResource> (作为L对象),用于本地化.

### 创建,更新和删除学生信息

#### 1.添加新的学生信息

##### 添加 "New Button" 按钮

打开 Students.razor 替换 <CardHeader> 部分为以下代码:

```csharp
<CardHeader>
<Row Class="justify-content-between">
<Column ColumnSize="ColumnSize.IsAuto">
<h2>@L["StudentsInfo"]</h2>
</Column>
<Column ColumnSize="ColumnSize.IsAuto">
<Button Color="Color.Primary"
    Clicked="OpenCreateModalAsync">@L["NewStudent"]</Button>
    </Column>
    </Row>
   </CardHeader>
```

##### 学生信息创建模态窗口

打开 Students.razor, 添加以下代码到页面底部:

```xml
<Modal @ref="@CreateModal">
    <ModalBackdrop />
    <ModalContent IsCentered="true">
        <Form>
            <ModalHeader>
                <ModalTitle>@L["NewStudent"]</ModalTitle>
                <CloseButton Clicked="CloseCreateModalAsync"/>
            </ModalHeader>
            <ModalBody>
                <Validations @ref="@CreateValidationsRef" Model="@NewEntity" ValidateOnLoad="false">
                    <Validation MessageLocalizer="@LH.Localize">
                        <Field>
                            <FieldLabel>@L["Name"]</FieldLabel>
                            <TextEdit @bind-Text="@NewEntity.Stu_Name">
                                <Feedback>
                                    <ValidationError/>
                                </Feedback>
                            </TextEdit>
                        </Field>
                    </Validation>
                    
                    <Field>
                        <FieldLabel>@L["Major"]</FieldLabel>
                        <Select TValue="Major" @bind-SelectedValue="@NewEntity.Stu_Major">
                            @foreach (int majorValue in Enum.GetValues(typeof(Major)))
                            {
                                <SelectItem TValue="Major" Value="@((Major)majorValue)">
                                    @L[$"Enum:Major:{majorValue}"]
                                </SelectItem>
                            }
                        </Select>
                    </Field>
                    <Validation MessageLocalizer="@LH.Localize">
                                            <Field>
                                                <FieldLabel>@L["Grade"]</FieldLabel>
                                                <TextEdit @bind-Text="@NewEntity.Stu_Grade">
                                                    <Feedback>
                                                        <ValidationError/>
                                                    </Feedback>
                                                </TextEdit>
                                            </Field>
                                        </Validation>
                                        
                                          <Validation MessageLocalizer="@LH.Localize">
                                                                <Field>
                                                                    <FieldLabel>@L["Class"]</FieldLabel>
                                                                    <TextEdit @bind-Text="@NewEntity.Stu_Class">
                                                                        <Feedback>
                                                                            <ValidationError/>
                                                                        </Feedback>
                                                                    </TextEdit>
                                                                </Field>
                                                            </Validation>
                    <Field>
                        <FieldLabel>@L["BirthDate"]</FieldLabel>
                        <DateEdit TValue="DateTime" @bind-Date="NewEntity.Stu_BirthDate"/>
                    </Field>
                    
                </Validations>
            </ModalBody>
            <ModalFooter>
                <Button Color="Color.Secondary"
                        Clicked="CloseCreateModalAsync">@L["Cancel"]</Button>
                <Button Color="Color.Primary"
                        Type="@ButtonType.Submit"
                        PreventDefaultOnSubmit="true"
                        Clicked="CreateEntityAsync">@L["Save"]</Button>
            </ModalFooter>
        </Form>
    </ModalContent>
</Modal>
```

#### 2.更新学生信息

##### 操作下拉菜单

打开 Students.razor , 在 DataGridColumns 中添加以下 DataGridEntityActionsColumn 作为第一项:

```xml
 <DataGridEntityActionsColumn TItem="StudentDto" @ref="@EntityActionsColumn">
                <DisplayTemplate>
                    <EntityActions TItem="StudentDto" EntityActionsColumn="@EntityActionsColumn">
                        <EntityAction TItem="StudentDto"
                                      Text="@L["Edit"]"
                                      Clicked="() => OpenEditModalAsync(context)" />
                    </EntityActions>
                </DisplayTemplate>
            </DataGridEntityActionsColumn>
```

- OpenEditModalAsync 定义在基类中, 它接收实体(书籍)参数, 编辑这个实体.

DataGridEntityActionsColumn 组件用于显示 DataGrid 每一行中的"操作" 下拉菜单. 如果其中只有唯一的操作, DataGridEntityActionsColumn 显示 **唯一按钮**, 而不是下拉菜单.

##### 编辑模态窗口

我们现在可以定义一个模态窗口编辑学生信息. 加入下面的代码到 Students.razor 页面的底部:

```xml
<Modal @ref="@EditModal">
    <ModalBackdrop />
    <ModalContent IsCentered="true">
        <Form>
            <ModalHeader>
                <ModalTitle>@EditingEntity.Stu_Name</ModalTitle>
                <CloseButton Clicked="CloseEditModalAsync"/>
            </ModalHeader>
            <ModalBody>
                <Validations @ref="@EditValidationsRef" Model="@NewEntity" ValidateOnLoad="false">
                    <Validation MessageLocalizer="@LH.Localize">
                        <Field>
                            <FieldLabel>@L["Name"]</FieldLabel>
                            <TextEdit @bind-Text="@EditingEntity.Stu_Name">
                                <Feedback>
                                    <ValidationError/>
                                </Feedback>
                            </TextEdit>
                        </Field>
                    </Validation>
                    <Field>
                        <FieldLabel>@L["Major"]</FieldLabel>
                        <Select TValue="Major" @bind-SelectedValue="@EditingEntity.Stu_Major">
                            @foreach (int majorValue in Enum.GetValues(typeof(Major)))
                            {
                                <SelectItem TValue="Major" Value="@((Major) majorValue)">
                                    @L[$"Enum:Major:{majorValue}"]
                                </SelectItem>
                            }
                        </Select>
                    </Field>
                    
                    <Validation MessageLocalizer="@LH.Localize">
                        <Field>
                            <FieldLabel>@L["Grade"]</FieldLabel>
                            <TextEdit @bind-Text="@EditingEntity.Stu_Grade">
                                <Feedback>
                                    <ValidationError/>
                                </Feedback>
                            </TextEdit>
                        </Field>
                    </Validation>
                    
                      <Validation MessageLocalizer="@LH.Localize">
                                            <Field>
                                                <FieldLabel>@L["Class"]</FieldLabel>
                                                <TextEdit @bind-Text="@EditingEntity.Stu_Class">
                                                    <Feedback>
                                                        <ValidationError/>
                                                    </Feedback>
                                                </TextEdit>
                                            </Field>
                                        </Validation>
                    
                    <Field>
                        <FieldLabel>@L["BirthDate"]</FieldLabel>
                        <DateEdit TValue="DateTime" @bind-Date="EditingEntity.Stu_BirthDate"/>
                    </Field>
    
                </Validations>
            </ModalBody>
            <ModalFooter>
                <Button Color="Color.Secondary"
                        Clicked="CloseEditModalAsync">@L["Cancel"]</Button>
                <Button Color="Color.Primary"
                        Type="@ButtonType.Submit"
                        PreventDefaultOnSubmit="true"
                        Clicked="UpdateEntityAsync">@L["Save"]</Button>
            </ModalFooter>
        </Form>
    </ModalContent>
</Modal>

```

##### AutoMapper 配置

基类 AbpCrudPageBase 使用 [对象到对象映射](https://docs.abp.io/zh-Hans/abp/latest/Object-To-Object-Mapping) 系统将 StudentDto 对象转化为CreateUpdateStudentDto 对象. 因此, 我们需要定义映射.
打开 StudentInfoManage.Blazor 项目中的StudentInfoManageBlazorAutoMapperProfile , 替换成以下内容:

```csharp
using AutoMapper;
using StudentInfoManage.Students;

namespace StudentInfoManage.Blazor;

public class StudentInfoManageBlazorAutoMapperProfile : Profile
{
public StudentInfoManageBlazorAutoMapperProfile()
{
//Define your AutoMapper configuration here for the Blazor project.
CreateMap<StudentDto, CreateUpdateStudentDto>();
}
}

```

#### 删除书籍

打开 Students.razor 页面, 在 EntityActions 中的"编辑" 操作下面加入以下的 EntityAction:

```xml
<EntityAction TItem="StudentDto"
              Text="@L["Delete"]"
              Clicked="() => DeleteEntityAsync(context)"
                                      ConfirmationMessage="() => GetDeleteConfirmationMessage(context)" />
```

- DeleteEntityAsync 定义在基类中. 通过向服务器发起请求删除实体.
- ConfirmationMessage 执行操作前显示确认消息的回调函数.
- GetDeleteConfirmationMessage 定义在基类中. 你可以覆写这个方法 (或传递其它值给 ConfirmationMessage 参数) 以定制本地化消息.


**至此学生信息的CURD结束**



