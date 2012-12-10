using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using msn2.net.QueuePlayer.Server;
using msn2.net.QueuePlayer.Shared;

namespace msn2.net.QueuePlayer.ServerHost
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ServerHostDialog : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
	//		private HttpChannel channel = null;
		private TcpChannel channel = null;
		private System.Windows.Forms.TextBox textBox1;
		private MediaServer server = null;

		/// <summary>
		/// Default constructor
		/// </summary>
		public ServerHostDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//channel = new HttpChannel(7777);
			channel = new TcpChannel(777);
			ChannelServices.RegisterChannel(channel);
			RemotingConfiguration.ApplicationName = "RemotingMedia";
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(MediaServer), 
				"MyMedia", WellKnownObjectMode.Singleton);
						
			server = new MediaServer();
			//server.LogEvent += new LogEventHandler(Server_LogEvent);
			
//			this.Visible = true;
//			this.TopMost = true;
			this.Left = Screen.PrimaryScreen.WorkingArea.Right  - this.Width;
			this.Top  = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
//			RemotingConfiguration.Configure("UMServerHost.exe.config");

			this.Visible = false;
		}

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ServerHostDialog));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenu = this.contextMenu1;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "Server Host Control";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "E&xit Server Host";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(242, 88);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			// 
			// ServerHostDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(242, 88);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBox1});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(10, 10);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ServerHostDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "QueuePlayer Server";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.Load += new System.EventHandler(this.ServerHostDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			ServerHostDialog dialog = new ServerHostDialog();

			Application.Run(dialog);

			Environment.Exit(0);
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			notifyIcon1.Visible = false;

			if (server != null)
			{
				server.Shutdown();
				server = null;
			}
			this.Close();
		}

		private void ServerHostDialog_Load(object sender, System.EventArgs e)
		{
			this.Visible = false;
		}

		private void Server_LogEvent(object sender, LogEventArgs e)
		{
			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e;
			this.Invoke(
				new LogEventDelegate(this.LogEvent), eventArgs);
		}

		public delegate void LogEventDelegate(LogEventArgs e);

		private void LogEvent(LogEventArgs e)
		{
			textBox1.Text += e.Function + ": " + e.Message + "\n";
			textBox1.SelectionStart = textBox1.Text.Length-1;
			textBox1.ScrollToCaret();
		}

		private void notifyIcon1_Click(object sender, System.EventArgs e)
		{
			this.Visible = !this.Visible;
		}


	}
}
