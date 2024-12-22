using BookRecomenderApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using System;

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
            Console.WriteLine("Getting recommendations from repository...");
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
            // First get the total count of records
            var totalCount = await _context.Recomendations.CountDocumentsAsync(_ => true);
            Console.WriteLine($"Total records in database: {totalCount}");

            if (totalCount == 0)
            {
                Console.WriteLine("No records found in database!");
                return new List<Recomendation>();
            }

            // Calculate maximum skip value (ensure we can still get 'count' records)
            var maxSkip = Math.Max(0, totalCount - count);
            var rand = new Random();
            var skip = rand.Next(0, (int)maxSkip + 1);

            Console.WriteLine($"Getting {count} records with skip {skip}");
            
            var results = await _context
                            .Recomendations
                            .Find(_ => true)
                            .Skip(skip)
                            .Limit(count)
                            .ToListAsync();

            Console.WriteLine($"Retrieved {results.Count} records");
            return results;
        }
    }
}