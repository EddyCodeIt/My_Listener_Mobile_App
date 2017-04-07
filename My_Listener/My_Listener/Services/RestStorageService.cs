using My_Listener.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// interface to define Service CRUD within app 
namespace My_Listener.Services
{
    interface RestStorageService
    {
        Task<bool> saveToDoTask(TaskTodo taskTodo);
        Task<List<TaskTodo>> getToDoList();
        Task<string> deleteToDoTask(TaskTodo taskTodo);

    }
}
