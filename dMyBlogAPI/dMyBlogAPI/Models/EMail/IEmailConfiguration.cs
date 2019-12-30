using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Models.EMail
{
    public interface IEmailConfiguration
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }
        string SmtAlias { get; set; }

        bool UseSSL { get; set; }

        string PopServer { get; set; }
        int PopPort { get; set; }
        string PopUsername { get; set; }
        string PopPassword { get; set; }

    }
}