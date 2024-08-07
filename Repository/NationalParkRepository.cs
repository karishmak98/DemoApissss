using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiPractice.Data;
using WebApiPractice.Models;
using WebApiPractice.Repository.IRepository;

namespace WebApiPractice.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaginationRepository _pagination;
        public NationalParkRepository(ApplicationDbContext context, IPaginationRepository pagination)
        {
            _context = context;
           _pagination = pagination;
        }

        public async Task<bool> CreateNationalParkAsync(NationalPark nationalPark)
        {
            await _context.NationalParks.AddAsync(nationalPark);
            return await SaveAsync();
        }

        public async Task<bool> DeleteNationalParkAsync(NationalPark nationalPark)
        {
            _context.NationalParks.Remove(nationalPark);
            return await SaveAsync();
        }

        public async Task<NationalPark?> GetNationalParkAsync(int? id)
        {
            return await _context.NationalParks.FindAsync(id.GetValueOrDefault());
        }

        public async Task<ICollection<NationalPark>> GetNationalParksAsync()
        {
            return await _context.NationalParks.ToListAsync();
        }

        public async Task<(List<NationalPark> paginatedList, int totalCount)> GetPaginatedNationalParksAsync(int pageNumber, int pageSize)
        {

            var paginatedList = await _context.NationalParks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var totalRecords = await _context.NationalParks.CountAsync();
            return (paginatedList, totalRecords);
        }


        public async Task<bool> NationalParkExistsAsync(int id)
        {
            return await _context.NationalParks.AnyAsync(np => np.NationalParkId == id);
        }

        public async Task<bool> NationalParkExistsAsync(string name)
        {
            return await _context.NationalParks.AnyAsync(np => np.Name == name);
        }

        public async Task<bool> SaveAsync()
        {
            // Return true if changes are saved successfully, otherwise false
            return await _context.SaveChangesAsync() ==1?true:false;
        }

        public async Task<bool> UpdateNationalParkAsync(NationalPark nationalPark)
        {
            _context.NationalParks.Update(nationalPark);
            return await SaveAsync();
        }
    }
}
