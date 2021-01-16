using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using TrelloClone.Core.Interfaces;
using TrelloClone.Core.Models;

namespace TrelloClone.Infrastructure
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {

        public DbSet<BoardModel> Boards { get; set; }
        public DbSet<CardModel> Cards { get; set; }
        public DbSet<TeamModel> Teams { get; set; }
        public DbSet<ListModel> Lists { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ListCardModel>().HasKey(lcm => new { lcm.CardId, lcm.ListId });
            builder.Entity<TeamUserModel>().HasKey(tum => new { tum.TeamId, tum.UserId });

        }

        public async Task<int> SaveAsync()
        {
            return await this.SaveChangesAsync();
        }
    }
}
