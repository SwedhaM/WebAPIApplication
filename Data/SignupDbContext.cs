using Signup.Models;
using Microsoft.EntityFrameworkCore;  
  
namespace Signup.Models
{  
    public class SignupDbContext : DbContext  
    {  
        public SignupDbContext(DbContextOptions<SignupDbContext> options) :  
            base(options)  
        {  
  
        }  
        public DbSet<Register> Signup_Detail{ get; set; }
        
    }  
}