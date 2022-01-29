using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using react_Api.Database;
using react_Api.Database.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace react_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly DatabaseContext dbContext;

        public TagController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Tag>> Search([FromQuery] string name)
        {
            return await dbContext.Tags
                .Where(e => e.Name.Contains(name))
                .ToListAsync();
        }
    }
}
