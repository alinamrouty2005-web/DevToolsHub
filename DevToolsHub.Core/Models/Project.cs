using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Models
{
    public class Project
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Many-to-One
        public User User { get; set; } = null!;

        // One-to-Many
        public ICollection<ToolHistory> ToolHistories { get; set; } = new List<ToolHistory>();

        // One-to-Many
        public ICollection<ApiRequest> ApiRequests { get; set; } = new List<ApiRequest>();
    }
}
