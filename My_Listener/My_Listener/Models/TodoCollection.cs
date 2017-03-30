using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace My_Listener
{
    class TodoCollection : ObservableCollection<TaskTodo>
    {

        // constructor
        public TodoCollection()
        {
            //Add(new TaskTodo("Study for Selenium MCQ", DateTime.Now));
            //Add(new TaskTodo("To do List for C# Project", DateTime.Now));
            //Add(new TaskTodo("Redesign Pivot for C# Project", DateTime.Now));
        }


    }


}
