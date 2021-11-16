using Business.Models;
using Business.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult GenerateActionResult<T>(this ControllerBase controllerBase, ServiceResult<T> serviceResult)
        {
            switch (serviceResult.ResultTypeId)
            {
                case ServiceResultType.Ok:
                    {
                        return controllerBase.Ok(serviceResult.Value);
                    }
                case ServiceResultType.Success:
                    {
                        return controllerBase.Ok();
                    }
                case ServiceResultType.NotFound:
                    {
                        return controllerBase.NotFound();
                    }
                case ServiceResultType.NoContent:
                    {
                        return controllerBase.NoContent();
                    }
                case ServiceResultType.BadRequest:
                    {
                        return controllerBase.BadRequest(serviceResult.Value);
                    }
                case ServiceResultType.AlreadyExists:
                    {
                        return controllerBase.BadRequest(serviceResult.Message);
                    }
            }

            return controllerBase.Problem(serviceResult.Message ?? serviceResult.Exception.StackTrace);
        }
    }
}
