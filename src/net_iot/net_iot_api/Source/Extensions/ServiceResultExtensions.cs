using Microsoft.AspNetCore.Mvc;
using net_iot_core.Shared.ResultHandling;

namespace net_iot_api.Extensions
{
    public static class ServiceResultExtensions
    {
        public static ProblemDetails ToProblemDetails(this ServiceResult result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Can't convert success service result to problem details");
            }

            return new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Type = "https://datatracker.ietf.org/doc/html/rfc7807",
                Extensions = new Dictionary<string, object?>()
                {
                    { "errors", result.Errors },
                    { "isError", result.IsError }
                }
            };
        }
    }
}