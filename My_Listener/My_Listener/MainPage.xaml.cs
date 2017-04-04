
using My_Listener.ViewModels;
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

        public TodoCollectionVM TodoCollection { get; set; }

        public MainPage(){
            this.InitializeComponent();
            TodoCollection = new TodoCollectionVM();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
           
        }


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
