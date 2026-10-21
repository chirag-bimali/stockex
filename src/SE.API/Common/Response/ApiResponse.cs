// src/PTS.API/Responses/ApiResponse.cs
using System.Text.Json.Serialization;
using SE.Application.Common.Response;

namespace SE.API.Common.Response;

public sealed class ApiResponse<T>
{
  public bool Success { get; init; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
  public T? Data { get; init; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
  public ApiError? Error { get; init; }

  public static ApiResponse<T> Ok(T data) => new() { Success = true, Data = data };

  public static ApiResponse<T> Fail(Error error) => new()
  {
    Success = false,
    Error = new ApiError(error.Code, error.Description, error.Details)
  };
}
