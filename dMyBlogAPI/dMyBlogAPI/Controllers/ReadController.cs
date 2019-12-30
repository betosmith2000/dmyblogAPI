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
    [Route("Read")]
    [ApiController]
    [EnableCors("_AllowOrigins")]
    public class ReadController : Controller
    {
        private readonly PostService _postService;

        public ReadController(PostService postService)
        {
            _postService = postService;
        }



        [Route("{id}")]
        public IActionResult Index(string id)
        {

            if (id.Length == 24)
            {
                // string url =string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
                Post post = _postService.Get(id);
                var appUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/read/{post.id}";
                var redirect = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/#/read/{post.author}/{post.id}";
                ViewBag.Title = post.title;
                ViewBag.Url = appUrl;
                ViewBag.Redirect = redirect;
                ViewBag.Excerpt = post.excerpt;
                ViewBag.Author = post.author;
                if (!string.IsNullOrEmpty(post.imageFileName))
                    ViewBag.ImageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/api/PostImage/{post.id}";
                else
                    ViewBag.ImageUrl = "https://www.dmyblog.co/assets/logo.png";
            }
            return View();
        }

    }
}
