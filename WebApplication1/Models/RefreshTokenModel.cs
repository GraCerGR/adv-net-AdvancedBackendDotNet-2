﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RefreshTokenModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }

        public DateTime Expires { get; set; }

    }
}
