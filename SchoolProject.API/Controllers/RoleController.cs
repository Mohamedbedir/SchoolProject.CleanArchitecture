using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationRoles.Commands.Models;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Models;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Responses;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Responses;
using SchoolProject.Core.Features.Students.Queries.Response;
using SchoolProject.Core.Pagination;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.API.Controllers
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class RoleController : AppControllerBase
    {

        [HttpGet(Router.RoleRouting.List)]
        [ProducesResponseType(typeof(Response<IReadOnlyList<GetRolesListResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<IReadOnlyList<GetRolesListResponse>>>>GetRolesList()
        {
            var response = await mediator.Send(new GetRolesListQuery());
            return Ok(response);
        }
        [HttpGet(Router.RoleRouting.ById)]
        [ProducesResponseType(typeof(Response<GetRoleByIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFound<GetRoleByIdResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<GetRoleByIdResponse>>> GetUserById([FromRoute] string id)
        {
            var response = await mediator.Send(new GetRoleByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.RoleRouting.Create)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> CreateStudent([FromForm] AddRoleCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpPut(Router.RoleRouting.Update)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> UpdateUser([FromForm] EditRoleCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }

        [HttpDelete(Router.RoleRouting.Delete)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> DeleteUser([FromRoute] string id)
        {
            var response = await mediator.Send(new DeleteRoleCommand(id));
            return NewResult(response);
        }
        [HttpGet(Router.RoleRouting.ManageUserClaims)]
        [ProducesResponseType(typeof(Response<ManageUserClaimsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFound<ManageUserClaimsResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<GetRoleByIdResponse>>> ManageUserClaims([FromRoute] string id)
        {
            var response = await mediator.Send(new ManageUserClaimsQuery(id));
            return NewResult(response);
        }
        [HttpPut(Router.RoleRouting.UpdateUserClaims)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFound<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotFound<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> UpdateUserClaims([FromBody] UpdateUserClaimsCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}
