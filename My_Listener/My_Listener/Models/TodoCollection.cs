using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using System.Diagnostics;
using My_Listener.Services;
using My_Listener.Services.Implementations;

namespace My_Listener.Models
{
    public class TodoCollection
    {
        public List<TaskTodo> TodoList { get; set; }
        public String Name { get; set; }

        private RestStorageService storageService;
        // constructor
        public TodoCollection()
        {
            storageService = new JsonStorageRequests();

            var listLoadingTask = Task.Run(() => storageService.getToDoList().Result);
            TodoList = listLoadingTask.Result;

        }

        public async void Add(TaskTodo task)
        {

            TodoList.Add(task);
            var status = await storageService.saveToDoTask(task);

            Debug.WriteLine("Adding Task (TodoCollection) STATUS => " + status.ToString());

        }


        public void Delete(TaskTodo todoTask)
        {
            Debug.WriteLine("Testing DELETE from Model");
            if (TodoList.Contains(todoTask))
            {
                TodoList.Remove(todoTask);
                var status = storageService.deleteToDoTask(todoTask);
                Debug.WriteLine("Deleting Task (TodoCollection) STATUS => " + status.ToString());
            }
        } // end of Delete()

        public async void Update(TaskTodo task)
        {
            var status = await storageService.saveToDoTask(task);
            Debug.WriteLine("Updating Task (TodoCollection) STATUS => " + status.ToString());
        } // end of Update()
    }


}
