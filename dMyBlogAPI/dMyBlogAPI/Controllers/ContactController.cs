using dMyBlogAPI.Models;
using dMyBlogAPI.Models.EMail;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace dMyBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_AllowOrigins")]
    public class ContactController: ControllerBase
    {
        private readonly IEmailService _email;

        public ContactController(IEmailService email)
        {
            this._email = email;
        }

      


        [HttpPost]
        public ActionResult<EmailData> Create(EmailData data)
        {
            //Enviar correo con un link para cambiar contraseña
            
            EmailMessage message = new EmailMessage();
            message.Content =$"<div class='margin:10px'>Name: {data.name}<br/>Address: {data.email}</div><br/><br/>Message:<br/>" + data.message;
            message.Subject = "dMy Blog Contact!";
            message.To.Add(new EmailAddress { Name = "dMy Blog", Address = "dmyblog@outlook.com"});
            string error = string.Empty;
            try
            {
                _email.Send(message);
            }
            catch (System.Exception ex)
            {
                error = ex.InnerException.ToString();
                System.Diagnostics.Trace.TraceError(ex.InnerException.ToString());
            }


            return Ok(error);

        }

       
       
    }
}
