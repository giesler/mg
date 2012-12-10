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
                        select d;
            
            return users.ToList<User>();
        }

        public User GetUser(int id)
        {
            var q = from d in base.Context.Data.Users
                    where d.Id == id
                    select d;
            List<User> list = q.ToList<User>();
            User user = list.Count > 0 ? list[0] : null;
            return user;
        }
    }
}
