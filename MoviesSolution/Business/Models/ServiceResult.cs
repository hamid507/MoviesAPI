using Business.Models.Enums;
using System;

namespace Business.Models
{
    public class ServiceResult<T>
    {
        public ServiceResultType ResultTypeId { get; private set; }
        public string Message { get; private set; }
        public T Value { get; private set; }
        public Exception Exception { get; private set; }

        public static ServiceResult<T> Ok(T value, string message = null)
        {
            return new ServiceResult<T>()
            {
                ResultTypeId = ServiceResultType.Ok,
                Value = value,
                Message = message
            };
        }

        public static ServiceResult<T> BadRequest(T value, string message = null)
        {
            return new ServiceResult<T>()
            {
                ResultTypeId = ServiceResultType.BadRequest,
                Value = value,
                Message = message
            };
        }

        public static ServiceResult<T> NoContent(string message = null)
        {
            return new ServiceResult<T>()
            {
                ResultTypeId = ServiceResultType.NoContent,
                Message = message
            };
        }

        public static ServiceResult<T> NotFound(string message = null)
        {
            return new ServiceResult<T>()
            {
                ResultTypeId = ServiceResultType.NotFound,
                Message = message
            };
        }

        public static ServiceResult<T> AlreadyExists(string message = null)
        {
            return new ServiceResult<T>()
            {
                ResultTypeId = ServiceResultType.AlreadyExists,
                Message = message
            };
        }

        public static ServiceResult<T> Error(Exception exception)
        {
            return new ServiceResult<T>()
            {
                ResultTypeId = ServiceResultType.Error,
                Exception = exception
            };
        }

        public static ServiceResult<T> Error(string message)
        {
            return new ServiceResult<T>()
            {
                ResultTypeId = ServiceResultType.Error,
                Message = message
            };
        }

        public static ServiceResult<T> Success()
        {
            return new ServiceResult<T>()
            {
                ResultTypeId = ServiceResultType.Success
            };
        }
    }
}
