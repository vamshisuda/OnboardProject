using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UserAPI.Utility;
using UserAPI.Models;

namespace UserAPI.DTOs
{
   
        public class CreateUserDTO
        {
            [Required]
            public String UserName { get; set; }


            [Required]
            public String FirstName { get; set; }

            [Required]
            public String LastName { get; set; }
            public String Email { get; set; }
        }

       
    public class GeneralUserDTO
    {
        
        public String UserName { get; set; }


        
        public String FirstName { get; set; }

        
        public String LastName { get; set; }

        
        public String Email { get; set; }
    }

    public class UserOutputModel
    {
        public PagingHeader Paging { get; set; }
        public List<LinkInfo> Links { get; set; }
        public List<User> Items { get; set; }
    }
}
