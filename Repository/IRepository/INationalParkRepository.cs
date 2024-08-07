using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPractice.Models;

namespace WebApiPractice.Repository.IRepository
{
    public interface INationalParkRepository
    {
        // Asynchronously retrieves a collection of NationalPark objects
        Task<ICollection<NationalPark>> GetNationalParksAsync();

        // Asynchronously retrieves a single NationalPark object by its ID
        Task<NationalPark?> GetNationalParkAsync(int? id);

        // Asynchronously checks if a NationalPark exists by its ID
        Task<bool> NationalParkExistsAsync(int id);

        // Asynchronously checks if a NationalPark exists by its name
        Task<bool> NationalParkExistsAsync(string name);

        // Asynchronously creates a new NationalPark
        Task<bool> CreateNationalParkAsync(NationalPark nationalPark);

        // Asynchronously updates an existing NationalPark
        Task<bool> UpdateNationalParkAsync(NationalPark nationalPark);

        // Asynchronously deletes a NationalPark
        Task<bool> DeleteNationalParkAsync(NationalPark nationalPark);

        // Asynchronously saves changes to the database
        Task<(List<NationalPark> paginatedList, int totalCount)> GetPaginatedNationalParksAsync(int pageNumber, int pageSize);
        Task<bool> SaveAsync();
    }
}
