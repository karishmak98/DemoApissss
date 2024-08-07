using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using WebApiPractice.Data;
using WebApiPractice.Models;
using WebApiPractice.Repository.IRepository;

namespace WebApiPractice.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public ProductRepository(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<bool> CreateProductAsync(ProductUploadModel product, string[] allowedExtensions)
        {
            ValidateImageExtension(product.ImageFile.FileName, allowedExtensions);

            string uniqueFileName = await SaveImageFileAsync(product.ImageFile);
            var newProduct = new Product
            {
                Name = product.Name,
                ImageUrl = uniqueFileName
            };

            await _context.Products.AddAsync(newProduct);
            return await SaveAsync();
        }

        public Task<bool> DeleteProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> ProductExistsAsync(string name)
        {
            return await _context.Products.AnyAsync(p => p.Name == name);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateProductAsync(int id,ProductUploadModel product, string[] allowedExtensions)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return false; // Product not found
            }

            if (product.ImageFile != null)
            {
                ValidateImageExtension(product.ImageFile.FileName, allowedExtensions);
                string uniqueFileName = await SaveImageFileAsync(product.ImageFile);

                // Delete old image file if it exists
                if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                {
                    DeleteImageFile(existingProduct.ImageUrl);
                }

                existingProduct.ImageUrl = uniqueFileName;
            }

            existingProduct.Name = product.Name;
            _context.Products.Update(existingProduct);
            return await SaveAsync();
        }

        private void ValidateImageExtension(string fileName, string[] allowedExtensions)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                throw new ArgumentException($"Only the following extensions are allowed: {string.Join(", ", allowedExtensions)}.");
            }
        }
        private void DeleteImageFile(string fileNameWithExtension)
        {
            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads", "Products");
            var filePath = Path.Combine(uploadsFolder, fileNameWithExtension);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<string> SaveImageFileAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads", "Products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        
    }
}
