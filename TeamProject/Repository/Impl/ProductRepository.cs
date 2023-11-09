using Microsoft.EntityFrameworkCore;
using TeamProject.Data;
using TeamProject.Dto;
using TeamProject.Entity;

namespace TeamProject.Repository.Impl
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> Get(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            return product ?? throw new BadHttpRequestException("Product not found.");
        }

        public async Task Add(ProductDTO productDto)
        {
            var vat = _configuration["Vat"]!;

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                ToTalPrice = CalculateTotalPrice(productDto.Quantity, productDto.Price)
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }


        public async Task Update(int id , ProductDTO productDto)
        {
            var vat = _configuration["Vat"]!;

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is not null)
            {
                product.Name = productDto.Name;
                product.Price = productDto.Price;
                product.Quantity = productDto.Quantity;
                product.ToTalPrice = (productDto.Quantity * productDto.Price) * (1 + Convert.ToDouble(vat));
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new BadHttpRequestException("Product not found");
            }
        }

        public async Task Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is not null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public double CalculateTotalPrice(int quantity, double price)
        {
            var vat = _configuration["Vat"]!;

            return (quantity * price) * (1 + Convert.ToDouble(vat));
        }
    }
}
