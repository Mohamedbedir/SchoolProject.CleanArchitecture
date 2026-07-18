using MailKit;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Emails.Commands.Models;
using SchoolProject.Core.Features.Emails.Commands.Validators;
using SchoolProject.Core.Localization;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Emails.Commands.Handlers
{
    public class EmailsCommandHandler:ResponseHandler,
        IRequestHandler<SendEmailCommand,Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly IEmailService emailService;

        public EmailsCommandHandler(IStringLocalizer<SharedResources> localizer,
            IEmailService emailService):base(localizer) 
        {
            this.localizer = localizer;
            this.emailService = emailService;
        }

        public async Task<Response<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var Email = await emailService.SendEmailAsync(request.Email, request.Massage,null);
            if (Email == "EmailSentSuccessfully")
                return Success<string>("", Message: "Email Sent Successfully");
            return BadRequest<string>("Failed To Email Sent");
        }
    }
}
