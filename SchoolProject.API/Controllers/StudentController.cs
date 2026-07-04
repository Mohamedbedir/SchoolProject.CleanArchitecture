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
using SchoolProject.Core.Pagination;
using SchoolProject.Data.AppMetaData;
using SchoolProject.Data.Entities;

namespace SchoolProject.API.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [ApiController]
    public class StudentController : AppControllerBase
    {
        //private readonly IMediator mediator;

        //public StudentController(IMediator mediator)
        //{
        //    this.mediator = mediator;
        //}
        [HttpGet(Router.StudentRouting.List)]
        [ProducesResponseType(typeof(Response<IReadOnlyList<GetStudentsResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<IReadOnlyList<GetStudentsResponse>>>> GetAllStudent()
        {
            var response=await mediator.Send(new GetStudentsQuery());
            return NewResult(response);    
        }
        [HttpGet(Router.StudentRouting.Paginated)]
        [ProducesResponseType(typeof(PaginatedResult<GetStudentsResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<IReadOnlyList<GetStudentsResponse>>>>
            GetAllStudentWithPagination([FromQuery] GetStudentsPaginatedQuery pagination)
        {
            var response=await mediator.Send(pagination);
            return Ok(response);    
        }
        [HttpGet(Router.StudentRouting.ById)]
        [ProducesResponseType(typeof(Response<GetStudentByIdResponse>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFound<GetStudentByIdResponse>),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<GetStudentByIdResponse>>> GetStudentById([FromRoute] int id)
        {
            var response=await mediator.Send(new GetStudentByIdQuery(id));
            return NewResult(response);    
        }
        [HttpPost(Router.StudentRouting.Create)]
        [ProducesResponseType(typeof(Response<string>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>),StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Response<string>),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> CreateStudent([FromBody] AddStudentCommand model)
        {
            var response=await mediator.Send(model);
            return NewResult(response);    
        }
        [HttpPut(Router.StudentRouting.Update)]
        [ProducesResponseType(typeof(Response<string>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> UpdateStudent([FromBody] EditStudentCommand model)
        {
            var response=await mediator.Send(model);
            return NewResult(response);    
        }
        [HttpDelete(Router.StudentRouting.Delete)]
        [ProducesResponseType(typeof(Response<string>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<string>>> DeleteStudent([FromRoute] int id)
        {
            var response=await mediator.Send(new DeleteStudentCommand(id));
            return NewResult(response);    
        }
    }
}
