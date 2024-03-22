using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace TaskTracker.TeamLead
{
    /// <summary>
    /// Логика взаимодействия для TeamLeadList.xaml
    /// </summary>
    public partial class TeamLeadList : System.Windows.Window
    {
        DataContext _db;
        ViewModel _viewModel;
        BindingList<ReportView> _reports = new BindingList<ReportView>() ;//вывод отчётов
        BindingList<MVVM.Statistic> _stat = new BindingList<MVVM.Statistic>();
        public TeamLeadList()
        {
            InitializeComponent();
            _db = new DataContext();
            _viewModel = new ViewModel();
            DataContext = _viewModel;
            _stat = StatisticView();
            this.Statistic.ItemsSource = _stat;
            _db.Report.Load();
            AddReportView();
            ReportGrid.ItemsSource = _reports;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddEmployeeToTask _addEmpWindow = new AddEmployeeToTask(this, _viewModel.SelectedTask.Id);
                _addEmpWindow.Show();
                this.Hide();
            }
            catch(NullReferenceException ex)
            {
                MessageBox.Show("Выберите задачу");
            }
        }
        private void AddReportView()
        {               
            foreach(var item in _db.Report.Local.ToList())
            {
                _reports.Add(new ReportView(item));
            }
        }
        private void Update()
        {
            ReportGrid.ItemsSource = null;
            foreach (var item in _db.Report.Local.ToList())
            {
                _reports.Add(new ReportView(item));
            }
            ReportGrid.ItemsSource = _reports;
            _stat = StatisticView();
            this.Statistic.ItemsSource = _stat;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void StatisticButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SortByTaskname_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportGrid.ItemsSource = null;
                ReportGrid.Items.Clear(); _reports.Clear();
                foreach (var item in _db.Report.Local.ToList())
                {
                    if (_db.Tasks.ToList().Find(x => x.Id == item.TaskId).TaskName == this.TaskNameSort.Text) _reports.Add(new ReportView(item));
                }
                ReportGrid.ItemsSource = _reports;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void TabItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Update();
        }

        private void SortByEmployeename_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportGrid.ItemsSource = null;
                ReportGrid.Items.Clear(); _reports.Clear();
                foreach (var item in _db.Report.Local.ToList())
                {
                    if (item.EmployeeName == this.EmployeeNameSort.Text) _reports.Add(new ReportView(item));
                }
                ReportGrid.ItemsSource = _reports;
                Update();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private BindingList<MVVM.Statistic> StatisticView()
        {

            _db.Tasks.Load();
            _db.Employees.Load();
            BindingList<MVVM.Statistic> new_list = new BindingList<MVVM.Statistic>();
            foreach (var emp in _db.Employees.Local.ToList())
            {
             var result = _db.GetTaskCountsForEmployee(emp.Id).FirstOrDefault();

                if (result != null)
                {
                    int notCompletedCount = result.NotCompletedCount;
                    int completedCount = result.CompletedCount;

                    new_list.Add(new MVVM.Statistic(emp, notCompletedCount, completedCount));
                }
                
                
            }


            return new_list;
        }
       
    }
}
