using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using dMyBlogAPI.Helpers;
using dMyBlogAPI.Models;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dMyBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssController : ControllerBase
    {
        private readonly PostService _postService;

        public RssController(PostService postService)
        {
            _postService = postService;

        }

        [ResponseCache(Duration = 1200)]
        [HttpGet]
        public ActionResult GetAll()
        {
            var feed = new SyndicationFeed("dMy Blog", "Simple decentralized blog!",new Uri("https://www.dmyblog.co"), "www.dmyblog.co", DateTime.Now);
            feed.Copyright = new TextSyndicationContent($"{DateTime.Now.Year} dMy Blog");


            var items = new List<SyndicationItem>();

            Paginate<Post> res = new Paginate<Post>();
            PaginateParams p = new PaginateParams();
            p.PageSize = 500;
            res = _postService.Get(p, "");
            foreach (var item in res.Data)
            {
                var postUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/#/read/{item.author}/{item.id}";
                var title = item.title;
                var description = item.excerpt;
                DateTime date = string.IsNullOrEmpty(item.date) ? new DateTime(2019, 5, 25) : Convert.ToDateTime(item.date);
                items.Add(new SyndicationItem(title, description, new Uri(postUrl), item.id, date));
            }

            feed.Items = items;


            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true
            };
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, settings))
                {
                    var rssFormatter = new Rss20FeedFormatter(feed, false);
                    rssFormatter.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
            }


        }


        [ResponseCache(Duration = 1200)]
        [HttpGet("{author}", Name = "GetAuthor")]
        public ActionResult GetAuthor(string author)
        {
            var feed = new SyndicationFeed($"dMy Blog | {author}", "Simple decentralized blog!", new Uri("https://www.dmyblog.co"), "www.dmyblog.co", DateTime.Now);
            feed.Copyright = new TextSyndicationContent($"{DateTime.Now.Year} dMy Blog");


            var items = new List<SyndicationItem>();

            Paginate<Post> res = new Paginate<Post>();
            PaginateParams p = new PaginateParams();
            p.PageSize = 500;
            res = _postService.Get(p, author);
            foreach (var item in res.Data)
            {
                var postUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/#/read/{item.author}/{item.id}";
                var title = item.title;
                var description = item.excerpt;
                DateTime date = string.IsNullOrEmpty(item.date) ? new DateTime(2019, 5, 25) : Convert.ToDateTime(item.date);
                items.Add(new SyndicationItem(title, description, new Uri(postUrl), item.id, date));
            }

            feed.Items = items;


            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true
            };
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, settings))
                {
                    var rssFormatter = new Rss20FeedFormatter(feed, false);
                    rssFormatter.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
            }


        }
    }
}