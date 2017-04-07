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
using Windows.UI.Popups;

namespace My_Listener.Models
{

    /*
        This class represents a Model for Todo List.
        Class contains List collection of tasks
        Model also uses Cloud Service to obtain data 
        This class can be wrapped in View Model that is bound to UI.
        
    */
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

            if (TodoList.Count == 0)
            {
                //await new MessageDialog("Ops!!! Can't get to Listener Service... /n Are you connected to WEB?!").ShowAsync();
            }
        }

        // add task to local collection and send request to cloud service
        public async void Add(TaskTodo task)
        {
            TodoList.Add(task);
            var status = await storageService.saveToDoTask(task);

            if (!status)
            {
                await new MessageDialog("Your Task wasn't saved to cloud storage! /n Maybe we are not connected?").ShowAsync();
            }

            Debug.WriteLine("Adding Task (TodoCollection) STATUS => " + status.ToString());

        }

        // delete from local collection and send request to cloud service
        public async void Delete(TaskTodo todoTask)
        {
            Debug.WriteLine("Testing DELETE from Model");
            if (TodoList.Contains(todoTask))
            {
                TodoList.Remove(todoTask);
                var status = storageService.deleteToDoTask(todoTask);

                if (status.Equals("FAIL"))
                {
                    await new MessageDialog("Deletion failed to process on server! /n Check if you have Internet connecton...").ShowAsync();
                }
                Debug.WriteLine("Deleting Task (TodoCollection) STATUS => " + status.ToString());
            }
        } // end of Delete()

        // Send request to service with update  
        public async void Update(TaskTodo task)
        {
            var status = await storageService.saveToDoTask(task);

            if (!status)
            {
                await new MessageDialog("Task You've edited wasn't saved to cloud storage! /n Maybe we are not connected?").ShowAsync();
            }
            Debug.WriteLine("Updating Task (TodoCollection) STATUS => " + status.ToString());
        } // end of Update()
    }


}
