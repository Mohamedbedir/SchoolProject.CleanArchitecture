using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Helpers
{
    public class MailSettings
    {
        public string Host { get; set; } = default!;

        public int Port { get; set; }

        public string Email { get; set; } = default!;

        public string DisplayName { get; set; } = default!;

        public string Password { get; set; } = default!;

        public bool EnableSSL { get; set; }
    }
}
