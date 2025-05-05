using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.Common.ResponseHandler
{
    public static class ResponseBuilder
    {
        public static ApiResponse Success(object data) => new()
        {
            Success = true,
            Message = "Operation successful.",
            Data = data
        };

        public static ApiResponse Success(string message) => new()
        {
            Success = true,
            Message = message
        };

        public static ApiResponse Error(string message, Exception? ex = null) => new()
        {
            Success = false,
            Message = message,
            Error = ex?.Message
        };

        public static ApiResponse NotFound(string message) => new()
        {
            Success = false,
            Message = message
        };
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Error { get; set; }
        public object? Data { get; set; }
    }
}
