using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DB
{
    public class GetTaskCounts_Result
    {
        public int NotCompletedCount { get; set; }
        public int CompletedCount { get; set; }
    }
}
