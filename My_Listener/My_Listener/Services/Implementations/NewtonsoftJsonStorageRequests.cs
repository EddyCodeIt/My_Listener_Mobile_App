using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http;
using System.Diagnostics;
using System.IO;
using Windows.Data.Json;

/*
    Ref.: http://www.c-sharpcorner.com/UploadFile/2b876a/consume-web-service-using-httpclient-to-post-and-get-json-da/ 
*/

namespace My_Listener.Services.Implementations
{
    class NewtonsoftJsonStorageRequests : RestStorageService
    {
        public async Task<bool> saveToDoTask(TaskTodo taskTodo)
        {
            bool statusFlag = false;

            Uri requestUri = new Uri("http://localhost:8080/yourlist/save-task");
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.taskDesc = taskTodo.TaskDesc.ToString();
            dynamicJson.dateCreated = taskTodo.DateCreated.ToString();
            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var objClient = new HttpClient();
            HttpResponseMessage respon = await objClient.PostAsync( requestUri, 
                                         new StringContent(json, Encoding.UTF8, "application/json"));
            string responJsonText = await respon.Content.ReadAsStringAsync();

            Debug.WriteLine(responJsonText);
            if (responJsonText.Equals("OK")){
                return statusFlag = true;
            } else {
                return statusFlag;
            }
        }

        public Task<string> deleteToDoTask(int taskIndex)
        {
            throw new NotImplementedException();
        }

        public Task<string> editToDoTask(TaskTodo taskTodo)
        {
            throw new NotImplementedException();
        }

        public async Task<ObservableCollection<TaskTodo>> getToDoList()
        {
            Uri requestUri = new Uri("http://localhost:8080/yourlist/get-todo");
            var objClient = new HttpClient();

            HttpResponseMessage respon = await objClient.GetAsync(requestUri);

            string temp = await respon.Content.ReadAsStringAsync();
            var tempJson = JsonArray.Parse(temp);

            // takes tempJson and returns ObservableCollection
            
            return createFreshCollection(tempJson);
        }

        private ObservableCollection<TaskTodo> createFreshCollection(JsonArray tempJson)
        {
            ObservableCollection<TaskTodo> tempList = new ObservableCollection<TaskTodo>();
            foreach (var item in tempJson)
            {
                var obj = item.GetObject();

                TaskTodo taskTodo = new TaskTodo();

                foreach (var key in obj.Keys)
                {
                    IJsonValue value;
                    if (!obj.TryGetValue(key, out value))
                        continue;

                    switch (key)
                    {
                        case "taskDesc":
                            taskTodo.TaskDesc = value.GetString();
                            break;
                        case "dateCreated":
                            DateTime dt = Convert.ToDateTime(value.GetString());
                            taskTodo.DateCreated = dt;
                            break;
                    }
                } // inner foreach

                tempList.Add(taskTodo);
            } // outter foreach
            return tempList; 
        } // end of json to collection converter



    }
}
