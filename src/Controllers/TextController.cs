using Microsoft.AspNetCore.Mvc;
using Refor.Models;
using Refor.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refor.Controllers
{
    [Route("api/text")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly ITextStoreService _textStoreService;
        private static readonly NotFoundObjectResult _notFoundResult = new("Not Found");

        public TextController(ITextStoreService textStoreService)
        {
            _textStoreService = textStoreService;
        }
        
        [HttpGet]
        public IAsyncEnumerable<StoredText> GetTexts([FromQuery] int page = 1)
        {
            // If the page is less than 1, we set it to one.
            if (page < 1)
                page = 1;
            return _textStoreService.Get(--page);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            StoredText? storedText = await _textStoreService.Get(id);
            if (storedText is null)
                return _notFoundResult;
            return Ok(storedText.Text);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] string text)
        {
            StoredText storedText = await _textStoreService.Upload(text);
            return Ok(storedText);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            StoredText? text = await _textStoreService.Get(id);
            if (text is null)
                return _notFoundResult;

            bool didDelete = await _textStoreService.Delete(text);
            return didDelete ? NoContent() : _notFoundResult;
        }
    }
}