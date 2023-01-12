using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace HomeTG.Models
{
    public class DB : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public DB(DbContextOptions<DB> options) : base(options)
        {}

        public IEnumerable<Card> GetCards(string name, string? set = null)
        {
            name = name.ToLower();
            set = set?.ToLower();
            return Cards.Where(c => (c.Name.ToLower().Contains(name) && (set == null || c.SetCode.ToLower().Equals(set))));
        }
    }
}
