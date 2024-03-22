using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.LoginForm
{
    internal class CheckFunctions
    {
        public static bool CheckAdmin(string login, string password, TaskTracker.DB.Admin a)
        {
            return login == a.Login && password == a.Password;
        }
        public static bool CheckEmployee(string login, string password, TaskTracker.DB.Employee a)
        {
            return login == a.Login && password == a.Password;
        }

    }
}
