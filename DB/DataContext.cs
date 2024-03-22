using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.MVVM;

namespace TaskTracker.DB
{
   internal class DataContext : DbContext
   {
        public DataContext()
            : base("DBConnection")
        { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Report> Report { get; set; }

        public DbSet<ReleaseTime> ReleaseTime { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasMany(t => t.TaskAssignments)
                .WithRequired(ta => ta.Task)
                .HasForeignKey(ta => ta.TaskId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.TaskAssignments)
                .WithRequired(ta => ta.Employee)
                .HasForeignKey(ta => ta.EmployeeId)
                .WillCascadeOnDelete(false);
        }
        public virtual IEnumerable<GetTaskCounts_Result> GetTaskCountsForEmployee(int employeeId)
        {
            var employeeIdParameter = new SqlParameter("@EmployeeId", employeeId);
            return Database.SqlQuery<GetTaskCounts_Result>("EXEC GetTaskCountsForEmployee @EmployeeId", employeeIdParameter);
        }

    }

}

