using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.DAL.Models
{
    public class Department : BaseEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
