using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;
using TaskTracker.DB;
using TaskTracker.MVVM;

namespace TaskTracker.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminList.xaml
    /// </summary>
    public partial class AdminList : Window
    {
        DataContext _db;
        ViewModel _viewModel;
        ArchiveViewModel _archiveViewModel;
        ArchiveNViewModel _archiveNViewModel;
        public AdminList()
        {
            InitializeComponent();
            _db = new DataContext();
            _viewModel = new ViewModel();
            _archiveViewModel = new ArchiveViewModel();
            _archiveNViewModel= new ArchiveNViewModel();
            ListMVVM.DataContext = _viewModel;
            ListArchiveMVVM.DataContext = _archiveViewModel; 
            ListArchiveNMVVM.DataContext = _archiveNViewModel;
            _db.Employees.Load();
            foreach(var item in _db.Employees.Local.ToList())
            {
                if (item.Role.ToString() == "Куратор группы") ListLead.Items.Add(item.Name);
            }
        }
        private void Update()//метод для обновления данных о кураторах
        {
            ListLead.Items.Clear();
            _db.Employees.Load();
            foreach (var item in _db.Employees.Local.ToList())
            {
                if (item.Role.ToString() == "Куратор группы") ListLead.Items.Add(item.Name);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckData())//проверяем заполенение
                {
                    _db.Tasks.Load();
                    DB.Task _new_task = new DB.Task();//создаём новую задачу
                    if (_db.Tasks.ToList().Find(x => x.TaskName == this.Title.Text) != null) throw new ArgumentException("Такое название уже есть!");
                    _new_task.TaskName = this.Title.Text;
                    _new_task.TaskText = this.Text.Text;
                    _new_task.ID_lead = _db.Employees.Local.ToList().Find(x => x.Name == ListLead.SelectedValue.ToString()).Id;//присваиваем задаче лидера группы
                    _db.Tasks.Add(_new_task);
                    _db.SaveChanges();//сохраняем базу данных

                    var taskListNew = _db.Tasks.Local.ToList();//получаем и базы данных список заданий
                    DB.ReleaseTime _new_time = new DB.ReleaseTime();//создаём новый объект для даты окончания задачи
                    _new_time.Time = Convert.ToDateTime(this.Time.Text);//присваиваем данные
                    _new_time.TaskId = taskListNew.Last().Id;
                    _db.ReleaseTime.Add(_new_time);
                    _db.SaveChanges();//сохраняем базу данных
                    MessageBox.Show("Новое задание добавлено");
                }
                else//если не заполнены поля
                {
                    MessageBox.Show("Введите значения в поля!");
                }
            }
            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (CheckEmpData() && CheckSameData())
            {
                DB.Employee new_empl = new DB.Employee();
                new_empl.Login = EmpLogin.Text;
                new_empl.Post = EmpPost.Text;
                new_empl.Name = EmpName.Text;
                new_empl.Password = EmpPass.Text;
                new_empl.Role = this.EmpRole.Text;
                _db.Employees.Add(new_empl);
                _db.SaveChanges();
                MessageBox.Show("Пользователь "+ new_empl.Name+" добавлен.");
            }
            else MessageBox.Show("Введите данные!");
        }
        //методы для проверки заполнения данных
        private bool CheckData()//проверка на заполнение задания
        {
            if ((this.Title.Text != null && this.Text.Text != null)
                && (this.Title.Text != "" && this.Text.Text != "")
                && (this.Time.Text != "" && this.Time.Text != "")) return true;

            return false;
        }
        private bool CheckEmpData()
        {
            if ((this.EmpPost.Text != null && this.EmpPost.Text != null)
                && (this.EmpPass.Text != "" && (this.EmpPass.Text != "")
                && (this.EmpName.Text != "" && this.EmpName.Text != ""))
                && (this.EmpLogin.Text != "" && this.EmpLogin.Text != "")) return true;

            return false;
        }
        private bool CheckSameData()
        {
            
            foreach(var employee in _db.Employees.Local.ToList())
            {
                if (employee.Login == this.EmpLogin.Text && employee.Password == this.EmpPass.Text)
                {
                    MessageBox.Show("Пользователь с таким аккаунтом уже есть.");
                    return false;
                }
            }

            return true;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.UpdateData();
            _archiveViewModel.UpdateData();
            _archiveNViewModel.UpdateData();
            ListMVVM.DataContext = _viewModel;
            ListArchiveMVVM.DataContext = _archiveViewModel;
            ListArchiveNMVVM.DataContext = _archiveNViewModel;
        }

        private void TabItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Update();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _db.Tasks.ToList().Find(x => x.Id == _viewModel.SelectedTask.Id).InArchive=true;
            _db.SaveChanges();
            Update();
        }

        private void Uncomplite_Click(object sender, RoutedEventArgs e)
        {
            _db.Tasks.ToList().Find(x => x.Id == _viewModel.SelectedTask.Id).NotComplite = true;
            _db.SaveChanges();
            Update();
        }
    }
}
