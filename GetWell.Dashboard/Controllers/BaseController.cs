using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.Controllers
{
    [Authorize]
	public class BaseController : Controller
	{
        protected static async Task<ContentResult> ContentResultAsync(CrudResponse message)
        {
            if (message.MessageKey == Crud.Success)
                return await SuccessContentResultAsync();

            return await Task.FromResult(new ContentResult
            {
                StatusCode = StatusCodes.Status400BadRequest, 
                Content = message.Message, 
                ContentType = "text/html"
            });
        }

        protected static async Task<ContentResult> ContentResultAsync(Crud message)
        {
            return await ContentResultAsync(new CrudResponse(message));
        }

        protected static async Task<ContentResult> SuccessContentResultAsync(string content = "Success", string contentType = "text/html")
        {
            return await Task.FromResult(new ContentResult
            {
                StatusCode = StatusCodes.Status200OK,
                Content = content, 
                ContentType = contentType
            });
        }
    }
}