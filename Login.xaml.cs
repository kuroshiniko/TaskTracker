using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskTracker.DB;
using TaskTracker.Admin;
using TaskTracker.LoginForm;
using TaskTracker.TeamLead;
using TaskTracker.Employee;

namespace TaskTracker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        DataContext db;
        List<TaskTracker.DB.Admin> AdminsData;
        List<TaskTracker.DB.Employee> EmpData;
        private string _password;
        public Login()
        {
            InitializeComponent();
            db = new DataContext();//подключаем бд
        }
        private bool CheckData()//проверяем поля на заполнение
        {
            if (!string.IsNullOrEmpty(_password) && !string.IsNullOrEmpty(this.login.Text))return true;

            return false;
        }
        private void CheckAdmin()
        {
            db.Admins.Load();//загружаем данные по администраторам
            AdminsData = db.Admins.Local.ToList();
            foreach (var admin in AdminsData)//проходим по каждой записи для администратора
            {
                if (CheckFunctions.CheckAdmin(this.login.Text, _password, admin))//если администратор найден
                {
                    AdminList _admin = new AdminList();//делаем экземпляр окна для администратора
                    _admin.Show();//показываем
                    this.Hide();//прячем окно для логина
                }
            }
        }
        private void CheckEmployee()
        {
            db.Employees.Load();//загружаем данные по работникам
            EmpData = db.Employees.Local.ToList();
            foreach (var emp in EmpData)//проходим по каждой записи для работника
            {
                if (CheckFunctions.CheckEmployee(this.login.Text, _password, emp))//если работник найден
                {
                    if (emp.Role == "Куратор группы")
                    {
                        TeamLeadList _lead = new TeamLeadList();//делаем экземпляр окна для куратора заданий
                        _lead.Show();//показываем
                        this.Hide();//прячем окно для логина
                    }
                    else
                    {
                        EmployeeList _emp = new EmployeeList(emp.Id);//делаем экземпляр окна для работника
                        _emp.Show();//показываем
                        this.Hide();//прячем окно для логина
                    }
                }

            }
           
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData())//если прошло
            {
                CheckEmployee();
                CheckAdmin();
            }
            else
            {
                MessageBox.Show("Введите значения в поля!");
            }
        }
        private void SecurePasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)//проверка на именно этот passwordbox
            {
                 _password = passwordBox.Password;//получаем значение, которое ввёл пользователь
            }
        }
    }
}
