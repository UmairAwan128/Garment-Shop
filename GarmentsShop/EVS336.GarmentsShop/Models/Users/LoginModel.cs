using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models.Users
{
    public class LoginModel
    {
      
        public string LoginId { get; set; }

      
        public string Password { get; set; }

    
        public bool RememberMe { get; set; }

        //will be used for tracking that if user enter 3 times wrong password move it to Password recovery page
        public int LoginAttemptNo { get; set; }
    }
}