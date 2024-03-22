using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DB
{
    public class TaskAssignment
    {
        [Key]
        [Column(Order = 0)]
        public int TaskId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int EmployeeId { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskAssignments")]
        public Task Task { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("TaskAssignments")]
        public Employee Employee { get; set; }
    }

}
