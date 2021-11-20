using Core.Interfaces;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        public Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}