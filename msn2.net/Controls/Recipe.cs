using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using msn2.net.Configuration;

namespace msn2.net.Controls
{
	public class Recipe : msn2.net.Controls.ShellForm
	{
		#region Declares

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
		private System.ComponentModel.IContainer components = null;
		private bool dirty = false;
		private bool loaded = false;

		#endregion

		#region Constructors

		public Recipe()
		{
			InitializeComponent();
		}

		public Recipe(msn2.net.Configuration.Data data): base(data)
		{
			InitializeComponent();

			RecipeConfigData recipe = (RecipeConfigData) data.ConfigData;

			this.textBoxName.Text			= data.Text;
			this.textBoxDescription.Text	= recipe.Description;
			this.textBoxDirections.Text		= recipe.Directions;
			this.textBoxIngredients.Text	= recipe.Ingredients;
			this.textBoxUrl.Text			= recipe.Url;

			dirty = false;
			loaded = true;
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
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

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
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
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
			this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
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
			this.textBoxUrl.TextChanged += new System.EventHandler(this.textBoxUrl_TextChanged);
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
			this.textBoxDescription.TextChanged += new System.EventHandler(this.textBoxDescription_TextChanged);
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
			this.textBoxIngredients.TextChanged += new System.EventHandler(this.textBoxIngredients_TextChanged);
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
			this.textBoxDirections.TextChanged += new System.EventHandler(this.textBoxDirections_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 184);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 23);
			this.label5.TabIndex = 13;
			this.label5.Text = "Directions:";
			// 
			// Recipe
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(424, 278);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
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
			this.Deactivate += new System.EventHandler(this.Recipe_Deactivate);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Save

		private void SaveRecipe()
		{
			if (loaded && Data != null)
			{
				RecipeConfigData recipe		= new RecipeConfigData();
				Data.Text					= this.textBoxName.Text;
				recipe.Description			= this.textBoxDirections.Text;
				recipe.Directions			= this.textBoxDirections.Text;
				recipe.Ingredients			= this.textBoxIngredients.Text;
				recipe.Url					= this.textBoxUrl.Text;
				Data.ConfigData				= recipe;

                Data.Save();				
			}
		}

		private void Recipe_Deactivate(object sender, System.EventArgs e)
		{
			if (dirty) 
			{
				SaveRecipe();
				dirty = false;
			}
		}

		#endregion

		#region Textbox mods

		private void textBoxName_TextChanged(object sender, System.EventArgs e)
		{
			dirty = true;
		}

		private void textBoxUrl_TextChanged(object sender, System.EventArgs e)
		{
			dirty = true;
		}

		private void textBoxDescription_TextChanged(object sender, System.EventArgs e)
		{
			dirty = true;
		}

		private void textBoxIngredients_TextChanged(object sender, System.EventArgs e)
		{
			dirty = true;
		}

		private void textBoxDirections_TextChanged(object sender, System.EventArgs e)
		{
			dirty = true;
		}

		#endregion

		#region Add

		public static Data Add(System.Windows.Forms.IWin32Window owner, Data parent)
		{
			InputPrompt p = new InputPrompt("Name the new recipe:");

			if (p.ShowShellDialog(owner) == DialogResult.Cancel)
				return null;

			RecipeConfigData recipeConfigData = new RecipeConfigData();
			return parent.Get(p.Value, recipeConfigData, typeof(RecipeConfigData));
		}

		#endregion

	}

	#region RecipeConfigData

	public class RecipeConfigData: msn2.net.Configuration.ConfigData
	{
		public static string TypeName = "Note";

		private string note;
		private string url;
		private string description;
		private string ingredients;
		private string directions;

		public RecipeConfigData()
		{}

		public RecipeConfigData(string note)
		{
			this.note = note;
		}

		public string Note
		{
			get { return note; }
			set { note = value; }
		}

		public string Url
		{
			get { return url; }
			set { url = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public string Ingredients
		{
			get { return ingredients; }
			set { ingredients = value; }
		}

		public string Directions
		{
			get { return directions; }
			set { directions = value; }
		}

		public override int IconIndex
		{
			get { return 2; }
		}

	}

	#endregion

}

