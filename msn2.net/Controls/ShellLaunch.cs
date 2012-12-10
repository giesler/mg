using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using msn2.net.Configuration;

namespace msn2.net.Controls
{
	public class ShellLaunch : msn2.net.Controls.ShellForm
	{
		#region Declares

		private System.Windows.Forms.TextBox textBox1;
		private msn2.net.Controls.ShellButton buttonGo;
		private System.Windows.Forms.CheckBox checkBoxCommandPrompt;
		private System.Windows.Forms.CheckBox checkBoxWebsite;
		private System.ComponentModel.IContainer components = null;

		#endregion

		#region Constructor

		public ShellLaunch()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public ShellLaunch(Data data): base(data)
		{
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
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.buttonGo = new msn2.net.Controls.ShellButton();
			this.checkBoxCommandPrompt = new System.Windows.Forms.CheckBox();
			this.checkBoxWebsite = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBox1.Location = new System.Drawing.Point(8, 8);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(208, 20);
			this.textBox1.TabIndex = 5;
			this.textBox1.Text = "<command>";
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			// 
			// buttonGo
			// 
			this.buttonGo.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonGo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonGo.Location = new System.Drawing.Point(224, 8);
			this.buttonGo.Name = "buttonGo";
			this.buttonGo.Size = new System.Drawing.Size(40, 24);
			this.buttonGo.StartColor = System.Drawing.Color.LightGray;
			this.buttonGo.TabIndex = 6;
			this.buttonGo.Text = "go";
			this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
			// 
			// checkBoxCommandPrompt
			// 
			this.checkBoxCommandPrompt.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.checkBoxCommandPrompt.BackColor = System.Drawing.Color.Transparent;
			this.checkBoxCommandPrompt.Location = new System.Drawing.Point(96, 32);
			this.checkBoxCommandPrompt.Name = "checkBoxCommandPrompt";
			this.checkBoxCommandPrompt.Size = new System.Drawing.Size(120, 16);
			this.checkBoxCommandPrompt.TabIndex = 7;
			this.checkBoxCommandPrompt.Text = "&Command Prompt";
			this.checkBoxCommandPrompt.CheckedChanged += new System.EventHandler(this.checkBoxCommandPrompt_CheckedChanged);
			// 
			// checkBoxWebsite
			// 
			this.checkBoxWebsite.BackColor = System.Drawing.Color.Transparent;
			this.checkBoxWebsite.Checked = true;
			this.checkBoxWebsite.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxWebsite.Location = new System.Drawing.Point(8, 32);
			this.checkBoxWebsite.Name = "checkBoxWebsite";
			this.checkBoxWebsite.Size = new System.Drawing.Size(80, 16);
			this.checkBoxWebsite.TabIndex = 8;
			this.checkBoxWebsite.Text = "&Website";
			// 
			// ShellLaunch
			// 
			this.AcceptButton = this.buttonGo;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(272, 54);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.checkBoxCommandPrompt,
																		  this.checkBoxWebsite,
																		  this.buttonGo,
																		  this.textBox1});
			this.Name = "ShellLaunch";
			this.Text = "Shell Launcher";
			this.TitleVisible = true;
			this.Activated += new System.EventHandler(this.ShellLaunch_Activated);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShellLaunch_Paint);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Go button

		private void buttonGo_Click(object sender, System.EventArgs e)
		{

				try
				{
					if (checkBoxCommandPrompt.Checked)
					{
					
						Process p = new Process();

						if (textBox1.Text.Length == 0)
						{
							p.StartInfo = new ProcessStartInfo("cmd.exe", @"");
						}
						else if (checkBoxCommandPrompt.Checked)
						{
							p.StartInfo = new ProcessStartInfo("cmd.exe", @"/k " + textBox1.Text);
						}
						else
						{
							p.StartInfo = new ProcessStartInfo(textBox1.Text);				
						}

						p.Start();
					
					}
					// start browser
					else
					{
						
						if (!textBox1.Text.Substring(0, 4).Equals("http"))
						{
							textBox1.Text = "http://" + textBox1.Text;
						}

						Data newData = Data.Get(textBox1.Text, textBox1.Text, new FavoriteConfigData(), typeof(FavoriteConfigData));

						WebBrowser browser = new WebBrowser(newData);
						browser.AddNewTab(newData.Text, newData.Url);
						browser.Show();

					}

				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error starting program", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

		this.textBox1.SelectAll();
		}

		#endregion

		#region Activate

		private void ShellLaunch_Activated(object sender, System.EventArgs e)
		{
			this.textBox1.SelectAll();
			this.textBox1.Focus();
		}

		#endregion

		#region Paint

		private void ShellLaunch_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);
		}

		#endregion

		private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.Enter)
			{
				textBox1.Text = "www." + textBox1.Text + ".com";
				e.Handled = true;
				buttonGo_Click(sender, EventArgs.Empty);
			}
		}

		private void checkBoxCommandPrompt_CheckedChanged(object sender, System.EventArgs e)
		{
			checkBoxWebsite.Checked = !checkBoxCommandPrompt.Checked;
		}

	}
}

