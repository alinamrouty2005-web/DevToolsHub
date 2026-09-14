using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class ApiRequestUpdateDto
    {
        public int? ProjectId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Method { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Url { get; set; } = string.Empty;

        public string? Headers { get; set; }

        public string? Body { get; set; }

        [Range(100, 599)]
        public int? StatusCode { get; set; }

        public string? ResponseBody { get; set; }
    }
}
