using Microsoft.AspNetCore.Mvc;
using react_Api.Database.Models;
using react_Api.Services.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace react_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService tagService;

        public TagController(ITagService tagService)
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