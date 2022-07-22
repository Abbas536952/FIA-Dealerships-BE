using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.Entities.Account
{
    public class SystemClaim
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RoleClaim> RoleClaims { get; set; }
    }
}
