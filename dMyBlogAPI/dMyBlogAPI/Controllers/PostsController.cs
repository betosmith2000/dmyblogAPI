using dMyBlogAPI.Helpers;
using dMyBlogAPI.Models;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace dMyBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_AllowOrigins")]

    public class PostsController : ControllerBase
    {

        private readonly PostService _postService;
        private readonly InteractionService _interactionService;

        public PostsController(PostService postService, InteractionService interactionService)
        {
            _postService = postService;
            this._interactionService = interactionService;
        }

        [HttpGet]
        public ActionResult<List<Post>> Get([FromQuery] PaginateParams p, string searchTerm)
        {
            Paginate<Post> res = new Paginate<Post>();
            res =_postService.Get(p, searchTerm);
            foreach (var item in res.Data)
            {
                item.interactions = _interactionService.Get(item.id, string.Empty);
            }
            //run whent change date type
            //foreach (var item in res.Data)
            //{
            //    _postService.Update(item.id, item);
            //}

            return Ok( res);
        }

        [HttpGet("{id:length(24)}", Name = "GetPost")]
        public ActionResult<Post> Get(string id)
        {
            var post = _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpPost]
        public ActionResult<Post> Create(Post post)
        {
            post.date = DateTime.Now.ToString("o", CultureInfo.InvariantCulture);
            _postService.Create(post);

            return CreatedAtRoute("GetPost", new { id = post.id.ToString() }, post);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Post postIn)
        {
            var book = _postService.Get(id);

            if (book == null)
            {
                this.Create(postIn);
                return NoContent();
            }

            _postService.Update(id, postIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var post = _postService.Get(id);

            if (post == null)
            {
                // return NotFound();
                return NoContent();
            }

            _postService.Remove(post.id);

            return NoContent();
        }
    }
}