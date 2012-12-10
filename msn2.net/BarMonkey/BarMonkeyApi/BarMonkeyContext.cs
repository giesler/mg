using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public class BarMonkeyContext
    {
        private BarMonkeyDataContext dataContext;

        public BarMonkeyContext()
        {
            this.dataContext = new BarMonkeyDataContext();
            this.Drinks = new DrinkManager(this);
            this.Users = new UserManager(this);
            this.Containers = new ContainerManager(this);
        }

        public BarMonkeyDataContext Data
        {
            get { return dataContext; }
            set { dataContext = value; }
        }

        private static BarMonkeyContext current = null;
        private static object lockObject = new object();
        public static BarMonkeyContext Current
        {
            get
            {
                if (current == null)
                {
                    lock (lockObject)
                    {
                        current = new BarMonkeyContext();
                    }
                }

                return current;
            }
        }

        public User CurrentUser
        {
            get
            {
                return this.currentUser;
            }
        }

        private User currentUser;

        public void Login(int userId)
        {
            var q = from u in this.Data.Users
                    where u.Id == userId
                    select u;
            
            this.currentUser = q.First<User>();
        }

        public UserManager Users { get; private set; }
        public DrinkManager Drinks { get; private set; }
        public ContainerManager Containers { get; private set; }

        public double OuncesDispensedPerSecond
        {
            get
            {
                return 1.2;
            }
        }
    }
}
