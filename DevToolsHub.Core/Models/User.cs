using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // One-to-Many
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        // Many-to-Many through UserRole
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // One-to-Many
        public ICollection<ToolHistory> ToolHistories { get; set; } = new List<ToolHistory>();

        // One-to-Many
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        // One-to-Many
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();

        // One-to-Many
        public ICollection<ApiRequest> ApiRequests { get; set; } = new List<ApiRequest>();

        // One-to-One
        public Subscription? Subscription { get; set; }

        // One-to-Many
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}



