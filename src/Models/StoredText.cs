using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Refor.Models
{
    [Index(nameof(ID))]
    public class StoredText
    {
        public string ID { get; init; } = null!;
        public string Text { get; init; } = null!;
        public Instant Uploaded { get; init; }
    }
}
