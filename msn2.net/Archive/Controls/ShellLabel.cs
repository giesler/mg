using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for ShellLabel.
	/// </summary>
	public class ShellLabel : System.Windows.Forms.UserControl
	{
		#region Declares

		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor

		public ShellLabel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		#endregion

		#region Disposal

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ShellLabel
			// 
			this.Name = "ShellLabel";
			this.Size = new System.Drawing.Size(184, 16);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShellLabel_Paint);

		}
		#endregion

		#region Paint

		private void ShellLabel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), e.ClipRectangle);
		}

		#endregion

	}
}
