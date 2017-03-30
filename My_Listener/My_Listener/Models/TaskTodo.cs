using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
    Refs.: 1) http://stackoverflow.com/questions/553150/updating-wpf-list-when-item-changes 
     
*/
namespace My_Listener
{
    // implementing interface to monitor changes in properties of a class
    public class TaskTodo : INotifyPropertyChanged
    {
        // Handler for event raised when a property is changed on a component
        public event PropertyChangedEventHandler PropertyChanged;

        private int taskId;
        private String taskDesc;
        private DateTime dateCreated;
        // location
        // timeLeft

        // Constructors

        public TaskTodo(){}

        public TaskTodo(String taskDesc, DateTime dateCreated)
        {
            this.taskDesc = taskDesc;
            this.dateCreated = dateCreated;
        }

        // Getters/Setters

        public int TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }

        public String TaskDesc
        {
            get { return taskDesc;
                  Debug.WriteLine("### Getter used ###");
                }
            set { taskDesc = value;
                  Debug.WriteLine("### Entered Setter for Task Description ###");

                  // call the event when setter for taskDesc is triggered
                  OnPropertyChanged(string.Empty);
                }
        }

        public DateTime DateCreated
        {
            get { return dateCreated;  }
            set { dateCreated = value;  }
        }


        // Method that deligates event handling when property is changed
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
