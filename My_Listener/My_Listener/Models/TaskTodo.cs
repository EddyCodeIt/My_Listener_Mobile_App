using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Listener
{
    public class TaskTodo
    {
        private String taskDesc;
        private DateTime dateCreated;
        // location
        // timeLeft

        public TaskTodo(String taskDesc, DateTime dateCreated)
        {
            this.taskDesc = taskDesc;
            this.dateCreated = dateCreated;
        }

        public String TaskDesc
        {
            get { return taskDesc;  }
            set { taskDesc = value;  }
        }

        public DateTime DateCreated
        {
            get { return dateCreated;  }
            set { dateCreated = value;  }
        }


    }

}
