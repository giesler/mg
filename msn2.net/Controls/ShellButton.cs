using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for ShellButton.
	/// </summary>
	public class ShellButton : System.Windows.Forms.Button
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ShellButton()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ShellButton
			// 
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShellButton_Paint);

		}
		#endregion

		private void ShellButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);

			if (this.Text.Length > 0)
			{
				StringFormat format		= new StringFormat();
				format.Alignment		= StringAlignment.Center;
				format.LineAlignment	= StringAlignment.Center;


				int y = this.Height - (this.Height - (this.Font.Height / 2));

				e.Graphics.DrawString(this.Text.Replace("&",""), this.Font, new SolidBrush(Color.Black), e.ClipRectangle, format);
			}
		}
	}
}
