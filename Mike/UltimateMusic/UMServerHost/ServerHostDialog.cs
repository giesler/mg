using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;

namespace UMServerHost
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ServerHostDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
	//		private HttpChannel channel = null;
		private TcpChannel channel = null;

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
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(UMServer.MediaServer), 
				"MyMedia", WellKnownObjectMode.Singleton);
						
//			RemotingConfiguration.Configure("UMServerHost.exe.config");
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ServerHostDialog));
			this.label1 = new System.Windows.Forms.Label();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(224, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Running server...";
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenu = this.contextMenu1;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "Server Host Control";
			this.notifyIcon1.Visible = true;
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
			// ServerHostDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(258, 48);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point(10, 10);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ServerHostDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Server Host Dialog";
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
			Application.Run(new ServerHostDialog());
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			notifyIcon1.Visible = false;
			Application.Exit();
		}

		private void ServerHostDialog_Load(object sender, System.EventArgs e)
		{
			this.Visible = false;
		}


	}
}
