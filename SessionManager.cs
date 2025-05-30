using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convenience_Store_Management.Helper
{
    public static class SessionManager
    {
        public static string CurrentLoggedInEmployeeId { get; set; }
        public static string CurrentLoggedInCustomerSdt { get; set; }
    }
}
