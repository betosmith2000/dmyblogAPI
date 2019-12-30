using dMyBlogAPI.Helpers;
using dMyBlogAPI.Models;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dMyBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_AllowOrigins")]

    public class CommentsController : ControllerBase
    {

        private readonly CommentService _commentService;


        public CommentsController(CommentService commentService)
        {
            _commentService = commentService;

        }

        [HttpGet]
        public ActionResult<List<Comment>> Get([FromQuery] PaginateParams p, string postId)
        {
            Paginate<Comment> res = new Paginate<Comment>();
            res = _commentService.Get(p, postId);



            return Ok(res);
        }

        [HttpGet("{id:length(24)}", Name = "GetComment")]
        public ActionResult<Comment> Get(string id)
        {
            var comment = _commentService.Get(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        [HttpPost]
        public ActionResult<Comment> Create(Comment comment)
        {
            if (string.IsNullOrEmpty(comment.rootId))
            {
                comment.date = DateTime.UtcNow;
                _commentService.Create(comment);

                return CreatedAtRoute("GetComment", new { id = comment.id.ToString() }, comment);
            }
            else
            {
                var _root = _commentService.Get(comment.rootId);

                if (comment.rootId == comment.parentId)
                {
                    if (_root == null)
                        return NotFound("Previous comment removed!");
                    if (_root.Comments == null)
                        _root.Comments = new List<Comment>();
                    comment.id = ObjectId.GenerateNewId().ToString();
                    _root.Comments.Add(comment);
                    _commentService.Update(_root.id, _root);
                    return CreatedAtRoute("GetComment", new { id = comment.id.ToString() }, comment);
                }
                else
                {

                    var _parentComment = SearchToInsertComment(_root.Comments, comment.parentId);
                    if (_parentComment == null)
                        return NotFound("Previous comment removed!");

                    if (_parentComment.Comments == null)
                        _parentComment.Comments = new List<Comment>();
                    comment.id = ObjectId.GenerateNewId().ToString();
                    _parentComment.Comments.Add(comment);
                    _commentService.Update(_root.id, _root);
                    return CreatedAtRoute("GetComment", new { id = comment.id.ToString() }, comment);

                }
            }
        }

        private Comment SearchToInsertComment(ICollection<Comment> comments, string id)
        {
            var comment = comments.Where(c => c.id == id).FirstOrDefault();
            if (comment == null || string.IsNullOrEmpty(comment.id))
            {

                foreach (var item in comments)
                {
                    var res = SearchToInsertComment(item.Comments, id);
                    if (res != null && !string.IsNullOrEmpty(res.id))
                    {
                        comment = res;
                        break;
                    }
                }
            }

            return comment;
        }
        private bool SearchToDeleteComment(ICollection<Comment> comments, string id)
        {
            bool res = false;
            var comment = comments.Where(c => c.id == id).FirstOrDefault();
            if (comment == null || string.IsNullOrEmpty(comment.id))
            {

                foreach (var item in comments)
                {
                    res = SearchToDeleteComment(item.Comments, id);
                    if (res)
                    {
                        break;
                    }
                }
            }
            else
            {
                if (comment.Comments != null && comment.Comments.Count > 0)
                    return false;
                comments.Remove(comment);
                res = true;
            }

            return res;
        }


        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Comment commentIn)
        {
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id, string rootId)
        {

            if (string.IsNullOrEmpty(rootId) || rootId == "null" || rootId == "undefined")
            {
                var comment = _commentService.Get(id);
                if (comment == null)
                {
                    return NotFound();
                }
                if (comment.Comments != null && comment.Comments.Count > 0) {
                    return NotFound();
                }
                _commentService.Remove(comment.id);
                return NoContent();
            }
            else
            {
                var _root = _commentService.Get(rootId);

                bool removed = SearchToDeleteComment(_root.Comments, id);
                if (!removed)
                {
                    return NotFound();
                }
                _commentService.Update(_root.id, _root);
                return NoContent();

            }
        }
    }
}