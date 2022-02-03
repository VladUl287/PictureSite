using Microsoft.AspNetCore.Mvc;
using react_Api.Database.Models;
using react_Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace react_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagService tagService;

        public TagController(TagService tagService)
        {
            this.tagService = tagService;
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Tag>> Search([FromQuery] string name, [FromQuery] int size = 10)
        {
            return await tagService.Search(name, size);
        }
    }
}