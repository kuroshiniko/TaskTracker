using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DB;

namespace TaskTracker.MVVM
{
    internal class ViewModelEmployee : INotifyPropertyChanged
    {
        private DB.Task selectedTask;//выбранный таск
        private ReleaseTime selectedTime;//выбранное время
        private int _id;//id работника

        public DB.Task SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
                // После изменения SelectedTask, выбираем соответствующее время
                SelectedTime = TimeView.FirstOrDefault(t => t.TaskId == value?.Id);
                OnPropertyChanged(nameof(SelectedTime)); // Уведомляем о изменении SelectedTime
            }
        }

        public ReleaseTime SelectedTime//выбранное время на задание
        {
            get { return selectedTime; }
            set
            {
                selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }

        public ObservableCollection<ReleaseTime> TimeView { get; set; }//создаём коллекцию
        public ObservableCollection<DB.Task> TasksView { get; set; }//создаём коллекцию




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ViewModelEmployee(int id)//создаём вывод данных для mvvm паттерна
        {
            DataContext _db = new DataContext();
            _id = id;
            TasksView = AddingTasks(_db, new ObservableCollection<DB.Task>(),_id);//используем метод для заполнения
            TimeView = AddingTime(_db, new ObservableCollection<ReleaseTime>());//используем метод для заполнения
        }

        public void UpdateView(DataContext db)
        {
            TasksView.Clear();//очищаем всё, что было на форме
            TimeView.Clear();//очищаем всё, что было на форме
            TasksView = AddingTasks(db, TasksView,_id);
            TimeView = AddingTime(db, TimeView);
        }
        private ObservableCollection<DB.Task> AddingTasks(DataContext db, ObservableCollection<DB.Task> new_list, int id)//заполнение задач из бд для определённого работника
        {
            db.Tasks.Load();
            db.TaskAssignments.Load();
            var tasks = db.Tasks.Local.ToList();//загружаем все задачи
            var tempList = db.TaskAssignments.Local.ToList().FindAll(x=>x.EmployeeId==_id);//подгружаем данные из бд
            for(int i=0;i<tempList.Count;i++)//находим задания, обозначенные пользователю
            {
                var a = tasks.Find(x => x.Id == tempList[i].TaskId);
                if(!a.InArchive&&!a.NotComplite)
                new_list.Add(tasks.Find(x => x.Id == tempList[i].TaskId));
            }
            return new_list;
        }
        private ObservableCollection<ReleaseTime> AddingTime(DataContext db, ObservableCollection<ReleaseTime> new_list)//заполнение времени из базы данных
        {
            db.ReleaseTime.Load();
            var tempList = db.ReleaseTime.Local.ToList();//подгружаем данные из бд
            foreach (var time in tempList)
            {
                new_list.Add(time);//добавляем их в нужный нам список
            }
            return new_list;
        }
        public void UpdateData()
        {
            DataContext _db = new DataContext();
            TasksView = AddingTasks(_db, new ObservableCollection<DB.Task>(),_id);
            TimeView = AddingTime(_db, new ObservableCollection<ReleaseTime>());
        }
    }
}
