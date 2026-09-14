using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class ToolHistoryResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ToolId { get; set; }
        public int? ProjectId { get; set; }
        public string? InputData { get; set; }
        public string? OutputData { get; set; }
        public DateTime ExecutedAt { get; set; }
    }
}
