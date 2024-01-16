using WebAPI.Repository.Abstract;
using WebAPI.Models.Domain;

namespace WebAPI.Repository.Implementation
{
    public class ProductRepostory : IProductRepository
    {
		private readonly DatabaseContext _context;
		public ProductRepostory(DatabaseContext context)
		{
			this._context = context;
		}
        public bool addProduct(AngularProduct model)
        {
			try
			{
				_context.angularProductsTbl.Add(model);
				_context.SaveChanges();
				return true;
			}
			catch (Exception)
			{

				return false;
			}
        }

        public IEnumerable<AngularProduct> getAll()
		{
			return _context.angularProductsTbl.ToList();
		}

		public AngularProduct getById(int id)
		{
			return _context.angularProductsTbl.Find(id);
		}

		public IEnumerable<AngularProduct> getByCategory(string category)
		{
			return _context.angularProductsTbl.Where(product => product.category == category).ToList();
		}
		


		public bool updateProduct(AngularProduct model)
		{
			try
			{
				_context.angularProductsTbl.Update(model);
				_context.SaveChanges();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool deleteProduct(AngularProduct model)
		{
			try
			{
				_context.angularProductsTbl.Remove(model);
				_context.SaveChanges();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}


    }
}
