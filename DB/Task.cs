using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DB
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string TaskName { get; set; }

        public bool InArchive { get; set; }
        public bool NotComplite { get; set; }

        [StringLength(8000)]
        public string TaskText { get; set; }

        public int ID_lead { get; set; }

        [ForeignKey("ID_lead")]
        public Employee Lead { get; set; }

        public virtual ICollection<TaskAssignment> TaskAssignments { get; set; }
    }

}
