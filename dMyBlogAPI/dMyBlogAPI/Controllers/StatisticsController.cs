using dMyBlogAPI.Helpers;
using dMyBlogAPI.Models;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_AllowOrigins")]
    public class StatisticsController: ControllerBase
    {
        private readonly PostService postService;

        public StatisticsController(PostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            PostStatistics res =  postService.GetStatistics();
            return Ok(res);
        }

    }
}
