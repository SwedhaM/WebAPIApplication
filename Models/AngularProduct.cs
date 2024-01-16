using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class AngularProduct
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string name { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        public string category { get; set; }
        [Required]
        public string color { get; set; }
        [Required]
        public string image { get; set; }
        [Required]
        public string authorname { get; set; }
        [Required]
        public string description { get; set; }
        public int? quantity { get; set; }
        public int? productId { get; set; }

        [NotMapped]
        public IFormFile? ImageFile{ get; set; }
    }
}