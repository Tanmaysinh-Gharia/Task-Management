
using Newtonsoft.Json;
using System.Net;
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

    /// <summary>
    /// Get boolean response of api
    /// </summary>
    //public bool GetBooleanResponse(ApiResponseModel apiResponse)
    //{
    //    bool.TryParse(Convert.ToString(apiResponse.Result), out bool booleanResponse);

    //    return apiResponse.StatusCode == (int)HttpStatusCode.OK
    //        && booleanResponse;
    //}

    /// <summary>
    /// Get integer response of api
    /// </summary>
    //public int GetIntegerResponse(ApiResponseModel apiResponse)
    //{
    //    int.TryParse(Convert.ToString(apiResponse.Result), out int integerResponse);

    //    return apiResponse.StatusCode == (int)HttpStatusCode.OK
    //        ? integerResponse
    //        : 0;
    //}

    /// <summary>
    /// Get decimal response of api
    /// </summary>
    //public decimal GetDecimalResponse(ApiResponseModel apiResponse)
    //{
    //    decimal.TryParse(Convert.ToString(apiResponse.Result), out decimal decimalResponse);

    //    return apiResponse.StatusCode == (int)HttpStatusCode.OK
    //        ? decimalResponse
    //        : decimal.Zero;
    //}
}