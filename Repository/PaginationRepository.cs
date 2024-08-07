using WebApiPractice.Repository.IRepository;

namespace WebApiPractice.Repository
{
    public class PaginationRepository : IPaginationRepository
    {
        public async Task<(List<T> paginatedList, int totalCount)> PaginationList<T>(IEnumerable<T> sourceList, int pageNumber = 1, int pageSize = 10)
        {
            int skipAmount = (pageNumber - 1) * pageSize;

            // Skip ordering if it's not required or order the list before calling this method
            List<T> paginatedList = sourceList.Skip(skipAmount).Take(pageSize).ToList();

            int totalCount = sourceList.Count(); // Get the total count before pagination

            return (paginatedList, totalCount);
        }
    }
}
