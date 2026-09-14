using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class ErrorResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
