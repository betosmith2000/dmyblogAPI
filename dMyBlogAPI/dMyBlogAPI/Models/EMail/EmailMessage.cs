using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Models.EMail
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            To = new List<EmailAddress>();
        }

        public List<EmailAddress> To { get; set; }
        public EmailAddress From { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}