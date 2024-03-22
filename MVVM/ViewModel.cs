using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TaskTracker.DB;
using Tasks = TaskTracker.DB.Task;
using System.Windows;
using System.Data.Entity;

namespace TaskTracker.MVVM
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private Tasks selectedTask;
        private ReleaseTime selectedTime;
        private bool xColor = false;

        public Tasks SelectedTask
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

        public ReleaseTime SelectedTime
        {
            get { return selectedTime; }
            set
            {
                selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }
        

        public ObservableCollection<Tasks> TasksView { get; set; }
        public ObservableCollection<ReleaseTime> TimeView { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ViewModel()//создаём вывод данных для mvvm паттерна
        {
            DataContext _db = new DataContext();
            TasksView = AddingTasks(_db, new ObservableCollection<Tasks>());
            TimeView = AddingTime(_db, new ObservableCollection<ReleaseTime>());
        }

        public void UpdateView(DataContext db)
        {
            TasksView.Clear();
            TimeView.Clear();
            TasksView = AddingTasks(db, TasksView);
            TimeView = AddingTime(db, TimeView);
        }
        private ObservableCollection<Tasks> AddingTasks(DataContext db, ObservableCollection<Tasks> new_list)//заполнение задач из бд
        {
            db.Tasks.Load();
            var tempList = db.Tasks.Local.ToList();//подгружаем данные из бд
            foreach (var task in tempList)
            {
                if(!task.InArchive&&!task.NotComplite)new_list.Add(task);// добавляем их в нужный нам список
            }
            return new_list;
        }
        private ObservableCollection<ReleaseTime> AddingTime(DataContext db, ObservableCollection<ReleaseTime> new_list)//заполнение времени из базы данных
        {
            db.ReleaseTime.Load();
            db.Tasks.Load();
            var tempList = db.ReleaseTime.Local.ToList();//подгружаем данные из бд
            foreach (var time in tempList)
            {
                if(!db.Tasks.ToList().Find(x=>x.Id==time.TaskId).InArchive && !db.Tasks.ToList().Find(x => x.Id == time.TaskId).NotComplite) new_list.Add(time);//добавляем их в нужный нам список
            }
            return new_list;
        }
        public void UpdateData()
        {
            DataContext _db = new DataContext();
            TasksView = AddingTasks(_db, new ObservableCollection<Tasks>());
            TimeView = AddingTime(_db, new ObservableCollection<ReleaseTime>());
        }


    }
    
}

