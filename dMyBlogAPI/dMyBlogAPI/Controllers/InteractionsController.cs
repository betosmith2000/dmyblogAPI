using dMyBlogAPI.Helpers;
using dMyBlogAPI.Models;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace dMyBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_AllowOrigins")]
    public class InteractionsController : ControllerBase
    {

        private readonly InteractionService _interactionService;

        public InteractionsController(InteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        [HttpGet]
        public ActionResult<InteractionTypeResult> Get(string postId, string userId)
        {
            InteractionTypeResult res;
            res = _interactionService.Get(postId,userId);

            return Ok(res);
        }

        [HttpGet("{id:length(24)}", Name = "GetInteraction")]
        public ActionResult<Interaction> Get(string id)
        {
            var interaction = _interactionService.Get(id);

            if (interaction == null)
            {
                return NotFound();
            }

            return interaction;
        }

        [HttpPost]
        public ActionResult<Interaction> Create(Interaction interaction)
        {
            _interactionService.Create(interaction);

            return CreatedAtRoute("GetInteraction", new { id = interaction.id.ToString() }, interaction);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Interaction interactionIn)
        {
            var book = _interactionService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _interactionService.Update(id, interactionIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var interaction = _interactionService.Get(id);

            if (interaction == null)
            {
                // return NotFound();
                return NoContent();
            }

            _interactionService.Remove(interaction.id);

            return NoContent();
        }
    }
}