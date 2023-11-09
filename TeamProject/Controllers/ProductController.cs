using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamProject.Dto;
using TeamProject.Repository;

namespace TeamProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<ProductDTO> _productValidator;

        public ProductController(IProductRepository productRepository, IValidator<ProductDTO> productValidator)
        {
            _productRepository = productRepository;
            _productValidator = productValidator;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllProducts() => Ok(await _productRepository.GetAll());

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetProductById(int id) => Ok(await _productRepository.Get(id));

        [HttpPost]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateProduct(ProductDTO productDto)
        {
            var validationResult = await _productValidator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _productRepository.Add(productDto);
            return Ok();
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDto)
        {
            var validationResult = await _productValidator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _productRepository.Update(id , productDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepository.Delete(id);
            return Ok();
        }
    }
}
