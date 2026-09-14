using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Models
{
        public class Collection
        {
            public int Id { get; set; }

            public int UserId { get; set; }

            public string Name { get; set; } = string.Empty;

            public string? Description { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public User User { get; set; } = null!;

            public ICollection<CollectionItem> CollectionItems { get; set; } = new List<CollectionItem>();
        }
    }
