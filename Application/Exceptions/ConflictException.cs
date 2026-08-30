using Microsoft.AspNetCore.Http;

namespace Application.Exceptions
{
    public class ConflictException : ApiException
    {
        public ConflictException(string message, List<string>? errors = null) 
            : base(message, StatusCodes.Status409Conflict, errors) { }
    }
}
