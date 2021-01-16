using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TrelloClone.Core.Models;

namespace TrelloClone.Core.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<BoardModel> Boards { get; set; }
        DbSet<CardModel> Cards { get; set; }
        DbSet<TeamModel> Teams { get; set; }
        DbSet<ListModel> Lists { get; set; }
        Task<int> SaveAsync();

    }
}
