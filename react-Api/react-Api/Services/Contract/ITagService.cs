using react_Api.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace react_Api.Services.Contract
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> Search(string name, int size);
    }
}