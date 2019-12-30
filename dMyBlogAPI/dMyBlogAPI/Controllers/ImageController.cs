using dMyBlogAPI.Helpers;
using dMyBlogAPI.Models;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dMyBlogAPI.Controllers
{
    [Route("api/PostImage")]
    [ApiController]
    [EnableCors("_AllowOrigins")]
    public class ImageController : Controller
    {
        private readonly PostService _postService;
        private readonly IConfiguration _config;

        public ImageController(PostService postService, IConfiguration config)
        {
            _postService = postService;
            this._config = config;
        }


        [Route("{id}")]
        public IActionResult Index(string id)
        {
            byte[] imageContent = null;
            if (id.Length == 24)
            {
                Post post = _postService.Get(id);
                
                string hubUrl = ReadBlockstackProfileGaiaHubUrl(post.author);
                if (!string.IsNullOrEmpty(hubUrl))
                {
                    string fullUrl = hubUrl + $"{post.imageFileName}";
                    imageContent = GetImageFromGaia(fullUrl);
                }
              
            }
            return File(imageContent, "image/jpeg","post.jpg");
        }

        private byte[] GetImageFromGaia(string urlImage) {
            byte[] res = null;
            
            WebRequest request = WebRequest.Create(urlImage);
            request.Method = "GET";
            request.ContentType = "text/plain";
            try
            {
                WebResponse wr = request.GetResponse();
                Stream receiveStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                string content = reader.ReadToEnd();


                var base64Data = Regex.Match(content, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                res = Convert.FromBase64String(base64Data);

                



                
            }
            catch
            {


            }
            return res;
        }

        private string ReadBlockstackProfileGaiaHubUrl(string id)
        {
            string url = _config.GetValue<string>("blockstackUrl") + $"/{id}";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/plain";
            string res = string.Empty;
            try
            {

                WebResponse wr = request.GetResponse();
                Stream receiveStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                string content = reader.ReadToEnd();


                Dictionary<string, blockstackInfo> json = JsonConvert.DeserializeObject<Dictionary<string, blockstackInfo>>(content);
                if (json.Count() > 0)
                {
                    KeyValuePair<string, string> hubUrl = json.FirstOrDefault().Value.profile.apps.Where(e => e.Key == "https://www.dmyblog.co").FirstOrDefault();
                    res = hubUrl.Value;
                }
            }
            catch
            {


            }
            return res;
        }
    }
}
