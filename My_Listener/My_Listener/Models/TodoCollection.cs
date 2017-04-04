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
            
            
            //TodoList = storageService.getToDoList().Result;
            //loadList();
            // Debug.WriteLine("Elements inside TODO LIST: " + TodoList.Count.ToString());
        }

        private async void loadList()
        {
            TodoList = await Task.Run(() => storageService.getToDoList().Result);
        }

        // public void add
        // call service to add
        // add to TodoList
        public async void Add(TaskTodo task)
        {
            if (!TodoList.Contains(task))
            {
                TodoList.Add(task);
                var status = await storageService.saveToDoTask(task);

                Debug.WriteLine("Adding Task (TodoCollection) STATUS => " + status.ToString());
            }
        }


        // public void delete
        // call service to delete
        // delete from TodoList

        public void Delete(TaskTodo person)
        {
            if (TodoList.Contains(person))
            {
                TodoList.Remove(person);
                var status = storageService.deleteToDoTask(person);
                Debug.WriteLine("Deleting Task (TodoCollection) STATUS => " + status.ToString());
            }
        }
        // public void update
        // call service task edited
        // edit task in TodoList
        public async void Update(TaskTodo task)
        {
            var status = await storageService.saveToDoTask(task);
            Debug.WriteLine("Updating Task (TodoCollection) STATUS => " + status.ToString());
        }
    }


}
