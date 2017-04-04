
using My_Listener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Listener.ViewModels
{
    public class TaskTodoVM : NotificationBase<TaskTodo>
    {
        public TaskTodoVM(TaskTodo taskTodo = null ) : base(taskTodo) { }

        public String TaskId
        {
            get { return This.TaskId; }
            set { SetProperty(This.TaskId, value, () => This.TaskId = value); }
        }

        public String TaskDesc
        {
            get { return This.TaskDesc; }
            set { SetProperty(This.TaskDesc, value, () => This.TaskDesc = value); }

        }

        public DateTime DateCreated
        {
            get { return This.DateCreated; }
            set { SetProperty(This.DateCreated, value, () => This.DateCreated = value); }
        }

        
    }

}
