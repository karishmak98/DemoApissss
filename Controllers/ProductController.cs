using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApiPractice.Models;
using WebApiPractice.Repository;
using WebApiPractice.Repository.IRepository;

namespace WebApiPractice.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public  async Task<IActionResult> GetProducts() 
        {
            var products = await _productRepository.GetProductsAsync();
            // Construct full image URL
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var productList = products.Select(p => new
            {
                p.Id,
                p.Name,
                ImageUrl = $"{baseUrl}/Uploads/Products/{p.ImageUrl}"  // Create full URL for image
            });
            return Ok(productList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm]ProductUploadModel product)
        {
            if (product == null) return BadRequest("product object is null.");
            if (product.ImageFile?.Length > 1 * 1024 * 1024)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
            }
            if (await _productRepository.ProductExistsAsync(product.Name))
            {
                ModelState.AddModelError("", $"Product already in DB: {product.Name}");
                return StatusCode(StatusCodes.Status409Conflict, ModelState); // 409 Conflict
            }
            // If the product does not exist, proceed with the file upload and product creation
            try
            {
                // Validate the file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                await _productRepository.CreateProductAsync(product, allowedExtensions);
                return Ok("Product created successfully.");
            }
            catch (Exception ex)
            {
                // Log exception and return a server error response
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while creating the product.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id,[FromForm] ProductUploadModel product)
        {
            if (product == null) return BadRequest("Product object is null."); // 400 Bad Request
            if (product.ImageFile?.Length > 1 * 1024 * 1024)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
            }
            string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
            if (!await _productRepository.UpdateProductAsync(id,product, allowedFileExtentions))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the product: {product.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError,ModelState); // 500 Internal Server Error
            }

            return Ok("Product update successfully");
        }
    }
}
