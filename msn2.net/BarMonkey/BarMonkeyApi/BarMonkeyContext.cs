﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public class BarMonkeyContext
    {
        private BarMonkeyDataContext dataContext;
        public string ConnectionString { get; private set; }

        public BarMonkeyContext()
        {
            this.ConnectionString = "Data Source=192.168.1.188;Initial Catalog=BarMonkey;User ID=bmuser;Password=foobar!12";
            if (Environment.MachineName.ToLowerInvariant() != "mgduo")
            {
                this.ConnectionString = "Data Source=kenny;Initial Catalog=BarMonkey;User ID=bmuser;Password=foobar!12";
            }

            this.dataContext = new BarMonkeyDataContext(this.ConnectionString);
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

        public void Reload()
        {
            this.dataContext = new BarMonkeyDataContext(this.ConnectionString);
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

        public List<SweetnessLevel> Sweetnesses
        {
            get
            {
                List<SweetnessLevel> list = new List<SweetnessLevel>();
                list.Add(new SweetnessLevel { Id = 0, Name = "Normal", Description = "" });
                list.Add(new SweetnessLevel { Id = 1, Name = "Sweet", Description = "" });
                return list;
            }
        }
    }

    public class SweetnessLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

