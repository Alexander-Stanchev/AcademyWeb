using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Areas.Administration.Models
{
    public class AdminViewModel
    {
        public string FullName { get; set; }
        public IEnumerable<UsersViewModel> Users { get; set; }
    }
}
