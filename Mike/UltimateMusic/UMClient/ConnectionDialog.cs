using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;

namespace UMClient
{
	/// <summary>
	/// Summary description for ConnectionDialog.
	/// </summary>
	public class ConnectionDialog : System.Windows.Forms.Form
	{
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new UMPlayer());
		}

		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox textBoxHost;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.RadioButton radioButtonRemote;
		private System.Windows.Forms.RadioButton radioButtonLocal;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton radioButtonLocalRun;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.RadioButton radioButtonInProcess;
		private string hostName = "";

		public ConnectionDialog()
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
			this.buttonConnect = new System.Windows.Forms.Button();
			this.textBoxHost = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.radioButtonRemote = new System.Windows.Forms.RadioButton();
			this.radioButtonLocal = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.radioButtonLocalRun = new System.Windows.Forms.RadioButton();
			this.radioButtonInProcess = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// buttonConnect
			// 
			this.buttonConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonConnect.Location = new System.Drawing.Point(96, 136);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.TabIndex = 5;
			this.buttonConnect.Text = "&Connect";
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// textBoxHost
			// 
			this.textBoxHost.Location = new System.Drawing.Point(88, 32);
			this.textBoxHost.Name = "textBoxHost";
			this.textBoxHost.Size = new System.Drawing.Size(144, 20);
			this.textBoxHost.TabIndex = 2;
			this.textBoxHost.Text = "chef";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(176, 136);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// radioButtonRemote
			// 
			this.radioButtonRemote.Checked = true;
			this.radioButtonRemote.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButtonRemote.Location = new System.Drawing.Point(16, 8);
			this.radioButtonRemote.Name = "radioButtonRemote";
			this.radioButtonRemote.Size = new System.Drawing.Size(248, 24);
			this.radioButtonRemote.TabIndex = 0;
			this.radioButtonRemote.TabStop = true;
			this.radioButtonRemote.Text = "Connect to a &remote server";
			this.radioButtonRemote.CheckedChanged += new System.EventHandler(this.radioButtonRemote_CheckedChanged);
			// 
			// radioButtonLocal
			// 
			this.radioButtonLocal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButtonLocal.Location = new System.Drawing.Point(16, 56);
			this.radioButtonLocal.Name = "radioButtonLocal";
			this.radioButtonLocal.Size = new System.Drawing.Size(248, 24);
			this.radioButtonLocal.TabIndex = 3;
			this.radioButtonLocal.Text = "&Start a new server running locally";
			this.radioButtonLocal.CheckedChanged += new System.EventHandler(this.radioButtonLocal_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(40, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Server:";
			// 
			// radioButtonLocalRun
			// 
			this.radioButtonLocalRun.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButtonLocalRun.Location = new System.Drawing.Point(16, 80);
			this.radioButtonLocalRun.Name = "radioButtonLocalRun";
			this.radioButtonLocalRun.Size = new System.Drawing.Size(248, 24);
			this.radioButtonLocalRun.TabIndex = 4;
			this.radioButtonLocalRun.Text = "Connect to &locally running server";
			this.radioButtonLocalRun.CheckedChanged += new System.EventHandler(this.radioButtonLocalRun_CheckedChanged);
			// 
			// radioButtonInProcess
			// 
			this.radioButtonInProcess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButtonInProcess.Location = new System.Drawing.Point(16, 104);
			this.radioButtonInProcess.Name = "radioButtonInProcess";
			this.radioButtonInProcess.Size = new System.Drawing.Size(248, 24);
			this.radioButtonInProcess.TabIndex = 7;
			this.radioButtonInProcess.Text = "Start a new server in &process";
			// 
			// ConnectionDialog
			// 
			this.AcceptButton = this.buttonConnect;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(266, 168);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.radioButtonInProcess,
																		  this.radioButtonLocalRun,
																		  this.radioButtonLocal,
																		  this.radioButtonRemote,
																		  this.buttonCancel,
																		  this.textBoxHost,
																		  this.label1,
																		  this.buttonConnect});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ultimate Music Connection";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonConnect_Click(object sender, System.EventArgs e)
		{
			Status status = new Status("Loading...");

			// if we need to start the server locally, do so
			if (radioButtonLocal.Checked) 
			{
				string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf(@"\"));
                status.Message = "Starting local server...";
				textBoxHost.Text = "localhost";
				System.Diagnostics.Process p = new System.Diagnostics.Process();
				p.StartInfo = new System.Diagnostics.ProcessStartInfo(path + @"\UMServerHost.exe");
				p.Start();
				p.WaitForInputIdle(5000);
			}

			if (radioButtonLocalRun.Checked || radioButtonInProcess.Checked) 
			{
				textBoxHost.Text = "localhost";
			}
            
			if (!radioButtonInProcess.Checked) 
			{
				status.Message = "Connecting to server...";
				hostName = textBoxHost.Text;

				ChannelServices.RegisterChannel(new TcpChannel(0));
				RemotingConfiguration.RegisterWellKnownClientType(
					typeof(UMServer.MediaServer),
					"tcp://" + textBoxHost.Text + ":777/RemotingMedia/MyMedia");
			}

			this.Visible = false;

			status.Dispose();

			this.Visible = false;

		}

		public string HostName 
		{
			get { return hostName; }
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			Visible = false;
		}

		private void radioButtonLocal_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxHost.Enabled = radioButtonRemote.Checked;
		}

		private void radioButtonRemote_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxHost.Enabled = radioButtonRemote.Checked;
		}

		private void radioButtonLocalRun_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxHost.Enabled = radioButtonRemote.Checked;
		}
	}
}
