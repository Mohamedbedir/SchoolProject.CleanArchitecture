using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Departments.Queries.Models;
using SchoolProject.Core.Features.Departments.Queries.Responses;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Response;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.API.Controllers
{
    [ApiController]

    public class DepartmentController : AppControllerBase
    {
        [HttpGet(Router.DepartmentRouting.ById)]
        [ProducesResponseType(typeof(Response<GetDepartmentByIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFound<GetDepartmentByIdResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<GetDepartmentByIdResponse>>> GetDepartmentById([FromRoute] int id)
        {
            var response = await mediator.Send(new GetDepartmentByIdQuery(id));
            return NewResult(response);
        }
    }
}
