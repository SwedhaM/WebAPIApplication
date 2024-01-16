// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;
// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using System.Threading.Tasks;
// using WebAPI.Data;
// using WebAPI.Models;

// [Route("Products")]
// public class ProductController : ControllerBase
// {
//     private readonly IWebHostEnvironment _environment;
//     private readonly DataContext _db;

//     public ProductController(IWebHostEnvironment environment,DataContext db)
//     {
//         _environment = environment;
//         _db=db;
//     }

//     [HttpPost]
//     public IActionResult CreateProduct([FromForm] Product product, [FromForm] IFormFile file)
//     {
//         // Validate and save the product details to the database
//         // Generate a unique filename and save the file to a designated folder
//         if (file != null && file.Length > 0)
//         {
//             string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
//             string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
//             string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//             using (var stream = new FileStream(filePath, FileMode.Create))
//             {
//                 file.CopyTo(stream);
//             }

//             // Update the product's image property with the filename or file path
//             product.image = uniqueFileName;
//         }

//         // Save the product to the database or perform other necessary actions
//         // Return the created product or appropriate response
//         _db.Products.Add(product);
//         _db.SaveChangesAsync();
//         return Ok(product);
//     }

//     [HttpPut("{id}")]
//     public IActionResult UpdateProduct(int id, [FromForm] Product product, [FromForm] IFormFile file)
//     {
//         // Validate and update the product details in the database
//         // Check if a new file is provided and handle it accordingly
//         if (file != null && file.Length > 0)
//         {
//             // Delete the existing file (if any) and save the new file
//             string existingFilePath = Path.Combine(_environment.WebRootPath, "uploads", product.image);
//             if (System.IO.File.Exists(existingFilePath))
//             {
//                 System.IO.File.Delete(existingFilePath);
//             }

//             string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
//             string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
//             string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//             using (var stream = new FileStream(filePath, FileMode.Create))
//             {
//                 file.CopyTo(stream);
//             }

//             // Update the product's image property with the new filename or file path
//             product.image = uniqueFileName;
//         }

//         // Update the product in the database or perform other necessary actions
//         // Return the updated product or appropriate response
//         _db.Products.Update(product);
//         _db.SaveChangesAsync();
//         return Ok(product);
//     }

//     [HttpDelete("{id}")]
//     public IActionResult DeleteProduct(int id)
//     {
//         var product = _db.Products.FirstOrDefault(p => p.id == id);

//         // Retrieve the product from the database and delete it
//         // Delete the associated image file (if any)
//         // Return the appropriate response
//         // _db.Products.Remove()
//         // _db.SaveChangesAsync();
//         // return NoContent();
//         if (product != null)
//         {
//             // Delete the associated image file (if any)
//             if (!string.IsNullOrEmpty(product.image))
//             {
//                 string imagePath = Path.Combine(_environment.WebRootPath, "uploads", product.image);
//                 if (System.IO.File.Exists(imagePath))
//                 {
//                     System.IO.File.Delete(imagePath);
//                 }
//             }

//             // Delete the product from the database
//             _db.Products.Remove(product);
//             _db.SaveChanges();

//             return NoContent();
//         }

//         return NotFound();

//     }


//     [HttpGet]
//     public IActionResult GetProducts()
//     {
//         // Retrieve all products from the database
//         var products = _db.Products.ToList();

//         if (products.Count > 0)
//         {
//             return Ok(products); // Return the products as the response
//         }

//         return NotFound(); // No products found
//     }

    

// }
