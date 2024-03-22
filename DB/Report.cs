    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DB
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        [StringLength(1000)]
        public string EmployeeName { get; set; }
        public int TaskId { get; set; }

        [StringLength(8000)]
        public string ReportText { get; set; }

        [ForeignKey("TaskId")]
        public Task Task { get; set; }
    }
}   
