using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace AppScheduling.Models.ViewModels
{

    public class RequireWhenNotAdminAttribute : ValidationAttribute
    {
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            HttpContextAccessor h1 = new HttpContextAccessor();
            string role = h1.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            return role == "Admin" ? ValidationResult.Success : new ValidationResult("Role name is required.");
        }
    }

    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        [Display(Name="Confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [RequireWhenNotAdmin]
        [Display(Name="Role Name")]
        public string RoleName { get; set; }
    }
}
