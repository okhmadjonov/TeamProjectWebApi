using TeamProject.Dto;
using TeamProject.Entity;

namespace TeamProject.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product> Get(int id);
        Task Add(ProductDTO productDto);
        Task Update(int id , ProductDTO productDto);
        Task Delete(int id);
    }
}
