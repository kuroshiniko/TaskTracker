using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using TaskTracker.DB;

namespace TaskTracker.MVVM
{
    public class TaskStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is ReleaseTime releaseTime)
            {
                // Предположим, что у ReleaseTime есть свойство Task, которое определяет его связанную задачу
                if (releaseTime.Task != null && releaseTime.Time < DateTime.Now)
                {
                    return Brushes.Red; 
                }
            }
            return Brushes.Black; // По умолчанию возвращаем черный цвет
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
