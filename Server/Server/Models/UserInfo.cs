using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class UserInfo
    {
        public string UserId { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual RoleInfo Role { get; set; } = null!;
    }
}
