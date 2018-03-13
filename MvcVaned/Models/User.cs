using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcVanced.Models
{
    public class User
    {
        [Display(Name = "UserID")]
        public string ID { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }

    }
}