using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public class PropertyForm: Form
    {
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            this.Opacity = 0.85f;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            this.Opacity = 0.70f;
        }
    }
}
