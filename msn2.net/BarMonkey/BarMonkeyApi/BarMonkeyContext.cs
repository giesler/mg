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
            this.Ingredients = new IngredientManager(this);
            this.Relays = new RelayManager(this);
        }

        public BarMonkeyContext Clone()
        {
            BarMonkeyContext context = new BarMonkeyContext();
            if (this.currentUser != null)
            {
                context.Login(this.currentUser.Id);
            }
            return context;
        }

        internal BarMonkeyDataContext Data
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
        public IngredientManager Ingredients { get; private set; }
        public RelayManager Relays { get; private set; }
        public User ImpersonateUser { get; set; }

        public List<Setting> Settings
        {
            get
            {
                var q = from s in this.Data.Settings
                        orderby s.Name
                        select s;

                return q.ToList<Setting>();
            }
        }
    }
}
