using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using msn2.net.QueuePlayer.Server;
using System.Configuration;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for ConnectionDialog.
	/// </summary>
	public class ConnectionDialog : msn2.net.Controls.ShellForm
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
		private System.Windows.Forms.Button buttonCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButtonInProcess;
		private System.Windows.Forms.RadioButton radioButtonLocal;
		private System.Windows.Forms.RadioButton radioButtonRemote;
		private System.Windows.Forms.TextBox textBoxHost;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.CheckBox checkBoxPlayLocally;
		private string hostName = "";

		public ConnectionDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

#if DEBUG
			radioButtonInProcess.Checked = true;
			buttonConnect_Click(this, EventArgs.Empty);
#endif
			string server = ConfigurationSettings.AppSettings["server"];
			if (server != null)
			{
				textBoxHost.Text = server;

//				ChannelServices.RegisterChannel(new TcpChannel(0));
//				RemotingConfiguration.RegisterWellKnownClientType(
//					typeof(MediaServer),
//					"tcp://" + textBoxHost.Text + ":777/RemotingMedia/MyMedia");
			}
			else
			{
//				textBoxHost.Text = "localhost";
			}


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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ConnectionDialog));
			this.buttonConnect = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButtonInProcess = new System.Windows.Forms.RadioButton();
			this.radioButtonLocal = new System.Windows.Forms.RadioButton();
			this.radioButtonRemote = new System.Windows.Forms.RadioButton();
			this.textBoxHost = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkBoxPlayLocally = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonConnect
			// 
			this.buttonConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonConnect.Location = new System.Drawing.Point(112, 184);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.TabIndex = 5;
			this.buttonConnect.Text = "&Connect";
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(192, 184);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.radioButtonInProcess,
																					this.radioButtonLocal,
																					this.radioButtonRemote,
																					this.textBoxHost,
																					this.label1});
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(272, 128);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Connection Options ";
			// 
			// radioButtonInProcess
			// 
			this.radioButtonInProcess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButtonInProcess.Location = new System.Drawing.Point(8, 96);
			this.radioButtonInProcess.Name = "radioButtonInProcess";
			this.radioButtonInProcess.Size = new System.Drawing.Size(248, 24);
			this.radioButtonInProcess.TabIndex = 13;
			this.radioButtonInProcess.Text = "Start a new server in &process";
			// 
			// radioButtonLocal
			// 
			this.radioButtonLocal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButtonLocal.Location = new System.Drawing.Point(8, 72);
			this.radioButtonLocal.Name = "radioButtonLocal";
			this.radioButtonLocal.Size = new System.Drawing.Size(248, 24);
			this.radioButtonLocal.TabIndex = 11;
			this.radioButtonLocal.Text = "&Start a new server running locally";
			// 
			// radioButtonRemote
			// 
			this.radioButtonRemote.Checked = true;
			this.radioButtonRemote.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButtonRemote.Location = new System.Drawing.Point(8, 24);
			this.radioButtonRemote.Name = "radioButtonRemote";
			this.radioButtonRemote.Size = new System.Drawing.Size(248, 24);
			this.radioButtonRemote.TabIndex = 8;
			this.radioButtonRemote.TabStop = true;
			this.radioButtonRemote.Text = "Connect to a &remote server";
			// 
			// textBoxHost
			// 
			this.textBoxHost.Location = new System.Drawing.Point(80, 48);
			this.textBoxHost.Name = "textBoxHost";
			this.textBoxHost.Size = new System.Drawing.Size(144, 20);
			this.textBoxHost.TabIndex = 10;
			this.textBoxHost.Text = "chef";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(32, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 9;
			this.label1.Text = "Server:";
			// 
			// checkBoxPlayLocally
			// 
			this.checkBoxPlayLocally.Location = new System.Drawing.Point(16, 144);
			this.checkBoxPlayLocally.Name = "checkBoxPlayLocally";
			this.checkBoxPlayLocally.Size = new System.Drawing.Size(232, 24);
			this.checkBoxPlayLocally.TabIndex = 9;
			this.checkBoxPlayLocally.Text = "Play media locally";
			// 
			// ConnectionDialog
			// 
			this.AcceptButton = this.buttonConnect;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(290, 216);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.checkBoxPlayLocally,
																		  this.groupBox1,
																		  this.buttonCancel,
																		  this.buttonConnect});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "QueuePlayer Connection";
			this.Load += new System.EventHandler(this.ConnectionDialog_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonConnect_Click(object sender, System.EventArgs e)
		{
			msn2.net.Controls.Status status = new msn2.net.Controls.Status("Loading...");

			// if we need to start the server locally, do so
			if (radioButtonLocal.Checked) 
			{
				string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf(Path.DirectorySeparatorChar));
                status.Message = "Starting local server...";
				textBoxHost.Text = "localhost";
				System.Diagnostics.Process p = new System.Diagnostics.Process();
				p.StartInfo = new System.Diagnostics.ProcessStartInfo(path + @"\msn2.net.QueuePlayer.ServerHost.exe");
				p.Start();
				p.WaitForInputIdle(5000);
			}

			if (!radioButtonInProcess.Checked) 
			{
				status.Message = "Starting server...";
				hostName = textBoxHost.Text;

				ChannelServices.RegisterChannel(new TcpChannel(0));
				RemotingConfiguration.RegisterWellKnownClientType(
					typeof(MediaServer),
					"tcp://" + textBoxHost.Text + ":777/RemotingMedia/MyMedia");
			}
			else
			{
                hostName = "in process server";
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

		private void ConnectionDialog_Load(object sender, System.EventArgs e)
		{
#if DEBUG
			buttonConnect_Click(this, e);
#endif
		}
	}
}
