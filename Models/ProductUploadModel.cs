using System.ComponentModel.DataAnnotations;

namespace WebApiPractice.Models
{
    public class ProductUploadModel
    {

        [Required]
        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
