

using ApiResponseModel = TaskManagement.Core.Common.ResponseHandler.ApiResponse;
namespace TaskManagement.Services;

public interface IResponseHandler
{
    T GetResponse<T>(ApiResponseModel apiResponse);
    string GetStringResponse(ApiResponseModel apiResponse);
    //bool GetBooleanResponse(ApiResponseModel apiResponse);
    //int GetIntegerResponse(ApiResponseModel apiResponse);
    //decimal GetDecimalResponse(ApiResponseModel apiResponse);
}