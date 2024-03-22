using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DB
{
    public class ReleaseTime
    {
        [Key]
        public int ID { get; set; }
        public int TaskId { get; set; }

        public DateTime Time { get; set; }

        [ForeignKey("TaskId")]
        public Task Task { get; set; }
    }
}
