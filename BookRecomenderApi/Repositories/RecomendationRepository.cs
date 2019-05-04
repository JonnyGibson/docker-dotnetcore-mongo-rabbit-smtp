using BookRecomenderApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using  System;

namespace BookRecomenderApi.Repositories
{
    public class RecomendationRepository : IRecomendationRepository
    {

        private readonly IRecomendationContext _context;
        public RecomendationRepository(IRecomendationContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Recomendation>> GetRecomendations(int count)
        {
            return await GetRandomTake(count);

        }

        private async Task<IEnumerable<Recomendation>> GetAll()
        {
            return await _context
                            .Recomendations
                            .Find(_ => true)
                            .ToListAsync();
        }

        private async Task<IEnumerable<Recomendation>> GetRandomTake(int count)
        {
            var rand = new Random();
            var i = rand.Next(0, 100 - count);
            return await _context
                            .Recomendations
                            .Find(_ => true)
                            .Skip(i)
                            .Limit(count)
                            .ToListAsync();

        }
    }
}