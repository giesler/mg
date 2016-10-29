using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.Configuration;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for ShellQuestion.
	/// </summary>
	public class ShellQuestion : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.Label labelMessage;
		private System.Windows.Forms.Label labelUrl;
		private msn2.net.Controls.ShellButton buttonOpen;
		private msn2.net.Controls.ShellButton buttonCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		public ShellQuestion()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public ShellQuestion(Data data, string url): base(data)
		{
			this.labelUrl.Text = url;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.labelMessage = new System.Windows.Forms.Label();
			this.labelUrl = new System.Windows.Forms.Label();
			this.buttonOpen = new msn2.net.Controls.ShellButton();
			this.buttonCancel = new msn2.net.Controls.ShellButton();
			this.SuspendLayout();
			// 
			// labelMessage
			// 
			this.labelMessage.Location = new System.Drawing.Point(8, 8);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(272, 16);
			this.labelMessage.TabIndex = 0;
			this.labelMessage.Text = "Do you want to open the following url?";
			this.labelMessage.Visible = false;
			// 
			// labelUrl
			// 
			this.labelUrl.Location = new System.Drawing.Point(32, 32);
			this.labelUrl.Name = "labelUrl";
			this.labelUrl.Size = new System.Drawing.Size(280, 16);
			this.labelUrl.TabIndex = 1;
			this.labelUrl.Text = "[url]";
			this.labelUrl.Visible = false;
			// 
			// buttonOpen
			// 
			this.buttonOpen.Location = new System.Drawing.Point(160, 64);
			this.buttonOpen.Name = "buttonOpen";
			this.buttonOpen.StartColor = System.Drawing.Color.LightGray;
			this.buttonOpen.TabIndex = 2;
			this.buttonOpen.Text = "&Open";
			this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(248, 64);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.StartColor = System.Drawing.Color.LightGray;
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// ShellQuestion
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(328, 94);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonCancel,
																		  this.buttonOpen,
																		  this.labelUrl,
																		  this.labelMessage});
			this.Name = "ShellQuestion";
			this.Text = "ShellQuestion";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShellQuestion_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		private void ShellQuestion_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);

			e.Graphics.DrawString(this.labelMessage.Text, this.labelMessage.Font, new SolidBrush(SystemColors.ControlText), new RectangleF(labelMessage.Location, labelMessage.Size));
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Visible = false;
			this.Dispose();
		}

		private void buttonOpen_Click(object sender, System.EventArgs e)
		{
			WebBrowser browser = new WebBrowser(Data.Text, this.labelUrl.Text);
			browser.Show();
		}
	}
}
