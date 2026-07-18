using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Accounts.Commands.Models;
using SchoolProject.Core.Features.Accounts.Queries.Models;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Data.AppMetaData;
using SchoolProject.Data.Helpers;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    public class AccountController : AppControllerBase
    {
        [HttpPost(Router.AccountRouting.SignIn)]
        [ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<JwtTokenResponse>>> SiginIn([FromForm] SignInCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpPost(Router.AccountRouting.RefreshToken)]
        [ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status422UnprocessableEntity)]
        //[ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<JwtTokenResponse>>> RefreshToken([FromBody] RefreshTokenCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpGet(Router.AccountRouting.ValidateToken)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> ValidateToken([FromQuery] GetValidatorQuery model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpGet(Router.AccountRouting.ConfirmEmail)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> ConfirmEmail([FromQuery] ConfirmEmailQuery model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpPost(Router.AccountRouting.SendResetPasswordCode)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> SendResetPasswordCode([FromQuery] SendResetPasswordCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpGet(Router.AccountRouting.ConfirmResetPassword)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> ConfirmResetPassword([FromQuery] ConfirmResetPasswordQuery model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpPost(Router.AccountRouting.ResetPassword)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> ResetPassword([FromForm] ResetPasswordCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }

        [HttpGet(Router.AccountRouting.SendResetPasswordLink)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> SendResetPasswordLink([FromQuery] SendResetPasswordLinkQuery model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpPost(Router.AccountRouting.ResetPasswordLink)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> ResetPasswordLink([FromQuery] ResetPasswordLinkCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
    }
}
