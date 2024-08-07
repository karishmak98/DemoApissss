using WebApiPractice.Models;

namespace WebApiPractice.Response
{
    public class NationalParkResponseModel
    {
        public IList<NationalPark> NationalParks { get; set; }
        public int TotalRecords { get; set; }
        public int PageLimit { get; set; }
    }
}
