using WebAPI.Models.Domain;

namespace WebAPI.Repository.Abstract
{
    public interface IProductRepository
    {
        bool addProduct(AngularProduct model);
        AngularProduct getById(int id);
        bool updateProduct(AngularProduct model);
        bool deleteProduct(AngularProduct model);
        IEnumerable<AngularProduct> getAll();
        IEnumerable<AngularProduct> getByCategory(string category);

    }
}
