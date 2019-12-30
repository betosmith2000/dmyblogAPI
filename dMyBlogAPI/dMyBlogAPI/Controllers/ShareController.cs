using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using dMyBlogAPI.Helpers;
using dMyBlogAPI.Models;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dMyBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_AllowOrigins")]
    public class ShareController : ControllerBase
    {
        private readonly SharesSevice shareService;

        public ShareController(SharesSevice shareService)
        {
            this.shareService = shareService;
        }


        [HttpGet]
        public ActionResult<ApiResult> GetAll(string username)
        {
            ApiResult apiResult = new ApiResult();
            return Ok(this.shareService.GetAll(username));
        }

        [HttpGet("{id}", Name = "GetShare")]
        public ActionResult<ShareModel> Get(string id)
        {
            ShareModel shareModel = this.shareService.Get(id);

            if (shareModel == null)
            {
                return NotFound();
            }

            return shareModel;
        }

        [HttpPost]
        public ActionResult Create(ShareModel share)
        {
            share.date = DateTime.Now.ToString("o", CultureInfo.InvariantCulture);
            this.shareService.Create(share);
            return CreatedAtRoute("GetShare", new { id = share.id }, share);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ShareModel shareIn)
        {
            if (this.shareService.Get(id) != null)
                return NotFound();
            this.Create(shareIn);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            
            if (string.IsNullOrEmpty(id))
                return NotFound();
            this.shareService.Remove(id);
            return NoContent();
        }


    }
}