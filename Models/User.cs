using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GG.Models
{
    public class User : BaseEntity
    {
        public int ID {get; set;}
        public string Username {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<Attendee> Attending {get; set;}
        public User()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Attending = new List<Attendee>();
        }
    }
    public class UserRegister : BaseEntity
    {
        [Required]
        [Display(Name = "Username")]
        public string Username {get; set;}

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email {get; set;}

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password {get; set;}

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword {get; set;}
    }
    public class UserLogin : BaseEntity
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string LoginEmail {get; set;}

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string LoginPassword {get; set;}
    }
}