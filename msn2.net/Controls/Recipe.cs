using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace msn2.net.Controls
{
	public class Recipe : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxUrl;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxIngredients;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxDirections;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListView listViewNotes;
		private System.ComponentModel.IContainer components = null;

		public Recipe()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
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
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.textBoxUrl = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxIngredients = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxDirections = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.listViewNotes = new System.Windows.Forms.ListView();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 5;
			this.label1.Text = "Name:";
			// 
			// textBoxName
			// 
			this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxName.Location = new System.Drawing.Point(80, 24);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(336, 20);
			this.textBoxName.TabIndex = 6;
			this.textBoxName.Text = "";
			// 
			// textBoxUrl
			// 
			this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxUrl.Location = new System.Drawing.Point(80, 48);
			this.textBoxUrl.Name = "textBoxUrl";
			this.textBoxUrl.Size = new System.Drawing.Size(336, 20);
			this.textBoxUrl.TabIndex = 8;
			this.textBoxUrl.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 7;
			this.label2.Text = "URL:";
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxDescription.Location = new System.Drawing.Point(80, 72);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.Size = new System.Drawing.Size(336, 40);
			this.textBoxDescription.TabIndex = 10;
			this.textBoxDescription.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Description:";
			// 
			// textBoxIngredients
			// 
			this.textBoxIngredients.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxIngredients.Location = new System.Drawing.Point(80, 120);
			this.textBoxIngredients.Multiline = true;
			this.textBoxIngredients.Name = "textBoxIngredients";
			this.textBoxIngredients.Size = new System.Drawing.Size(336, 56);
			this.textBoxIngredients.TabIndex = 12;
			this.textBoxIngredients.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 120);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 23);
			this.label4.TabIndex = 11;
			this.label4.Text = "Ingredients:";
			// 
			// textBoxDirections
			// 
			this.textBoxDirections.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxDirections.Location = new System.Drawing.Point(80, 184);
			this.textBoxDirections.Multiline = true;
			this.textBoxDirections.Name = "textBoxDirections";
			this.textBoxDirections.Size = new System.Drawing.Size(336, 88);
			this.textBoxDirections.TabIndex = 14;
			this.textBoxDirections.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 184);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 23);
			this.label5.TabIndex = 13;
			this.label5.Text = "Directions:";
			// 
			// listViewNotes
			// 
			this.listViewNotes.Location = new System.Drawing.Point(8, 280);
			this.listViewNotes.Name = "listViewNotes";
			this.listViewNotes.Size = new System.Drawing.Size(408, 144);
			this.listViewNotes.TabIndex = 15;
			// 
			// Recipe
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(424, 430);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listViewNotes,
																		  this.textBoxDirections,
																		  this.label5,
																		  this.textBoxIngredients,
																		  this.label4,
																		  this.textBoxDescription,
																		  this.label3,
																		  this.textBoxUrl,
																		  this.label2,
																		  this.textBoxName,
																		  this.label1});
			this.Name = "Recipe";
			this.Text = "Recipe";
			this.TitleVisible = true;
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}

