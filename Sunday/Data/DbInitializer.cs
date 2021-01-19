using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Sunday.Data
{
    public class DbInitializer
    {
        public DbInitializer()
        {
        }

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            context.Database.ExecuteSqlRaw("SELECT create_hypertable({0}, {1});", "\"Data\"", "Timestamp");
            context.SaveChanges();
        }
    }
}
