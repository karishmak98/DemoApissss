using WebApiPractice.Models;

namespace WebApiPractice.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<bool> CreateProductAsync(ProductUploadModel product, string[] allowedExtension);
        Task<bool> UpdateProductAsync(int id,ProductUploadModel product, string[] allowedExtension);
        Task<bool> DeleteProductAsync(Product product);
        Task<bool> ProductExistsAsync(string name);
        Task<bool> ProductExistsAsync(int id);

        Task<string> SaveImageFileAsync(IFormFile imageFile);
        Task<bool> SaveAsync();

    }
}
