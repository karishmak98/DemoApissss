using Microsoft.EntityFrameworkCore;
using WebApiPractice.Data;
using WebApiPractice.Models;
using WebApiPractice.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiPractice.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _context;

        public TrailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTrailAsync(Trail trail)
        {
            await _context.Trails.AddAsync(trail);
            return await SaveAsync();
        }

        public async Task<bool> DeleteTrailAsync(Trail trail)
        {
            _context.Trails.Remove(trail);
            return await SaveAsync();
        }

        public async Task<Trail?> GetTrailAsync(int? trailId)
        {
            return await _context.Trails
                .Include(x => x.NationalPark)
                .FirstOrDefaultAsync(x => x.Id == trailId);
        }

        public async Task<ICollection<Trail>> GetTrailsAsync()
        {
            return await _context.Trails
                .Include(x => x.NationalPark)
                .ToListAsync();
        }

        public async Task<ICollection<Trail>> GetTrailsInNationalParkAsync(int nationalParkId)
        {
            return await _context.Trails
                .Include(np => np.NationalPark)
                .Where(t => t.NationalParkId == nationalParkId)
                .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TrailExistsAsync(int trailId)
        {
            return await _context.Trails
                .AnyAsync(x => x.Id == trailId);
        }

        public async Task<bool> TrailExistsAsync(string trailName)
        {
            return await _context.Trails
                .AnyAsync(x => x.Name == trailName);
        }

        public async Task<bool> UpdateTrailAsync(Trail trail)
        {
            _context.Trails.Update(trail);
            return await SaveAsync();
        }
    }
}
