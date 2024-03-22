using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
using Word = Microsoft.Office.Interop.Word;

namespace TaskTracker.Employee
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : System.Windows.Window
    {
        private int _id;//id работника, который пишет отчёт
        private int _id_task;//id задачи
        EmployeeList returnWindow;
        DataContext _db;
        string _docFile = @"EmployeeFolder\Sample.docx";
        public ReportWindow(EmployeeList a,int id,int task_id)
        {
            _db = new DataContext();
            InitializeComponent();
            _id=id;
            _id_task=task_id;
            returnWindow = a;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _db.Employees.Load();
            Report new_report = new DB.Report();//создание репорта
            new_report.TaskId = _id_task;
            new_report.EmployeeName = _db.Employees.Local.ToList().Find(x=>x.Id==_id).Name;//ищем имя сотрудника, который написал отчёт
            new_report.ReportText = reportText.Text;
            _db.Report.Add(new_report);//добавляем отчёт в базу данных
            _db.SaveChanges();
            MessageBox.Show("Отчёт добавлен");
            ExportWord(_docFile,new_report);
            returnWindow.Show();
            this.Close();
        }
        private void ExportWord(string filename,Report a)
        {
            if (File.Exists(filename))
            {
                FileInfo _fileInfo = new FileInfo(filename);

                var items = new Dictionary<string, string>
                {
                    {"<Name>",a.EmployeeName },
                    {"<NameOfTask>",_db.Tasks.ToList().Find(x=>x.Id==a.TaskId).TaskName},
                    {"<ReportText>",this.reportText.Text}

                };
                Process(items, _fileInfo,a);
                MessageBox.Show("Отчёт был сформирован");
            }
            else
            {
                MessageBox.Show("Образец не найден");
            }
        }
        public void Process(Dictionary<string, string> items, FileInfo _fileInfo, Report a)
        {
            Word.Application app = null;
            try
            {
                app = new Word.Application();
                Object file = _fileInfo.FullName;

                Object missing = Type.Missing;

                app.Documents.Open(file);

                foreach (var item in items)
                {
                    Word.Find find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    Object replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: missing,
                        Replace: replace);
                }
                Object newFileName = System.IO.Path.Combine(_fileInfo.DirectoryName, _db.Tasks.ToList().Find(x => x.Id == a.TaskId).TaskName+ " "+ a.EmployeeName + " " + DateTime.Now.ToString("yyyyMMdd"));
                app.ActiveDocument.SaveAs2(newFileName);
                app.ActiveDocument.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (app != null) app.Quit();
            }
        }
    }
}

