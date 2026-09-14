using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Models
{
    public class ApiRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? ProjectId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Method { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string? Headers { get; set; }

        public string? Body { get; set; }

        public int? StatusCode { get; set; }

        public string? ResponseBody { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public User User { get; set; } = null!;

        public Project? Project { get; set; }
    }
}
