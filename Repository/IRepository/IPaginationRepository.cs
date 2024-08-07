namespace WebApiPractice.Repository.IRepository
{
    public interface IPaginationRepository
    {
        Task<(List<T> paginatedList, int totalCount)> PaginationList<T>(IEnumerable<T> sourceList, int pageNumber = 1, int pageSize = 10);
    }
}
