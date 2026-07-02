using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Queries.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Responses;
using SchoolProject.Core.Localization;
using SchoolProject.Core.Pagination;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationUser.Queries.Handlers
{
    public class AppUsersQueryHandler: ResponseHandler ,
        IRequestHandler<GetUserByIdQuery,Response<GetUserByIdResponse>>,
        IRequestHandler<GetUsersPaginatedQuery,PaginatedResult<GetUsersPaginatedResponse>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public AppUsersQueryHandler(IStringLocalizer<SharedResources> localizer
            ,IMapper mapper
            ,UserManager<AppUser> userManager):base(localizer)
        {
            this.localizer = localizer;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user= await userManager.FindByIdAsync(request.Id);
            if (user == null)
                return NotFound<GetUserByIdResponse>();
            var UserMapping = mapper.Map<GetUserByIdResponse>(user);
            return Success(UserMapping);
        }

        public Task<PaginatedResult<GetUsersPaginatedResponse>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
        {
            var Users= userManager.Users.AsQueryable();
            if (request.FullName != null)
                Users=Users.Where(u => u.FullName.Contains(request.FullName));
            var PagenatedUsers = Users.Select(u => new GetUsersPaginatedResponse()
            {
                Email = u.Email,
                UserName = u.UserName,
                FullName = u.FullName,
                Phone=u.PhoneNumber,
                Address = u.Address,
                Country = u.Country,
            }).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return PagenatedUsers;
        }
    }
}
