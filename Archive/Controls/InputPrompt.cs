using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace msn2.net.Controls
{
	public class InputPrompt : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.TextBox textBoxInput;
		private msn2.net.Controls.ShellButton buttonOK;
		private msn2.net.Controls.ShellButton buttonCancel;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.ComponentModel.IContainer components = null;

		public InputPrompt(string title)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.Text = title;
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
			this.textBoxInput = new System.Windows.Forms.TextBox();
			this.buttonOK = new msn2.net.Controls.ShellButton();
			this.buttonCancel = new msn2.net.Controls.ShellButton();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
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
			// textBoxInput
			// 
			this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.errorProvider1.SetError(this.textBoxInput, "You must enter something!");
			this.errorProvider1.SetIconPadding(this.textBoxInput, 1);
			this.textBoxInput.Location = new System.Drawing.Point(8, 16);
			this.textBoxInput.Name = "textBoxInput";
			this.textBoxInput.Size = new System.Drawing.Size(240, 20);
			this.textBoxInput.TabIndex = 0;
			this.textBoxInput.Text = "";
			this.textBoxInput.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxInput_Validating);
			this.textBoxInput.Validated += new System.EventHandler(this.textBoxInput_Validated);
			this.textBoxInput.TextChanged += new System.EventHandler(this.textBoxInput_TextChanged);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(136, 48);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(56, 24);
			this.buttonOK.StartColor = System.Drawing.Color.LightGray;
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonCancel.CausesValidation = false;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(200, 48);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(56, 24);
			this.buttonCancel.StartColor = System.Drawing.Color.LightGray;
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// InputPrompt
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(272, 78);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.textBoxInput});
			this.Dialog = true;
			this.MinimumSize = new System.Drawing.Size(0, 112);
			this.Name = "InputPrompt";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Input Prompt";
			this.TitleVisible = true;
			this.Load += new System.EventHandler(this.InputPrompt_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.InputPrompt_Paint);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Visible = false;
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Visible = false;
		}

		private void textBoxInput_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (textBoxInput.Text.Length == 0)
			{
				e.Cancel = true;
				this.errorProvider1.SetError(textBoxInput, "You must enter something!");
			}
		}

		private void textBoxInput_Validated(object sender, System.EventArgs e)
		{
			this.errorProvider1.SetError(textBoxInput, "");
		}

		private void InputPrompt_Load(object sender, System.EventArgs e)
		{
		
		}

		private void textBoxInput_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void InputPrompt_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);
		}

		public string Value
		{
			get { return textBoxInput.Text; }
			set { textBoxInput.Text = value; }
		}

	}
}

