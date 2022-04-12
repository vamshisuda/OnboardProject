
using System;
using System.ComponentModel.DataAnnotations;
namespace UserAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public String UserName { get; set; }


        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }

        [Required]
        public String Email { get; set; }
    }
}


