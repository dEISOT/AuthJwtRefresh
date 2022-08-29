﻿using System.ComponentModel.DataAnnotations;

namespace AuthJwtRefresh.DTO
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
