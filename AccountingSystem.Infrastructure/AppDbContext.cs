using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        //public DbSet<Account> Accounts => Set<Account>();
        //public DbSet<Transaction> Transactions => Set<Transaction>();
    }
}
