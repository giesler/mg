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
		#region Declares

		private System.ComponentModel.Container components = null;
		private Color startColor = Color.LightGray;

		#endregion

		#region Constructors

		public ShellButton()
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
			// ShellButton
			// 
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShellButton_Paint);

		}
		#endregion

		#region Paint

		private void ShellButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, startColor);

			if (this.Text.Length > 0)
			{
				StringFormat format		= new StringFormat();
				format.Alignment		= StringAlignment.Center;
				format.LineAlignment	= StringAlignment.Center;

				int y = this.Height - (this.Height - (this.Font.Height / 2));
				string text = this.Text.Replace("&", "");

				bool cutOff = false;
				try
				{
					while (e.Graphics.MeasureString(text, this.Font).Width > e.ClipRectangle.Width)
					{
						if (!cutOff)
						{
							cutOff = true;
							text = text + text.Substring(0, text.Length - 3) +  "...";
						}
						else
						{
							text = text.Substring(0, text.Length - 5) + "...";
						}
					}
				}
				catch (Exception)
				{
					// BUGBUG - in above code, text.substring is probably cutting off more then 'text'
				}

				e.Graphics.DrawString(text, this.Font, new SolidBrush(Color.Black), e.ClipRectangle, format);
			}
		}

		#endregion

		#region Properties

		public Color StartColor
		{
			get
			{
				return startColor;
			}
			set
			{
				startColor = value;
				this.Refresh();
			}
		}

		#endregion

	}
}