using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public abstract class BaseLinkList: FlowLayoutPanel
    {
        public void Clear()
        {
            this.Controls.Clear();
        }

    }
}
