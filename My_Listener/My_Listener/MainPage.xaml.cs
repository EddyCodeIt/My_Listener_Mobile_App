using My_Listener.Services;
using My_Listener.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace My_Listener
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page 
    {
        // Collection of tasks
        private ObservableCollection<TaskTodo> listOfTasks;
        public ObservableCollection<TaskTodo> ListOfTasks{
            get { return listOfTasks; }
        }

        private RestStorageService requestStorageService;


        public MainPage(){
            this.InitializeComponent();
            listOfTasks = new TodoCollection();
            requestStorageService = new NewtonsoftJsonStorageRequests();
            listOfTasks = requestStorageService.getToDoList().Result;
        }

        // ListView Control
        private void toDoList_SelectionChanged(object sender, SelectionChangedEventArgs e){

           
        }


        #region Command Bar Methods

        private void settings_Click(object sender, RoutedEventArgs e){

        }
        
        // Add button activates Input Box for user to enter a new Task
        private async void addButton_Click(object sender, RoutedEventArgs e){
            // Store value returned by the function that calls Dialog Box
            string task = await InputTextDialogAsync("Add New Task");

            // check if returned task is not empty String
            if(task.Length == 0){ /* do nothing */ }
            else{
                // Add obtained value to an Observable Collection of Tasks.
                // UI automatically pick up new changes in the collection
                // thanks to data binding and ObservableCollection<T> interface
                bool result = await requestStorageService.saveToDoTask(new TaskTodo(task, DateTime.Now));

                if (result)
                {
                    Debug.WriteLine("OK");
                } else{
                    Debug.WriteLine("FAIL");
                }


                listOfTasks.Add(new TaskTodo(task, DateTime.Now));
            } 
        }

        private void delete_Click(object sender, RoutedEventArgs e){
            // Call a function RemoveAt(index) inherited from Observable Collection Interface
            // Index is obtained from front end ListView with x:Name = "toDoList"

            // check if selectedIndex is not pointing to any item (value -1)
            // Debug.WriteLine(toDoList.SelectedIndex);
            // if not, remove item at selected index
            if (toDoList.SelectedIndex != -1)
            {
                listOfTasks.RemoveAt(toDoList.SelectedIndex);
            }           
        }

        private async void edit_Click(object sender, RoutedEventArgs e){
            // Similar to add a new task, call anync Dialog Box and allow user edit 
            // task. 
            string editedTask = await InputTextDialogAsync("Editting Task");
            // Get selected item index
            if (editedTask.Length == 0) { /* Do nothing */ }
            else {
                listOfTasks.ElementAt(toDoList.SelectedIndex).TaskDesc = editedTask;

                Debug.WriteLine(listOfTasks.ElementAt(toDoList.SelectedIndex).TaskDesc.ToString());
                
            }
        }

        #endregion Command Bar

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
        }


    }
}
