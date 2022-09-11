using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class RoleInfo
    {
        public RoleInfo()
        {
            UserInfos = new HashSet<UserInfo>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
