#region Usings

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using msn2.net.QueuePlayer.Shared;

#endregion

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for Advanced.
	/// </summary>
	public class Advanced : msn2.net.Controls.ShellForm
	{
		#region Declares

		private System.Windows.Forms.Label labelOpacity;
		private System.Windows.Forms.TrackBar trackBarOpacity;
		private System.Windows.Forms.Label labelRate;
		private System.Windows.Forms.TrackBar trackBarRate;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TrackBar trackBarBalance;
		private System.Windows.Forms.LinkLabel linkResetBalance;
		private System.Windows.Forms.LinkLabel linkResetRate;
		private System.Windows.Forms.LinkLabel linkResetOpacity;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button buttonMD5;
		private System.Windows.Forms.Button buttonNames;
		private System.Windows.Forms.CheckBox checkBoxLocalPlayer;

		#endregion
		#region Constructor

		public Advanced()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set initial values
			trackBarRate.Value = (int) (QueuePlayerClient.Player.client.mediaServer.Rate * 100);
			trackBarBalance.Value = QueuePlayerClient.Player.client.mediaServer.Balance;

			if (QueuePlayerClient.Player.client.player != null)
			{
				checkBoxLocalPlayer.Checked = true;
			}
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
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.labelOpacity = new System.Windows.Forms.Label();
			this.trackBarOpacity = new System.Windows.Forms.TrackBar();
			this.labelRate = new System.Windows.Forms.Label();
			this.trackBarRate = new System.Windows.Forms.TrackBar();
			this.label4 = new System.Windows.Forms.Label();
			this.trackBarBalance = new System.Windows.Forms.TrackBar();
			this.linkResetBalance = new System.Windows.Forms.LinkLabel();
			this.linkResetRate = new System.Windows.Forms.LinkLabel();
			this.linkResetOpacity = new System.Windows.Forms.LinkLabel();
			this.buttonMD5 = new System.Windows.Forms.Button();
			this.buttonNames = new System.Windows.Forms.Button();
			this.checkBoxLocalPlayer = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBalance)).BeginInit();
			this.SuspendLayout();
			// 
			// labelOpacity
			// 
			this.labelOpacity.Location = new System.Drawing.Point(16, 160);
			this.labelOpacity.Name = "labelOpacity";
			this.labelOpacity.TabIndex = 11;
			this.labelOpacity.Text = "Opacity";
			// 
			// trackBarOpacity
			// 
			this.trackBarOpacity.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.trackBarOpacity.LargeChange = 10;
			this.trackBarOpacity.Location = new System.Drawing.Point(16, 184);
			this.trackBarOpacity.Maximum = 100;
			this.trackBarOpacity.Name = "trackBarOpacity";
			this.trackBarOpacity.Size = new System.Drawing.Size(416, 45);
			this.trackBarOpacity.TabIndex = 10;
			this.trackBarOpacity.TickFrequency = 10;
			this.trackBarOpacity.Value = 100;
			this.trackBarOpacity.Scroll += new System.EventHandler(this.trackBarOpacity_Scroll);
			// 
			// labelRate
			// 
			this.labelRate.Location = new System.Drawing.Point(16, 88);
			this.labelRate.Name = "labelRate";
			this.labelRate.TabIndex = 9;
			this.labelRate.Text = "Rate";
			// 
			// trackBarRate
			// 
			this.trackBarRate.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.trackBarRate.LargeChange = 10;
			this.trackBarRate.Location = new System.Drawing.Point(16, 112);
			this.trackBarRate.Maximum = 200;
			this.trackBarRate.Minimum = 1;
			this.trackBarRate.Name = "trackBarRate";
			this.trackBarRate.Size = new System.Drawing.Size(416, 45);
			this.trackBarRate.TabIndex = 8;
			this.trackBarRate.TickFrequency = 20;
			this.trackBarRate.Value = 1;
			this.trackBarRate.Scroll += new System.EventHandler(this.trackBarRate_Scroll);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 16);
			this.label4.Name = "label4";
			this.label4.TabIndex = 7;
			this.label4.Text = "Balance";
			// 
			// trackBarBalance
			// 
			this.trackBarBalance.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.trackBarBalance.LargeChange = 1000;
			this.trackBarBalance.Location = new System.Drawing.Point(16, 40);
			this.trackBarBalance.Maximum = 5000;
			this.trackBarBalance.Minimum = -5000;
			this.trackBarBalance.Name = "trackBarBalance";
			this.trackBarBalance.Size = new System.Drawing.Size(416, 45);
			this.trackBarBalance.SmallChange = 100;
			this.trackBarBalance.TabIndex = 6;
			this.trackBarBalance.TickFrequency = 1000;
			this.trackBarBalance.Scroll += new System.EventHandler(this.trackBarBalance_Scroll);
			// 
			// linkResetBalance
			// 
			this.linkResetBalance.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.linkResetBalance.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkResetBalance.Location = new System.Drawing.Point(376, 16);
			this.linkResetBalance.Name = "linkResetBalance";
			this.linkResetBalance.Size = new System.Drawing.Size(48, 16);
			this.linkResetBalance.TabIndex = 12;
			this.linkResetBalance.TabStop = true;
			this.linkResetBalance.Text = "Reset";
			this.linkResetBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.linkResetBalance.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkResetBalance_LinkClicked);
			// 
			// linkResetRate
			// 
			this.linkResetRate.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.linkResetRate.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkResetRate.Location = new System.Drawing.Point(376, 96);
			this.linkResetRate.Name = "linkResetRate";
			this.linkResetRate.Size = new System.Drawing.Size(48, 16);
			this.linkResetRate.TabIndex = 13;
			this.linkResetRate.TabStop = true;
			this.linkResetRate.Text = "Reset";
			this.linkResetRate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.linkResetRate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkResetRate_LinkClicked);
			// 
			// linkResetOpacity
			// 
			this.linkResetOpacity.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.linkResetOpacity.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkResetOpacity.Location = new System.Drawing.Point(376, 168);
			this.linkResetOpacity.Name = "linkResetOpacity";
			this.linkResetOpacity.Size = new System.Drawing.Size(48, 16);
			this.linkResetOpacity.TabIndex = 14;
			this.linkResetOpacity.TabStop = true;
			this.linkResetOpacity.Text = "Reset";
			this.linkResetOpacity.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.linkResetOpacity.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkResetOpacity_LinkClicked);
			// 
			// buttonMD5
			// 
			this.buttonMD5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonMD5.Location = new System.Drawing.Point(88, 280);
			this.buttonMD5.Name = "buttonMD5";
			this.buttonMD5.Size = new System.Drawing.Size(112, 32);
			this.buttonMD5.TabIndex = 15;
			this.buttonMD5.Text = "MD5 Files";
			this.buttonMD5.Click += new System.EventHandler(this.buttonMD5_Click);
			// 
			// buttonNames
			// 
			this.buttonNames.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonNames.Location = new System.Drawing.Point(208, 280);
			this.buttonNames.Name = "buttonNames";
			this.buttonNames.Size = new System.Drawing.Size(112, 32);
			this.buttonNames.TabIndex = 16;
			this.buttonNames.Text = "Formatted Names";
			this.buttonNames.Click += new System.EventHandler(this.buttonNames_Click);
			// 
			// checkBoxLocalPlayer
			// 
			this.checkBoxLocalPlayer.Location = new System.Drawing.Point(104, 240);
			this.checkBoxLocalPlayer.Name = "checkBoxLocalPlayer";
			this.checkBoxLocalPlayer.Size = new System.Drawing.Size(208, 16);
			this.checkBoxLocalPlayer.TabIndex = 17;
			this.checkBoxLocalPlayer.Text = "Local Player";
			this.checkBoxLocalPlayer.CheckedChanged += new System.EventHandler(this.checkBoxLocalPlayer_CheckedChanged);
			// 
			// Advanced
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 326);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.checkBoxLocalPlayer,
																		  this.buttonNames,
																		  this.buttonMD5,
																		  this.linkResetOpacity,
																		  this.linkResetRate,
																		  this.linkResetBalance,
																		  this.labelOpacity,
																		  this.trackBarOpacity,
																		  this.labelRate,
																		  this.trackBarRate,
																		  this.label4,
																		  this.trackBarBalance});
			this.Name = "Advanced";
			this.Text = "Advanced";
			((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBalance)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		#region Link Events
		private void linkResetBalance_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			QueuePlayerClient.Player.client.mediaServer.Balance = 0;
		}

		private void linkResetRate_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			QueuePlayerClient.Player.client.mediaServer.Rate = 1.0;
		}

		private void linkResetOpacity_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
            QueuePlayerClient.Player.Opacity = 1;			
			trackBarOpacity.Value = 100;
		}

		#endregion
		#region Trackbar Events
		private void trackBarBalance_Scroll(object sender, System.EventArgs e)
		{
			QueuePlayerClient.Player.SetWaitForMessage();
			QueuePlayerClient.Player.client.mediaServer.Balance = trackBarBalance.Value;
		}

		private void trackBarRate_Scroll(object sender, System.EventArgs e)
		{
			QueuePlayerClient.Player.SetWaitForMessage();
			QueuePlayerClient.Player.client.mediaServer.Rate = ((double)trackBarRate.Value / 100.0);
		}

		private void trackBarOpacity_Scroll(object sender, System.EventArgs e)
		{
			QueuePlayerClient.Player.Opacity = trackBarOpacity.Value/100.0;
		}

		#endregion
		#region RateEvent

		public delegate void RateChangedDelegate(MediaRateEventArgs e);

		public void InvokeRateChanged(MediaRateEventArgs e) 
		{
			trackBarRate.Value = (int) (e.Rate * 100);
			QueuePlayerClient.Player.ClearWaitForMessage();
		}

		#endregion
		#region BalanceEvent

		public delegate void BalanceChangedDelegate(MediaBalanceEventArgs e);

		public void InvokeBalanceChanged(MediaBalanceEventArgs e) 
		{
			trackBarBalance.Value = e.Balance;
			QueuePlayerClient.Player.ClearWaitForMessage();
		}

		#endregion
		#region Button Events
		private void buttonMD5_Click(object sender, System.EventArgs e)
		{
			SqlConnection cn = new SqlConnection(QueuePlayerClient.Player.client.ConnectionString);
			SqlCommand cmd = new SqlCommand("update media set md5=@md5 where mediaid=@mediaid", cn);
			cmd.Parameters.Add("@MD5", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@MediaId", SqlDbType.Int);
            
			string fileShare = QueuePlayerClient.Player.client.mediaServer.ShareDirectory + Path.DirectorySeparatorChar;

			DataView dv = new DataView(QueuePlayerClient.Player.client.dsMedia.Media);
			dv.RowFilter = "MD5 IS NULL";
			int count = dv.Count;
			int i = 0;
			msn2.net.Controls.Status status = new msn2.net.Controls.Status("MD5'ing files...", count);

			cn.Open();
			foreach (DataRowView rowView in dv)
			{
				i++;

				DataSetMedia.MediaRow row = (DataSetMedia.MediaRow) rowView.Row;
				string file = fileShare + row.MediaFile;
				string md5 = MediaUtilities.MD5ToString(MediaUtilities.MD5Hash(file));

                cmd.Parameters["@MediaId"].Value = row.MediaId;
				cmd.Parameters["@MD5"].Value = md5;
				cmd.ExecuteNonQuery();
                
				row.MD5 = md5;

				status.Message = String.Format("MD5'ing ({0}/{1})...", i, count);
				status.Increment(1);
				status.Refresh();
				if (status.Cancel)
					break;
			}
			cn.Close();

			status.Hide();
			status.Dispose();
		}

		private void buttonNames_Click(object sender, System.EventArgs e)
		{
			SqlConnection cn = new SqlConnection(QueuePlayerClient.Player.client.ConnectionString);
			SqlCommand cmd = new SqlCommand("update media set MediaFile=@MediaFile where mediaid=@mediaid", cn);
			cmd.Parameters.Add("@MediaFile", SqlDbType.NVarChar, 500);
			cmd.Parameters.Add("@MediaId", SqlDbType.Int);
            
			string fileShare = QueuePlayerClient.Player.client.mediaServer.ShareDirectory + Path.DirectorySeparatorChar;

			int count = QueuePlayerClient.Player.client.dsMedia.Media.Count;
			int i = 0;
			msn2.net.Controls.Status status = new msn2.net.Controls.Status("Naming files...", count);

			cn.Open();
			foreach (DataSetMedia.MediaRow row in QueuePlayerClient.Player.client.dsMedia.Media)
			{
				i++;

				string file = fileShare + row.MediaFile;
				string correctFile = fileShare + MediaUtilities.NameMediaFile(row);

				if (file != correctFile)
				{
					// make sure target dirs exist
					string dir = correctFile.Substring(0, correctFile.LastIndexOf(Path.DirectorySeparatorChar));
					
					if (!Directory.Exists(dir))
						Directory.CreateDirectory(dir);

					int j = 1;
					while (File.Exists(correctFile))
					{
						correctFile = correctFile.Substring(0, correctFile.LastIndexOf(".")) + " (" + j + ")" + correctFile.Substring(correctFile.LastIndexOf("."));
						j++;
					}
					File.Move(file, correctFile);

					row.MediaFile = correctFile.Substring(fileShare.Length);

					cmd.Parameters["@MediaId"].Value = row.MediaId;
					cmd.Parameters["@MediaFile"].Value = row.MediaFile;
					cmd.ExecuteNonQuery();
					
				}

				status.Message = String.Format("MD5'ing ({0}/{1})...", i, count);
				status.Increment(1);
				status.Refresh();
				if (status.Cancel)
					break;
			}
			cn.Close();

			status.Hide();
			status.Dispose();		
		}

		#endregion
		#region Other events
		private void checkBoxLocalPlayer_CheckedChanged(object sender, System.EventArgs e)
		{
			if (checkBoxLocalPlayer.Checked)
			{
				QueuePlayerClient.Player.client.StartLocalPlayer();
			} 
			else
			{
				QueuePlayerClient.Player.client.StopLocalPlayer();
			}
		}

		#endregion
	}
}
