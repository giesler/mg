using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using msn2.net.Controls;

namespace msn2.net.ProjectF
{
	/// <summary>
	/// Summary description for ShellFormListItem.
	/// </summary>
	public class ShellFormListItem : System.Windows.Forms.UserControl
	{
		#region Declares

		private msn2.net.Controls.ShellButton shellButton1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Button buttonHide;
		private ShellForm shellForm = null;

		#endregion

		#region Constructors

		public ShellFormListItem()
		{
			InitializeComponent();
		}

		public ShellFormListItem(ShellForm form)
		{
			InitializeComponent();

			this.shellForm = form;
			shellForm.VisibleChanged	+= new EventHandler(Page_VisibleChanged);
			shellForm.TextChanged		+= new EventHandler(Page_TextChanged);
			shellForm.AllowUnload		= false;
			this.shellButton1.Alignment	= StringAlignment.Near;
			this.shellButton1.Text		= shellForm.Text;
		}

		#endregion

		#region Event Handlers

		private void Page_VisibleChanged(object sender, EventArgs e)
		{
			if (shellForm.Visible)
			{
				this.shellButton1.StartColor	= Color.DarkGray;
				this.shellButton1.EndColor		= Color.Gray;
				this.shellButton1.ForeColor		= Color.White;
			}
			else
			{
				this.shellButton1.StartColor	= Color.LightGray;
				this.shellButton1.EndColor		= Color.Empty;
				this.shellButton1.ForeColor		= Color.Black;
			}
		}

		private void Page_TextChanged(object sender, EventArgs e)
		{
			this.shellButton1.Text = shellForm.Text;
		}

		private void shellButton1_Click(object sender, System.EventArgs e)
		{
			shellForm.Visible = !shellForm.Visible;
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
			this.shellButton1 = new msn2.net.Controls.ShellButton();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.buttonHide = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// shellButton1
			// 
			this.shellButton1.Alignment = System.Drawing.StringAlignment.Center;
			this.shellButton1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.shellButton1.EndColor = System.Drawing.Color.Empty;
			this.shellButton1.Font = new System.Drawing.Font("Tahoma", 8F);
			this.shellButton1.Location = new System.Drawing.Point(16, 0);
			this.shellButton1.Name = "shellButton1";
			this.shellButton1.Size = new System.Drawing.Size(160, 16);
			this.shellButton1.StartColor = System.Drawing.Color.LightGray;
			this.shellButton1.TabIndex = 0;
			this.shellButton1.Text = "Form Name";
			this.shellButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.shellButton1.Click += new System.EventHandler(this.shellButton1_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Location = new System.Drawing.Point(176, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(24, 16);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(16, 16);
			this.pictureBox2.TabIndex = 2;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
			// 
			// buttonHide
			// 
			this.buttonHide.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonHide.BackColor = System.Drawing.Color.Transparent;
			this.buttonHide.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonHide.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonHide.ForeColor = System.Drawing.Color.Black;
			this.buttonHide.Location = new System.Drawing.Point(184, 1);
			this.buttonHide.Name = "buttonHide";
			this.buttonHide.Size = new System.Drawing.Size(18, 14);
			this.buttonHide.TabIndex = 6;
			this.buttonHide.Text = "X";
			this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
			// 
			// ShellFormListItem
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonHide,
																		  this.pictureBox2,
																		  this.pictureBox1,
																		  this.shellButton1});
			this.Name = "ShellFormListItem";
			this.Size = new System.Drawing.Size(200, 16);
			this.ResumeLayout(false);

		}
		#endregion

		#region Paint
		private void pictureBox2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);
			e.Graphics.DrawIcon(this.ShellForm.Icon, new Rectangle(0, 0, 16, 16));
		}
		#endregion

		private void buttonHide_Click(object sender, System.EventArgs e)
		{
			shellForm.Dispose();
		}

		#region Properties

		public msn2.net.Controls.ShellForm ShellForm
		{
			get
			{
				return this.shellForm;
			}
		}

		#endregion
	}
}
