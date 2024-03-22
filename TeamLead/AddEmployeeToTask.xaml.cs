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
using System.Windows.Shapes;
using TaskTracker.DB;

namespace TaskTracker.TeamLead
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeeToTask.xaml
    /// </summary>
    public partial class AddEmployeeToTask : Window
    {
        DataContext _db;
        private int taskId=1;
        List<DB.Employee> data;// cписок работников
        private List<int> id_emps = new List<int>();// список id работников, которых выводить
        private List<int> id_empsToAdd = new List<int>();// список id работников, которых записывать
        TeamLeadList list;
        
        public AddEmployeeToTask(TeamLeadList a, int task_id)
        {
            taskId = task_id;
            _db = new DataContext();
            InitializeComponent();
            _db.Employees.Load();
            data = _db.Employees.Local.ToList().FindAll(x=>x.Role=="Работник");
            list = a;
            foreach (var item in data)
            {
                 id_emps.Add(item.Id);
                 AllEmp.Items.Add(item.Name + " " + item.Post);
            }

        }



        private void Window_Closed(object sender, EventArgs e)
        {
            list.Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            data.Remove(data.Find(x => x.Id == id_emps[AllEmp.SelectedIndex]));
            id_empsToAdd.Add(id_emps[AllEmp.SelectedIndex]);
            id_emps.Remove(id_emps[AllEmp.SelectedIndex]);
            ComboBox comboBox1 = new ComboBox();

            comboBox1.Width = 256;
            comboBox1.Height = 20;
            foreach (var item in data)
            {
                comboBox1.Items.Add(item.Name + " " + item.Post);
            }

            additionalComboBoxesPanel.Children.Add(comboBox1);
            comboBox1.SelectedIndex = 0;
        }


        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                id_empsToAdd.Add(data.Find(x => x.Id == id_emps[AllEmp.SelectedIndex]).Id);
                for (int i = 0; i < additionalComboBoxesPanel.Children.Count; i++)
                {
                    DB.TaskAssignment a = new DB.TaskAssignment();
                    a.TaskId = taskId;
                    a.EmployeeId = id_empsToAdd[i];
                    _db.TaskAssignments.Add(a);
                }

                _db.SaveChanges();
                MessageBox.Show("Задание распределено");
                this.Close();
                
            }
            catch(System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                MessageBox.Show("Один или несколько сотрудников уже выполняет данную задачу.");
            }

        }
    }
}
