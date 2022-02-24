using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DolmToken.Models
{
    public class User
    {
        private int _userId;

        public int UserId
        {
            get { return this._userId; }
            set
            {
                if (value >= 0)
                {
                    this._userId = value;
                }
            }
        }

        public string username { get; set; }

        public string password { get; set; }

        public string email { get; set; }

    }
}
