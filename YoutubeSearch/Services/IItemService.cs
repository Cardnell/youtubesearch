using System.Collections.Generic;
using System.Threading.Tasks;
using YoutubeSearch.Models;

namespace YoutubeSearch.Services
{
    public interface IItemService
    {
        Task<ItemSet> GetItems(string filter);
        Task<ItemSet> GetItems(string filter, string pageToken);
    }
}