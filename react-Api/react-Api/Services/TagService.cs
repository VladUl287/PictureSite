using Microsoft.EntityFrameworkCore;
using react_Api.Database;
using react_Api.Database.Models;
using react_Api.Services.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace react_Api.Services
{
    public class TagService : ITagService
    {
        private readonly DatabaseContext dbContext;

        public TagService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Tag>> Search(string name, int size)
        {
            return await dbContext.Tags
                .AsNoTracking()
                .Where(e => e.Name.Contains(name))
                .Take(size)
                .ToListAsync();
        }
    }
}