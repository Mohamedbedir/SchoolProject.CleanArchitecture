using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Responses;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Response;
using SchoolProject.Core.Pagination;
using SchoolProject.Data.AppMetaData;
using System.Security.Claims;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    //[Authorize(Roles ="Admin")]
    public class UserController : AppControllerBase
    {
        [HttpGet(Router.UserRouting.Paginated)]
        [ProducesResponseType(typeof(PaginatedResult<GetUsersPaginatedResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResult<GetStudentsResponse>>>
            GetUsersWithPagination([FromQuery] GetUsersPaginatedQuery pagination)
        {
            var response = await mediator.Send(pagination);
            return Ok(response);
        }
        [HttpGet(Router.UserRouting.ById)]
        [ProducesResponseType(typeof(Response<GetUserByIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFound<GetUserByIdResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<GetStudentByIdResponse>>> GetUserById([FromRoute] string id)
        {
            var response = await mediator.Send(new GetUserByIdQuery(id));
            return NewResult(response);
        }
        [HttpGet(Router.UserRouting.CurrentUser)]
        [ProducesResponseType(typeof(Response<GetCurrentUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest<GetCurrentUserResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<GetCurrentUserResponse>>> GetCurrentUser()
        {
            var response = await mediator.Send(new GetCurrentUserQuery());
            return NewResult(response);
        }
        [HttpPost(Router.UserRouting.Create)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> CreateUser([FromBody] AddAppUserCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpPut(Router.UserRouting.Update)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> UpdateUser([FromBody] EditAppUserCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpPut(Router.UserRouting.ChangePassword)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> ChangePassword([FromBody] ChangeUserPasswordCommand model)
        {
                      
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpPut(Router.UserRouting.LockUser)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> LockUser([FromRoute] string id)
        {
                      
            var response = await mediator.Send(new LockUserCommand(id));
            return NewResult(response);
        }
        [HttpPut(Router.UserRouting.UnLockUser)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> UnLockUser([FromRoute] string id)
        {
                      
            var response = await mediator.Send(new UnLockUserCommand(id));
            return NewResult(response);
        }
       


        [HttpDelete(Router.UserRouting.Delete)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> DeleteUser([FromRoute] string id)
        {
            var response = await mediator.Send(new DeleteAppUserCommand(id));
            return NewResult(response);
        }
    }
}
