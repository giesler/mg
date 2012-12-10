using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public class UserManager: ManagerBase
    {        
        internal UserManager(BarMonkeyContext context)
            : base(context, typeof(User))
        {
        }

        public List<User> GetUsers()
        {
            var users = from d in base.Context.Data.Users
                        select new User();
            
            return users.ToList<User>();
        }
    }
}
