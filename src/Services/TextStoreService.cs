using Refor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refor.Services
{
    public interface ITextStoreService
    {
        Task<StoredText> Get(string id);
        Task<StoredText> Upload(string text);
        Task<bool> Delete(StoredText storedText);
        IAsyncEnumerable<StoredText> Get(int page);
    }

    public class TextStoreService : ITextStoreService
    {
        public Task<bool> Delete(StoredText storedText)
        {
            throw new System.NotImplementedException();
        }

        public Task<StoredText> Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public IAsyncEnumerable<StoredText> Get(int page)
        {
            throw new System.NotImplementedException();
        }

        public Task<StoredText> Upload(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
