using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DB;
using System.Data.Entity;

namespace TaskTracker.TeamLead
{
    public class ReportView//класс для вывода отчётов
    {
        public string TaskName { get; set; }
        public string EmployeeName { get; set; }
        public string ReportText { get; set; }
        public ReportView(Report a)
        {
            DataContext db = new DataContext();
            db.Tasks.Load();
            TaskName = db.Tasks.Local.ToList().Find(x => x.Id == a.TaskId).TaskName;
            EmployeeName = a.EmployeeName;
            ReportText= a.ReportText;
        }
    }
}
