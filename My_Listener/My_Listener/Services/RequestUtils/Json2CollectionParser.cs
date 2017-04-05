using My_Listener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace My_Listener.Services.RequestUtils
{
    class Json2CollectionParser
    {

        // REF.: https://github.com/arkiq/AppDataBind/blob/master/App9databind2/MainPage.xaml.cs
        public List<TaskTodo> parseJArray2List(JsonArray tempJson)
        {
            List<TaskTodo> tempList = new List<TaskTodo>();
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
                        case "taskId":
                            taskTodo.TaskId = value.GetString();
                            break;
                        case "taskDesc":
                            taskTodo.TaskDesc = value.GetString();
                            break;
                        case "dateCreated":
                            DateTime dt = Convert.ToDateTime(value.GetString());
                            taskTodo.DateCreated = dt;
                            break;
                        case "location":
                            taskTodo.Location = value.GetString();
                            break;

                    }
                } // inner foreach

                tempList.Add(taskTodo);
            } // outter foreach
            return tempList;
        } // end of json to collection converter
    }
}
