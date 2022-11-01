﻿using System.ComponentModel.DataAnnotations;

namespace JWTAPI.Models
{
    public class Logs
    {
        [Key]
        public int userID { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Descryption { get; set; }
    }
}
