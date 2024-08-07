using WebApiPractice.Models;

namespace WebApiPractice.Repository.IRepository
{
    public interface ITrailRepository
    {
        Task<ICollection<Trail>> GetTrailsAsync();
        Task<ICollection<Trail>> GetTrailsInNationalParkAsync(int nationalParkId);
        Task<Trail?> GetTrailAsync(int? trailId);
        Task<bool> TrailExistsAsync(int trailId);
        Task<bool> TrailExistsAsync(string trailName);
        Task<bool> CreateTrailAsync(Trail trail);
        Task<bool> UpdateTrailAsync(Trail trail);
        Task<bool> DeleteTrailAsync(Trail trail);
        Task<bool> SaveAsync();
    }
}
