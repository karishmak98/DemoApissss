using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApiPractice.Models
{
    public class Trail
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public enum  DifficultyType { Easy,Moderate,Difficult}
        public DifficultyType Difficulty { get; set; }
        public int NationalParkId { get; set; }
        [ForeignKey("NationalParkId")]
        public NationalPark? NationalPark { get; set; }

    }
}
