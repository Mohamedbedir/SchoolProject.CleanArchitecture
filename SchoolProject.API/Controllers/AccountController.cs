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
        [ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status422UnprocessableEntity)]
        //[ProducesResponseType(typeof(Response<JwtTokenResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<JwtTokenResponse>>> ValidateToken([FromBody] GetValidatorQuery model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
    }
}
