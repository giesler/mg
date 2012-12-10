using System;
using System.Collections.Generic;
using System.Text;

namespace msn2.net.Pictures
{
    public class PersonGroup
    {
        private int id;
        private string name;

        public PersonGroup(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id
        {
            get
            {
                return this.id;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}
