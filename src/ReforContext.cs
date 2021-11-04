using Microsoft.EntityFrameworkCore;
using Refor.Models;

namespace Refor
{
    public class ReforContext : DbContext
    {
        public DbSet<StoredText> Texts => Set<StoredText>();

        // This constructor is required to initialize the DB context, don't remove it!
        public ReforContext(DbContextOptions<ReforContext> options) : base(options) { }
    }
}
