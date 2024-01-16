using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class SignUpData
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password must match.")]
        [DataType(DataType.Password)]
        
        public string ConfirmPassword { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; }
    }
}



//  [Required]
//         public int UserID { get; set; }