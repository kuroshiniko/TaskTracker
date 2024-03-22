using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DB
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Column("_role")]
        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(50)]
        public string Post { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Login { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }


        public virtual ICollection<TaskAssignment> TaskAssignments { get; set; }
    }

}
