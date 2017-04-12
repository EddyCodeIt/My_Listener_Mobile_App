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
using Windows.UI.Popups;

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

            // for local debuggin use http://localhost:8080/
            Uri requestUri = new Uri("http://52.35.177.146:8080/yourlist/save-task"); // request URI 
            dynamic dynamicJson = new ExpandoObject(); // create dynamic object that will store values for json
            // create dynamic object that representing json; add todo task data to it 
            dynamicJson.taskId = taskTodo.TaskId.ToString(); 
            dynamicJson.taskDesc = taskTodo.TaskDesc.ToString();
            dynamicJson.dateCreated = taskTodo.DateCreated.ToString();
            dynamicJson.location = taskTodo.Location;
            // Store serialized object in string to include it as body of a request
            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson); // serialize json object and store it in string 
            var objClient = new HttpClient(); // create Http Client to access http methods

            // try make a request
            try {
                // send a post request to service and wait for respons; store response in HttpResponseMessage object
                HttpResponseMessage respon = await objClient.PostAsync(requestUri,
                             new StringContent(json, Encoding.UTF8, "application/json"));

                string responJsonText = await respon.Content.ReadAsStringAsync(); // read respons from service as string
                if (responJsonText.Equals("OK")) {
                    statusFlag = true;
                }
            }
            catch (HttpRequestException exception) {
                
                // catch Http Exception
                Debug.WriteLine("DESCRIPTION: " + exception.Message + " STATUS CODE: " + exception.HResult);
            }

            return statusFlag;
        }


        public async Task<string> deleteToDoTask(TaskTodo taskTodo)
        {
            string status = "FAIL"; // set default to fail, in case request doesn't go through.
            Uri requestUri = new Uri("http://52.35.177.146:8080/yourlist/delete-task/" + taskTodo.TaskId.ToString()); // send request to URI with task id number
            Debug.WriteLine(requestUri.ToString());
            var objClient = new HttpClient(); // new http client to make a request 

            try {
                HttpResponseMessage respon = await objClient.DeleteAsync(requestUri); // get response message from service

                status = await respon.Content.ReadAsStringAsync(); // get string representation of response

            }
            catch (HttpRequestException exception) {

                // handle exception if deletion wasn't complete on server side
                Debug.WriteLine("DESCRIPTION: " + exception.Message + " STATUS CODE: " + exception.HResult);
            }

            return status;
        }


        public async Task<List<TaskTodo>> getToDoList() {
            // 52.35.177.146 localhost
            Uri requestUri = new Uri("http://52.35.177.146:8080/yourlist/get-todo"); // create new request URI 
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
            } catch (HttpRequestException exception)  {
                // if exception is thrown, tempList is returned as an empty list

                Debug.WriteLine("DESCRIPTION: " + exception.Message + " STATUS CODE: " + exception.HResult);
            }

            return tempList;
        } // end of getToDoList()
    } // end of class
} // end of namespace
