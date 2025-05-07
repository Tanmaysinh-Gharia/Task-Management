using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.ApiRoutes
{
    public static class UserManagementRoutes
    {
        public const string UserApi = "user";

        #region Routes
        public const string GetUsers = "user-list";
        public const string GetUserById = "get-user/{id}";
        public const string AddUser = "add-user";
        public const string UpdateUser = "update-user";
        public const string DeleteUser = "delete-user/{id}";
        #endregion
    }
}
