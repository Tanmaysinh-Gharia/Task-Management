

using ApiResponseModel = TaskManagement.Core.Common.ResponseHandler.ApiResponse;
namespace TaskManagement.Services;

public interface IResponseHandler
{
    T GetResponse<T>(ApiResponseModel apiResponse);
    string GetStringResponse(ApiResponseModel apiResponse);
}