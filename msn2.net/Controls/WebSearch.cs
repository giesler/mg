using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace msn2.net.Controls
{
	public class WebSearch : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button buttonGo;
		private System.ComponentModel.IContainer components = null;

		public WebSearch()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.Text = "Web Search";
			comboBox1.SelectedIndex = 0;

			this.Left = Screen.PrimaryScreen.Bounds.Right - this.Width - 50;
			this.Top  = Screen.PrimaryScreen.Bounds.Bottom  - this.Height - 800;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.buttonGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.Items.AddRange(new object[] {
														   "Google",
														   "Google Groups"});
			this.comboBox1.Location = new System.Drawing.Point(8, 8);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(240, 21);
			this.comboBox1.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBox1.Location = new System.Drawing.Point(8, 32);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(192, 20);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "<search>";
			// 
			// buttonGo
			// 
			this.buttonGo.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonGo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonGo.Location = new System.Drawing.Point(208, 32);
			this.buttonGo.Name = "buttonGo";
			this.buttonGo.Size = new System.Drawing.Size(40, 24);
			this.buttonGo.TabIndex = 3;
			this.buttonGo.Text = "go";
			this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
			// 
			// WebSearch
			// 
			this.AcceptButton = this.buttonGo;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(256, 64);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonGo,
																		  this.textBox1,
																		  this.comboBox1});
			this.FixedSize = new System.Drawing.Size(264, 64);
			this.KeyPreview = true;
			this.Name = "WebSearch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Activated += new System.EventHandler(this.WebSearch_Activated);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WebSearch_KeyUp);
			this.ResumeLayout(false);

		}
		#endregion

		private void WebSearch_Activated(object sender, System.EventArgs e)
		{
			this.textBox1.SelectAll();
			this.textBox1.Focus();
		}

		private void WebSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				textBox1.Text = "";
				textBox1.Focus();
				e.Handled = true;
			}
		}

		private void buttonGo_Click(object sender, System.EventArgs e)
		{
			string url = "";

			if (comboBox1.SelectedIndex == 0)
			{
				url = "http://www.google.com/custom?q={0}";
			}
			else
			{
				url = "http://groups.google.com/groups?hl=en&q=";
			}

			
			url = String.Format(url, System.Web.HttpUtility.UrlEncode(textBox1.Text));

            Process p = new Process();
			p.StartInfo = new ProcessStartInfo(url);
			p.Start();
		}
	}
}

