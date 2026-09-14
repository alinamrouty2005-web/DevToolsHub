using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class ToolResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
