using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TaskTracker.MVVM;

namespace TaskTracker.Employee
{
    /// <summary>
    /// Логика взаимодействия для EmployeeList.xaml
    /// </summary>
    public partial class EmployeeList : Window
    {
        private bool IsExitable = false;
        ViewModelEmployee _viewModel;
        private int id_employee;//id работника, который вошёл в аккаунт
        public EmployeeList(int id_emp)
        {
            InitializeComponent();
            id_employee = id_emp;
            _viewModel = new ViewModelEmployee(id_employee);//создаём модель для отображения задач
            DataContext = _viewModel; //привязываем данные
            
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsExitable = true;
            ReportWindow a = new ReportWindow(this,id_employee,_viewModel.SelectedTask.Id);//создаём окно для написания отчёта
            a.Show();//показываем окно
            this.Hide();//прячем текущее окно
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsExitable)
            {
                e.Cancel = true;
                MessageBox.Show("Отправьте отчёт! :)");
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
