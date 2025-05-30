using Framework.Model;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.BaseDTOs;
using System.Net;

namespace API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        public int UserId
        {
            get
            {
                var userId = HttpContext.Items["UserId"];

                if (userId == null)
                    throw new UnauthorizedAccessException("User is not authorized.");

                return (int)userId;
            }
        }

        protected IActionResult GenerateResponse<T>(BaseResponse<T> response)
        {
            HttpStatusCode statusCode;
            switch (response.Code)
            {
                case "400":
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case "404":
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case "409":
                    statusCode = HttpStatusCode.Conflict;
                    break;
                case "401":
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                case "403":
                    statusCode = HttpStatusCode.Forbidden;
                    break;
                case "500":
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
                default:
                    statusCode = HttpStatusCode.OK;
                    break;
            }

            return StatusCode((int)statusCode, response);
        }

        protected IActionResult GenerateBaseResponse(string code, string message)
        {
            return GenerateResponse(new BaseResponse<object>(false, code, message, null));
        }
    }
}
