using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Response;
using SchoolProject.Core.Features.Subjects.Command.Models;
using SchoolProject.Core.Features.Subjects.Query.Models;
using SchoolProject.Core.Features.Subjects.Query.Response;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.API.Controllers
{
    [Authorize(Roles = "User")]
    //[Route("api/[controller]")]
    [ApiController]
    public class SubjectController : AppControllerBase
    {
        //private readonly IMediator mediator;

        //public SubjectController(IMediator mediator)
        //{
        //    this.mediator = mediator;
        //}

        [HttpGet(Router.SubjectRouting.List)]
        [ProducesResponseType(typeof(Response<IReadOnlyList<GetSubjectsResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<IReadOnlyList<GetSubjectsResponse>>>> GetAllSubject()
        {
            var response = await mediator.Send(new GetSubjectsQuery());
            return Ok(response);
        }
        [HttpGet(Router.SubjectRouting.ById)]
        [ProducesResponseType(typeof(Response<GetSubjectByIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFound<GetSubjectByIdResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<GetSubjectByIdResponse>>> GetSubjectById([FromRoute] int id)
        {
            var response = await mediator.Send(new GetSubjectByIdQuery(id));
            return NewResult(response);
        }
        [HttpPost(Router.SubjectRouting.Create)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> CreateSubject([FromBody] AddSubjectCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpPut(Router.SubjectRouting.Update)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> UpdateSubject([FromBody] UpdateSubjectCommand model)
        {
            var response = await mediator.Send(model);
            return NewResult(response);
        }
        [HttpDelete(Router.SubjectRouting.Delete)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> DeleteSubject([FromRoute] int id)
        {
            var response = await mediator.Send(new DeleteSubjectCommand(id));
            return NewResult(response);
        }
    }
}
