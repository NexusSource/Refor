using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NodaTime;
using Refor.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refor.Services
{
    public interface ITextStoreService
    {
        ValueTask<bool> Delete(StoredText storedText);
        ValueTask<StoredText?> Get(string id);
        IAsyncEnumerable<StoredText> Get(int page);
        ValueTask<StoredText> Upload(string text);
    }

    public class TextStoreService : ITextStoreService
    {
        private readonly IClock _clock;
        private readonly ILogger _logger;
        private readonly ReforContext _reforContext;
        private readonly IRandomStringService _randomStringService;

        public TextStoreService(IClock clock, ILogger<TextStoreService> logger, ReforContext reforContext, IRandomStringService randomStringService)
        {
            _clock = clock;
            _logger = logger;
            _reforContext = reforContext;
            _randomStringService = randomStringService;
        }

        public async ValueTask<bool> Delete(StoredText storedText)
        {
            _logger.LogInformation("Attempting to delete stored text with the ID '{ID}'.", storedText.ID);
            StoredText? internalFetch = await _reforContext.Texts.FindAsync(storedText.ID);
            if (internalFetch is null)
            {
                _logger.LogWarning("We tried to delete a stored text which doesn't exist in the database.");
                return false;
            }
            _logger.LogInformation("Found the stored text we want to delete. Removing it from the database.");
            _reforContext.Texts.Remove(internalFetch);
            await _reforContext.SaveChangesAsync();
            return true;
        }

        public ValueTask<StoredText?> Get(string id)
        {
            return _reforContext.Texts.FindAsync(id)!;
        }

        public IAsyncEnumerable<StoredText> Get(int page)
        {
            return _reforContext.Texts.Skip(10 * page).Take(10).AsAsyncEnumerable();
        }

        public async ValueTask<StoredText> Upload(string text)
        {
            _logger.LogInformation("Upload a new stored text.");
            _logger.LogInformation("Looking for a new unique key.");

            string? id = null;
            while (id is null)
            {
                // We retry until we have ID. The RandomStringService will very rarely give a duplicate.
                // but, there's actually no true guarantee.
                string potentialID = _randomStringService.GetRandomString();
                StoredText? potentialDuplicateIDText = await Get(potentialID);
                if (potentialDuplicateIDText is null)
                    id = potentialID;
            }
            _logger.LogInformation("We have acquired an ID '{ID}'.", id);

            StoredText storedText = new()
            {
                ID = id,
                Text = text,
                Uploaded = _clock.GetCurrentInstant()
            };

            _reforContext.Texts.Add(storedText);
            _logger.LogInformation("Saving new stored text to the database.");
            await _reforContext.SaveChangesAsync();
            return storedText;
        }
    }
}
