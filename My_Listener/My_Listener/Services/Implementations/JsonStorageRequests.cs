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
using My_Listener.Models;
using My_Listener.Services.RequestUtils;

/*
    Ref.: http://www.c-sharpcorner.com/UploadFile/2b876a/consume-web-service-using-httpclient-to-post-and-get-json-da/ 
*/

namespace My_Listener.Services.Implementations
{
    public class JsonStorageRequests : RestStorageService
    {
        Json2CollectionParser jsonParser; // class that implements Json Parsing methods.

        // Constructor
        public JsonStorageRequests(){
            jsonParser = new Json2CollectionParser(); // create instance of Json parser
        }

        // POST Request to Service. Takes TaskTodo Model as a param and return boolean to represent status
        public async Task<bool> saveToDoTask(TaskTodo taskTodo)
        {
            bool statusFlag = false;

            Uri requestUri = new Uri("http://localhost:8080/yourlist/save-task");
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.taskId = taskTodo.TaskId.ToString();
            dynamicJson.taskDesc = taskTodo.TaskDesc.ToString();
            dynamicJson.dateCreated = taskTodo.DateCreated.ToString();
            dynamicJson.location = taskTodo.Location;
            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var objClient = new HttpClient();

            try {
                HttpResponseMessage respon = await objClient.PostAsync(requestUri,
                             new StringContent(json, Encoding.UTF8, "application/json"));

                string responJsonText = await respon.Content.ReadAsStringAsync();
                if (responJsonText.Equals("OK")) {
                    statusFlag = true;
                }
            }
            catch (HttpRequestException exception) {

                Debug.WriteLine("DESCRIPTION: " + exception.Message + " STATUS CODE: " + exception.HResult);
            }

            return statusFlag;
        }


        public async Task<string> deleteToDoTask(TaskTodo taskTodo)
        {
            string status = "FAIL"; // set default to fail, in case request doesn't go through.
            Uri requestUri = new Uri("http://localhost:8080/yourlist/delete-task/" + taskTodo.TaskId.ToString());
            Debug.WriteLine(requestUri.ToString());
            var objClient = new HttpClient();

            try {
                HttpResponseMessage respon = await objClient.DeleteAsync(requestUri);

                string responJsonText = await respon.Content.ReadAsStringAsync();
                status = responJsonText;
            }
            catch (HttpRequestException exception) {

                Debug.WriteLine("DESCRIPTION: " + exception.Message + " STATUS CODE: " + exception.HResult);
            }

            return status;
        }


        public async Task<List<TaskTodo>> getToDoList() {
            Uri requestUri = new Uri("http://localhost:8080/yourlist/get-todo"); // create new request URI 
            var objClient = new HttpClient(); // create new HttpClient that calls http methods (GET, POST) to provided URI
            string temp; // to store response body read as string 
            List<TaskTodo> tempList = new List<TaskTodo>();  // temporary list to return from method

            // try make a request, if all goes well, list is populated with data from Service
            try {
                HttpResponseMessage respon = await objClient.GetAsync(requestUri); // send GET request and wait for response
                respon.EnsureSuccessStatusCode(); // throws exception if request has failed

                temp = await respon.Content.ReadAsStringAsync(); // read response to a string
                                                    // JsonArray.Parse(temp) takes String as argument to create JsonArray
                tempList = jsonParser.parseJArray2List(JsonArray.Parse(temp)); // call function to parse JsonArray to a List 
                                                                             // returns List with Todo Task Model Object
            } catch (HttpRequestException exception) {
                // if exception is throwen, tempList is returned as an empty list
                Debug.WriteLine("DESCRIPTION: " + exception.Message + " STATUS CODE: " + exception.HResult);
            }

            return tempList;
        } // end of getToDoList()
    } // end of class
} // end of namespace
