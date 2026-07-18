using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationRoles.Commands.Models;
using SchoolProject.Core.Features.Emails.Commands.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    public class EmailController : AppControllerBase
    {
        [HttpPost(Router.EmailRouting.SendEmail)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> SendEmail([FromForm] SendEmailCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
    }
}
