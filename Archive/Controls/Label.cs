using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for Label.
	/// </summary>
	public class Label : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Constructor / Disposal

		public Label()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// Label
			// 
			this.Name = "Label";
			this.Size = new System.Drawing.Size(176, 32);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Label_Paint);

		}
		#endregion

		private void Label_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			SizeF sizeF = new SizeF();
			string temp = this.Text;
			sizeF = e.Graphics.MeasureString(temp, this.Font);
			if (sizeF.Width > this.Width)
				temp = temp + "...";

			while (sizeF.Width > this.Width)
			{
				temp = temp.Substring(0, temp.Length-5) + "...";
				sizeF = e.Graphics.MeasureString(temp, this.Font);
			}
			
			e.Graphics.DrawString(temp, this.Font, new SolidBrush(SystemColors.ControlText), 0, 0);
			
		}

	}
}
