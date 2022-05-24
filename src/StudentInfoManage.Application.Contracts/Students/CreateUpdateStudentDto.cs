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
    public DateTime Stu_BirthDate { get; set; } = DateTime.Now;

    [Required]
    public String Stu_Grade { get; set; }
        
    [Required]
    public String Stu_Class { get; set; }
}