using Microsoft.EntityFrameworkCore;
using SmartDocs.Entities;
using System.Transactions;

namespace SmartDocs
{
    public class SmartDocsDbContext : DbContext
    {
        public SmartDocsDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<BlockchainTransaction> BlockchainTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
