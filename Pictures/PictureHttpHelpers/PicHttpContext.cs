using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace msn2.net.Pictures
{
    public class PicHttpContext
    {
        public static PicContext Current
        {
            get
            {
                PicContext context = null;

                if (HttpContext.Current.Items.Contains(PicHttpModule.CONTEXTKEY))
                {
                    context = (PicContext)HttpContext.Current.Items[PicHttpModule.CONTEXTKEY];
                }

                return context;
            }
        }
    }
}
