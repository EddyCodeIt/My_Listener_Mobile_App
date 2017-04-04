using My_Listener;
using My_Listener.Models;
using My_Listener.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace My_Listener.ViewModels
{
    public class TodoCollectionVM : NotificationBase
    {
        TodoCollection todoCollection;

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
            get { return _SelectedIndex; }
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
            todoCollection = new TodoCollection();
            _SelectedIndex = -1;

            foreach (var task in todoCollection.TodoList)
            {
                var nt = new TaskTodoVM(task);
                nt.PropertyChanged += Task_OnNotifyPropertyChanged;
                _TodoList.Add(nt);
            }
        }

        public void Add()
        {
            var task = new TaskTodoVM();
            task.PropertyChanged += Task_OnNotifyPropertyChanged;
            TodoList.Add(task);
            SelectedIndex = TodoList.IndexOf(task);

        }

        public void Delete()
        {
            if (SelectedIndex != -1)
            {
                var task = TodoList[SelectedIndex];
                TodoList.RemoveAt(SelectedIndex);
                todoCollection.Delete(task);
            }
        }
        public void Task_OnNotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("On NotifyPropertyChanged Event Triggered! ");
            todoCollection.Update((TaskTodoVM)sender);
        }
    }
}
