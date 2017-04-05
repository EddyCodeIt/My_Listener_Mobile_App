using My_Listener;
using My_Listener.Models;
using My_Listener.Services.GeoLocationServices;
using My_Listener.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace My_Listener.ViewModels
{
    public class TodoCollectionVM : NotificationBase
    {
        TodoCollection todoCollection;
        private GeoLocationService myGeoService;

        ObservableCollection<TaskTodoVM> _TodoList = new ObservableCollection<TaskTodoVM>();

        #region GET/SET TodoList
        public ObservableCollection<TaskTodoVM> TodoList
        {
            get { return _TodoList; }
            set { SetProperty(ref _TodoList, value); }
        }
        #endregion

        #region GET/SET Index & Selected Index 
        int _SelectedIndex;
        public int SelectedIndex
        {
            get { Debug.WriteLine("SELECTED INDEX CHANGED - TodoCollectionVM :: " + _SelectedIndex);  return _SelectedIndex;  }
            set
            {
                if (SetProperty(ref _SelectedIndex, value))
                { RaisePropertyChanged(nameof(SelectedTask)); }
            }
        }

        public TaskTodoVM SelectedTask
        {
            get { return (_SelectedIndex >= 0) ? _TodoList[_SelectedIndex] : null; }
        }

        #endregion

        // Constructor
        public TodoCollectionVM()
        {
            // Create new Model that you want to wrap in View Model
            // Model can have a data already in it or may call http services to obtain it
            Debug.WriteLine("Constructing VM");
            todoCollection = new TodoCollection();

            myGeoService = new GeoLocationService(); // create instance of Geo Service
            myGeoService.initialiseGeoLocation(); // initialise service, ask user for access
            Debug.WriteLine(myGeoService.Geo_status);

            _SelectedIndex = -1; // Set selected index in ListView to nothing. 
                                // This property has x:Bind to it's getter in ListView on Page

            // Loop over collection in Model and add each Todo task from it to a ViewModel (ObservableCollection)
            foreach (var task in todoCollection.TodoList)
            {
                var nt = new TaskTodoVM(task); // Note that ObservableCollection _TodoList 
                                               // takes objects of type TaskTodoVM that is wrapping 
                                               // POCO Model for Task on the List.
               // nt.PropertyChanged += Task_OnNotifyPropertyChanged; // 
                _TodoList.Add(nt);
            }
        }

        public async void Add()
        {
            var task = new TaskTodoVM(); // create new view model of a task
            
            string userInput = await InputTextDialogAsync("New Task"); // call a method that represents dialog with a user
                                                                      // obtain result of an input
            if (userInput.Length == 0) { /* do nothing */ }
            else
            {
                task.TaskId = "Task" + new Random().Next(0, 10000).ToString();
                task.TaskDesc = userInput; // assign result of an input to an attribute of a VM
                task.DateCreated = DateTime.Now;

                // call geolocation services
                task.Location = await myGeoService.GetCurrentPossition();
                // task.PropertyChanged += Task_OnNotifyPropertyChanged;
                TodoList.Add(task);
                SelectedIndex = TodoList.IndexOf(task);
                todoCollection.Add(task);
            }
        }

        public void Delete()
        {
            Debug.WriteLine("Testing DELETE from View Model");
            if (SelectedIndex != -1)
            {
                Debug.WriteLine("Selected index is != -1 ");
                var task = TodoList[SelectedIndex];
                TodoList.RemoveAt(SelectedIndex);
                task.PropertyChanged += Task_OnNotifyPropertyChanged;
                todoCollection.Delete(task);
            }
        }

        public async void Update()
        {
            Debug.WriteLine("Testing UPDATE from View Model");
            if (SelectedIndex != -1)
            {
                var task = TodoList[SelectedIndex];
                task.TaskDesc = await InputTextDialogAsync("Edit Task");
                task.PropertyChanged += Task_OnNotifyPropertyChanged;
                todoCollection.Update(task);
            }
        }

        public void Task_OnNotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("On NotifyPropertyChanged Event Triggered! ");
            //todoCollection.Update((TaskTodoVM)sender);
        } // end of Task_OnNotifyPropertyChanged()


        // Method that creates dialog with user and asks for input
        // Method asynchronously calls a window by setting it's return type to Task<T>. 
        // In this case, thread that deals with a task returns String representation 
        // of an input from ContentDialog. 
        private async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox(); // user input
            inputTextBox.AcceptsReturn = false; // 
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog(); // Representing Dialog Box with a user
            dialog.Content = inputTextBox; // add input box to a dialog
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true; // enable cancel button
            dialog.PrimaryButtonText = "Ok"; // submit
            dialog.SecondaryButtonText = "Cancel"; // cancel operation
            // beggin asynch operation of a dialog box and wait for primary button to be tapped by a user
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                // if secondary button was tapped or dialog was closed, return empty String
                return "";
        } // end of InputTextDialogAsync()
    } // end of class
} // end of namespace
