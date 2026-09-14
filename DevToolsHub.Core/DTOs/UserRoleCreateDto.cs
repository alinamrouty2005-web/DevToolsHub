using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class UserRoleCreateDto
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }
    }
}
