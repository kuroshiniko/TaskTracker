using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.MVVM
{
    public class Statistic
    {
        public string Name { get; set; }
        public int Complite {  get; set; }
        public int UnComplite { get; set; }
        public Statistic(DB.Employee a,int _uncomp,int _comp) { 
            Name = a.Name;
            Complite = _comp;
            UnComplite = _uncomp;
        }
    }
}
