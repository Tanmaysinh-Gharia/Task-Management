using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.ApiRoutes
{
    public static class TaskManagementRoutes
    {
        public const string TaskApi = "task";
        public const string Create = "create";
        public const string Update = "update/{id}";
        public const string Delete = "delete/{id}";
        public const string ChangeStatus = "change-status/{id}";
        public const string FilteredList = "list";
        public const string History = "history/{id}";
        public const string GetTask = "get/{id}";
    }
}
