using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Listener.Services.Implementations
{
    interface RestStorageService
    {
        Task<bool> saveToDoTask(TaskTodo taskTodo);
        Task<ObservableCollection<TaskTodo>> getToDoList();
        Task<string> deleteToDoTask(int taskIndex);
        Task<string> editToDoTask(TaskTodo taskTodo);
    }
}
