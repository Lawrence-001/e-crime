﻿using System.ComponentModel.DataAnnotations;

namespace e_crime.mvc.ViewModels.Auth
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email cannot be empty")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password cannot be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
