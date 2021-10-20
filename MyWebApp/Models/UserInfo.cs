using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    public class UserInfo
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string UserAgent { get; set; }
    }
}
