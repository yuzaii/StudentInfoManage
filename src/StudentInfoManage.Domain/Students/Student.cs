using System;
using StudentInfoManage.Students;
using Volo.Abp.Domain.Entities.Auditing;

namespace StudentInfoManage.Stendents;
public class Student : AuditedAggregateRoot<Guid>
    {
        // public string Name { get; set; }
        //
        // public BookType Type { get; set; }
        //
        // public DateTime PublishDate { get; set; }
        //
        // public float Price { get; set; }
        
        public string Stu_Name { get; set; }

        public Major Stu_Major { get; set; }

        public DateTime Stu_BirthDate { get; set; }

        public String Stu_Grade { get; set; }
        
        public String Stu_Class { get; set; }
    }
