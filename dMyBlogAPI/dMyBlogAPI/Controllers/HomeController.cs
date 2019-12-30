using dMyBlogAPI.Models;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Controllers
{
    [Route("")]
    public class HomeController:Controller
    {

        private readonly PostService _postService;

        public HomeController(PostService postService)
        {
            _postService = postService;
        }
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Title = "dMy Blog";
            ViewBag.Url = "https://www.dmyblog.co";
            ViewBag.Excerpt = "Simple decentralized blog!";
            ViewBag.Author = "";
            return View();
        }


        //[Route("{id}")]
        //public IActionResult Index(string id)
        //{

        //    if (id.Length == 24)
        //    {
        //        // string url =string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
        //        Post post = _postService.Get(id);
        //        var appUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/#/read/{post.author}/{post.shareCode}";
        //        ViewBag.Title = post.title;
        //        ViewBag.Url = appUrl;
        //        ViewBag.Excerpt = post.excerpt;
        //        ViewBag.Author = post.author;
        //    }
        //    return View();
        //}

      
    }
}
