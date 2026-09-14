using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Models
{
    public class Tool
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Category { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<ToolHistory> ToolHistories { get; set; } = new List<ToolHistory>();

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        public ICollection<CollectionItem> CollectionItems { get; set; } = new List<CollectionItem>();
    }
}
