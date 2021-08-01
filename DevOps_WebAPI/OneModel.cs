using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOps_WebAPI
{
    public class OneModel
    {
        public OneModel()
        {
            this.UserId = int.MinValue;
            this.UserName = string.Empty;
            this.EmailAddress = string.Empty;
            this.PhoneNo = string.Empty;
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNo { get; set; }
    }
}
