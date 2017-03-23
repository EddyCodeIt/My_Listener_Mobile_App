using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private List<TaskTodo> listOfTasks = new List<TaskTodo>();

        public MainPage()
        {
            this.InitializeComponent();

            listOfTasks.Add(new TaskTodo { TaskDesc = "Study for Selenium MCQ", DateCreated = DateTime.Now });
            listOfTasks.Add(new TaskTodo { TaskDesc = "To do List for C# Project", DateCreated = DateTime.Now });
            listOfTasks.Add(new TaskTodo { TaskDesc = "Redesign Pivot for C# Project", DateCreated = DateTime.Now });
            listOfTasks.Add(new TaskTodo { TaskDesc = "Study for SSRAD Assesment 2", DateCreated = DateTime.Now });
            listOfTasks.Add(new TaskTodo { TaskDesc = "Create Neo4j DB structure for timetable", DateCreated = DateTime.Now });
            listOfTasks.Add(new TaskTodo { TaskDesc = "Documment main project FX Client", DateCreated = DateTime.Now });



           
            toDoList.ItemsSource = listOfTasks;
        }

        // ListView Control
        private void toDoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        #region Command Bars Navigation Methods

        #region Top Bar
        private void settings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void admin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void search_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion Top Bar

        #region Bottom Bar
        
        private void goToTasks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void goToDiary_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion Bottom Bar

        #endregion Command Bar

    }
}
