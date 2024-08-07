using System.ComponentModel.DataAnnotations;
namespace WebApiPractice.Models
{
    public class NationalPark
    {
        public int NationalParkId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string State { get; set; }
        public byte[] Picture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Established { get; set; }
    }
}
