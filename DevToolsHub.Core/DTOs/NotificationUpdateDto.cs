using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class NotificationUpdateDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
