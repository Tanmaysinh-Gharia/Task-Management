
using Newtonsoft.Json;
using ApiResponseModel = TaskManagement.Core.Common.ResponseHandler.ApiResponse;
namespace TaskManagement.Services;

/// <summary>
/// Manage api response and covert that response to proper object.
/// </summary>
public class ResponseHandler : IResponseHandler
{
    /// <summary>
    /// Deserialize object to class
    /// </summary>
    public T GetResponse<T>(ApiResponseModel apiResponse)
    {
        return apiResponse.Success 
            ? JsonConvert.DeserializeObject<T>(apiResponse.Data.ToString() ?? string.Empty)
            : default;
    }

    /// <summary>
    /// Get string response of api
    /// </summary>
    public string GetStringResponse(ApiResponseModel apiResponse)
    {
        return apiResponse.Success
            ? Convert.ToString(apiResponse.Data)
            : string.Empty;
    }
}