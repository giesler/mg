#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace msn2.net.Pictures
{
    public class PictureSystem: MarshalByRefObject
    {
        public PictureSystem()
        {
        }


        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void AddPictures()
        {
        }

    }
}
