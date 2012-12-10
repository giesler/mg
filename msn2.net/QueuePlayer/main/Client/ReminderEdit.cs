using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for ReminderEdit.
	/// </summary>
	public class ReminderEdit : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.ComboBox comboBoxAMPM;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ReminderEdit()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.comboBoxAMPM = new System.Windows.Forms.ComboBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(72, 8);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(248, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(40, 40);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(224, 24);
			this.radioButton1.TabIndex = 2;
			this.radioButton1.Text = "Time:";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(40, 72);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(224, 24);
			this.radioButton2.TabIndex = 3;
			this.radioButton2.Text = "Timer:";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(104, 40);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(80, 20);
			this.textBox2.TabIndex = 4;
			this.textBox2.Text = "";
			// 
			// comboBoxAMPM
			// 
			this.comboBoxAMPM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxAMPM.Items.AddRange(new object[] {
															  "am",
															  "pm"});
			this.comboBoxAMPM.Location = new System.Drawing.Point(192, 40);
			this.comboBoxAMPM.Name = "comboBoxAMPM";
			this.comboBoxAMPM.Size = new System.Drawing.Size(56, 21);
			this.comboBoxAMPM.TabIndex = 5;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(104, 72);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(80, 20);
			this.textBox3.TabIndex = 6;
			this.textBox3.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(192, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 7;
			this.label2.Text = "m or m:ss";
			// 
			// buttonOK
			// 
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(152, 112);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.TabIndex = 8;
			this.buttonOK.Text = "&OK";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(240, 112);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 9;
			this.buttonCancel.Text = "&Cancel";
			// 
			// ReminderEdit
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(328, 142);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.label2,
																		  this.textBox3,
																		  this.comboBoxAMPM,
																		  this.textBox2,
																		  this.radioButton2,
																		  this.radioButton1,
																		  this.textBox1,
																		  this.label1});
			this.Name = "ReminderEdit";
			this.Text = "ReminderEdit";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
