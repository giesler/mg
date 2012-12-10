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

        public string UserName
        {
            get
            {
                return this.userName;
            }
        }

        private string userName = "EEK";

        public void Login(string userName)
        {
            this.userName = userName;
        }

        public UserManager Users { get; private set; }
        public DrinkManager Drinks { get; private set; }
    }
}
