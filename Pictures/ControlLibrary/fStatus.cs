using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for fStatus.
	/// </summary>
	public class fStatus : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblStatus;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fStatus()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public fStatus(string status)
		{
			InitializeComponent();

			this.StatusText		= status;
		}

		public fStatus(string status, int max) 
		{
			InitializeComponent();

			this.StatusText = status;
			this.Max = max;
			this.Show();
			this.Refresh();

			if (max <= 0) 
				progressBar1.Visible = false;
			
			this.lblStatus.Refresh();
		}

		public fStatus(Form centerOn, string status, int max) 
		{
			InitializeComponent();

            this.StatusText = status;
			this.Max = max;
			this.Show();
			this.Refresh();

			if (centerOn != null) 
			{
				Left = centerOn.Left + (centerOn.Width/2)  - (Width/2);
				Top  = centerOn.Top  + (centerOn.Height/2) - (Height/2);
			}

			if (max <= 0) 
				progressBar1.Visible = false;
			
			this.lblStatus.Refresh();
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

		public void ShowStatusForm() 
		{
			this.Show();
			this.Refresh();
		}

		public int Max 
		{
			get 
			{
				return progressBar1.Maximum;
			}
			set 
			{
                SetMax(value);
            }

        }

        private delegate void SetMaxDelegate(int max);

        private void SetMax(int max)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetMaxDelegate(SetMax), new object[] { max });
                return;
            }

            if (max <= 0)
                progressBar1.Visible = false;
            else
            {
                progressBar1.Visible = true;
            }

            progressBar1.Maximum = max;
            Refresh();
        }

        public int Current
		{
			get 
			{
				return progressBar1.Value;
			}
			set
			{
                SetCurrent(value);
			}
		}

        private delegate void SetCurrentDelegate(int current);

        private void SetCurrent(int current)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SetCurrentDelegate(SetCurrent), new object[] { current });
                return;
            }

            progressBar1.Value = current;
            Refresh();
        }

        public String StatusText 
		{
			set
			{
				if (this.InvokeRequired)
				{
					SetStatusDelegate setDelegate	= new SetStatusDelegate(SetStatus);
					this.BeginInvoke(setDelegate, new object[] {value});
					return;
				}
				SetStatus(value);
			}
		}

		public void HideStatus()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new MethodInvoker(HideStatus));
				return;
			}
			this.Hide();
		}

		private delegate void SetStatusDelegate(string status);

		public void SetStatus(string status)
		{
			lblStatus.Text = status;
			Refresh();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(8, 8);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(320, 23);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Please Wait...";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 40);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(320, 8);
            this.progressBar1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(248, 56);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            // 
            // fStatus
            // 
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(338, 88);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "fStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Please wait...";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
