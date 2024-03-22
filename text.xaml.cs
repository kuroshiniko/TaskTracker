using Microsoft.Office.Interop.Word;
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
using System.Xml.Linq;
using TaskTracker.DB;
using Word = Microsoft.Office.Interop.Word;

namespace TaskTracker
{
    /// <summary>
    /// Логика взаимодействия для text.xaml
    /// </summary>
    public partial class text : System.Windows.Window
    {
        DataContext db;
        public text()
        {
            InitializeComponent();
            db = new DataContext();//подключаем бд
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Document doc = wordApp.Documents.Add();

            Microsoft.Office.Interop.Word.Paragraph paragraph = doc.Paragraphs.Add();
            paragraph.Range.Text = "Отчёт.";

            // Форматируем текст
            paragraph.Range.Font.Size = 16;
            paragraph.Range.Font.Bold = 1; // Жирный текст
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; // Выравнивание по центру

            // Добавляем пустую строку для отделения текста и таблицы
            doc.Paragraphs.Add();

            db.Employees.Load();
            List<DB.Employee> empList = db.Employees.Local.ToList();
            MessageBox.Show(empList.Count.ToString());



            // Создаем таблицу в Word
            Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(paragraph.Range, empList.Count , 3);
            table.Borders.Enable = 1;

            //// Заголовки столбцов
            table.Cell(1, 1).Range.Text = "Имя";
            table.Cell(1, 2).Range.Text = "Должность";
            table.Cell(1, 3).Range.Text = "Роль";


            // Заполняем таблицу данными
            for (int i = 0; i < empList.Count; i++)
            {
                table.Cell(i + 1, 1).Range.Text = empList[i].Name;
                table.Cell(i + 1, 2).Range.Text = empList[i].Post;
                table.Cell(i + 1, 3).Range.Text = empList[i].Role;

            }
            table = Reset(table);   
            
            doc.SaveAs2(@"C:\Users\Ded insaid\Desktop\Отчет работники.docx");
            doc.Close();
            wordApp.Quit();


            System.Diagnostics.Process.Start(@"C:\Users\Ded insaid\Desktop\Отчет работники.docx");
        }

        private Word.Table Reset(Word.Table table)
        {
            table.Cell(1, 1).Range.Font.Reset();
            table.Cell(1, 2).Range.Font.Reset();
            table.Cell(1, 3).Range.Font.Reset();
            table.Cell(1, 1).Range.ParagraphFormat.Reset();
            table.Cell(1, 2).Range.ParagraphFormat.Reset();
            table.Cell(1, 3).Range.ParagraphFormat.Reset();
            for (int i = 0; i < db.Employees.Local.ToList().Count; i++)
            {
                table.Cell(i + 1, 1).Range.Font.Reset();
                table.Cell(i + 1, 1).Range.ParagraphFormat.Reset();
                table.Cell(i + 1, 2).Range.Font.Reset();
                table.Cell(i + 1, 2).Range.ParagraphFormat.Reset();
                table.Cell(i + 1, 3).Range.Font.Reset();
                table.Cell(i + 1, 3).Range.ParagraphFormat.Reset();

            }
            return table;
        }
    }
}
