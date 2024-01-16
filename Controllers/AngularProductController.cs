using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Domain;
using WebAPI.Models.DTO;
using WebAPI.Repository.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class AngularProductController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        public AngularProductController(DatabaseContext context, IFileService fileService, IProductRepository productRepo)
        {
            this._context = context;
            this._fileService = fileService;
            this._productRepository = productRepo;
        }
        

        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(_productRepository.getAll());
        }

        [HttpGet("{id}")]
        public IActionResult getProductById(int id)
        {
            var product = _productRepository.getById(id);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("NewArrivals")]
        public IActionResult getNewArrivals()
        {
           var newProducts = _context.angularProductsTbl
                .OrderByDescending(product => product.id)
                .Take(10)
                .ToList();


            if (newProducts.Count > 0)
            {
                return Ok(newProducts);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet("{category}")]
        public IActionResult getByCategory(string category)
        {
            var products = _productRepository.getByCategory(category);
            if (products != null && products.Any())
            {
                return Ok(products);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("categories")]
        public IActionResult getCategories()
        {
            var categories = _context.angularProductsTbl.Select(product => product.category).Distinct().ToList();
            return Ok(categories);
        }

        [HttpGet("HomePage")]
        public IActionResult getHomePage()
        {
            // Assuming you have a property in the AngularProduct model that represents the date of addition (e.g., AddedDate)
            var newProducts = _context.angularProductsTbl
                .OrderBy(product => product.id)
                .Take(3)
                .ToList();


            if (newProducts.Count > 0)
            {
                return Ok(newProducts);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet("Products")]
        public IActionResult getProducts()
        {
            // Assuming you have a property in the AngularProduct model that represents the date of addition (e.g., AddedDate)
            var newProducts = _context.angularProductsTbl
                .OrderBy(product => product.id)
                .Take(8)
                .ToList();


            if (newProducts.Count > 0)
            {
                return Ok(newProducts);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet("search")]
        public IActionResult searchProduct(string query)
        {
            var products = _context.angularProductsTbl
                .Where(product => product.name.Contains(query)
                || product.description.Contains(query)
                || product.authorname.Contains(query)
                || product.color.Contains(query)
                || product.category.Contains(query)
                || product.price.ToString().Contains(query))
                .ToList();

            if (products.Count > 0)
            {
                return Ok(products);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPut("{id}")]
        public IActionResult editProduct(int id, [FromForm] AngularProduct model)
        {
            var existingProduct = _productRepository.getById(id);
            if (existingProduct != null)
            {
                // Update the existing product with the new values from the model
                existingProduct.name = model.name;
                existingProduct.price = model.price;
                existingProduct.category = model.category;
                existingProduct.color = model.color;
                existingProduct.authorname = model.authorname;
                existingProduct.description = model.description;
                // You may also need to update the image if a new image is provided
                if (model.ImageFile != null)
                {
                    var fileResult = _fileService.saveImage(model.ImageFile);
                    if (fileResult.Item1 == 1)
                    {
                        // Delete the existing image file
                        _fileService.deleteImage(existingProduct.image);

                        // Update the product with the new image file name
                        existingProduct.image = fileResult.Item2;
                    }
                    else
                    {
                        // Handle the case when there was an error saving the new image file
                        return StatusCode(500, "Error updating the product image.");
                    }
                }
                var success = _productRepository.updateProduct(existingProduct);
                if (success)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "Error updating the product.");
                }
            }
            else
            {
                return NotFound();
            }
        }

[HttpPost]
        public IActionResult addProduct([FromForm] AngularProduct model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass the valid data";
                return Ok(status);
            }
            if (model.ImageFile != null)
            {
                var fileResult = _fileService.saveImage(model.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    model.image = fileResult.Item2; // getting name of image
                }
                var productResult = _productRepository.addProduct(model);
                if (productResult)
                {
                    status.StatusCode = 1;
                    status.Message = "Added successfully";
                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Error on adding product";

                }
            }
            return Ok(status);

        }

        [HttpDelete("{id}")]
        public IActionResult deleteProduct(int id)
        {
            var existingProduct = _productRepository.getById(id);
            if (existingProduct != null)
            {
                // Delete the product
                var success = _productRepository.deleteProduct(existingProduct);
                if (success)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "Error deleting the product.");
                }
            }
            else
            {
                return NotFound();
            }
        }


    }
}












