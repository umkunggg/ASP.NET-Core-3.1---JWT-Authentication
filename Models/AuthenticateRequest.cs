using System.ComponentModel.DataAnnotations;

namespace JWT.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username{ get; set;}
        
        [Required]
        public string Password{ get; set;}
    }
}