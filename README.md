# My_Listener_Mobile_App
Universal Windows Application programmed on C#. Application that records your tasks. 

## Overview of an app

Application is a To Do List that is able to save your daily tasks to a Cloud based
REST API. 

Task To do includes:
*Description of a task
*Date
*Geo location of where task was saved

Application allows you to:
*Loads saved tasks from exteranal storage (Cloud)
*Save task
*Delete task
*Edit task
*All tasks are saved on Cloud via HTTP requests. 

Note: if user doesn't have internet access or cloud storage is offline,
application still allow user to save tasks locally, but will be wiped out
when application is restarted.

On start up:
*Application will ask to allow to access geolocations if Geo sensor is active on a device
*Application will notify user if Geo sensor is not active


## Development Features & Application Architecture

App is developed using MVVM design pattern.
MVVM is a Model-View-View Model. 

### Models
A Model is representing an objects/classes with some date.
In my application, I have two models, for Todo List (TodoCollection class) and a task Todo (TaskTodo class).

Task Model is very simple POCO, consists of parameters and get/set methods for it.

Todo List model contains a List of tasks and methods to add, delete, update a task,
and constructor that calls Service to get all tasks from Cloud when application is launching.

Todo List model calls Service class methods to send http requests to cloud storage.

Overall, I understand Model as a part of application for storing data in memory during it's
run time. Model can be fed with data by other classes that provide such functionality.

### Model Views
A MV is glue between a Model and application View (pages on a UI).
For each model there is a model view.

It serves as wrapper for Models, with INotifyProperty capabilities and is bounded to View.
Whenever there is a change in a View Model properties or collections, UI is able to respond to
it and update page accordingly.

I want to note a class called ViewModelHelper that is composed of two classes.
NotificationBase and NotificationBase<T> that implement INotifyPropertyChanged interface.

NotificationBase contains implementation for SetProperty methods that are used in VMs
to set new values for properties. 

This class was adopted from MVVM [guide] (https://blogs.msdn.microsoft.com/johnshews_blog/2015/09/09/a-minimal-mvvm-uwp-app/).

In VM for Todo List, I am using observable collection to bind with UI's List View element. 
Observable collection is being populated from Model's List during construction time
of VM. 


### View 

View is a actually page that interats with a user. 
View is bound to VM via {x:Bind} to get necessary  data. 
New feature of Windows 10 is {x:Bind} markup extension, alternative to {Binding}.
Despite a lack of some features that {Binding} provide, {x:Bind} runs in less time,
consume less memory and support better debugging. For more information [see...] (https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/x-bind-markup-extension)

X:bind allowed me to bind VM to view and use it's getters to access data,
as well as, allows to use public methods defined in VM. Code behind a view page does not need
implementation for OnClick or OnTap events anymore, as you can inject method from other class.
This allows to develope reusable and scalable architecture. 

### Services

In my application, I have two services: 
* Client requests service to an API to store and retrieve data.
* Geo Location Service to obtain current possition coordinates to get city and country out of it.

** Client - Rest Service **

Service up on cloud accepts HTTP requests to a specific URLs and return resources back to 
an application. Body of request/response carries a data that can be represented 
in various file formats. In my architecture I use JSON format to send data accross a network.

RestStorageService interface encapsulates a methods to a Model to request some data
from REST API on cloud. JsonStorageRequest class implements those methods, but programmer 
is free to change implementation of a Service without breaking other application code.

Client application builds a json following way:
1. Create an object that will be represented in json
2. Add member variables to it at run time
3. Use JsonConvert class from Newtonsoft library (available on NuGet) to serialize 
object and obtain JSON String

This string than can be used as a paramater for the request functions available 
from HttpClient class (Windows.Net.Http library).

To obtain a response use HttpResponseMessage class as catcher.
Use function ReadAsStringAsync to get response message as string.














