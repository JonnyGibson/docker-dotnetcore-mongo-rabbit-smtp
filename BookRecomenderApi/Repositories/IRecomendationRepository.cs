using BookRecomenderApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookRecomenderApi.Repositories
{
    public interface IRecomendationRepository
    {
        Task<IEnumerable<Recomendation>> GetRecomendations(int count);
    }
}