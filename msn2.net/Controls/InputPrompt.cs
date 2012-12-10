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
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.SuspendLayout();
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
			this.textBoxInput.TabIndex = 1;
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
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(200, 48);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(56, 24);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// InputPrompt
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(272, 86);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.textBoxInput});
			this.Name = "InputPrompt";
			this.Load += new System.EventHandler(this.InputPrompt_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
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

		public string Value
		{
			get { return textBoxInput.Text; }
			set { textBoxInput.Text = value; }
		}

	}
}

