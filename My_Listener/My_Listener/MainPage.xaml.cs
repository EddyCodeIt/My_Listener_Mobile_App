using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<TaskTodo> listOfTasks { get; set; }
        
        public ObservableCollection<TaskTodo> ListOfTasks
        {
            get { return listOfTasks; }
        }


        public MainPage()
        {
            this.InitializeComponent();
            listOfTasks = new TodoCollection();
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

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            string task = await InputTextDialogAsync("Title");
            listOfTasks.Add(new TaskTodo (task, DateTime.Now));

        }

        private async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }
    }
}
