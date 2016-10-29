using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using msn2.net.Configuration;

namespace msn2.net.Controls.Shell
{
	/// <summary>
	/// Summary description for UserControl.
	/// </summary>
	public class UserControl : System.Windows.Forms.UserControl
	{
		#region Declares

		private System.ComponentModel.Container components = null;
		private Data data = null;

		#endregion

		#region Constructor

		public UserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		public UserControl(Data data)
		{
			this.data = data;
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
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Properties

		public Data Data
		{
			get 
			{ 
				return data; 
			}
		}

		#endregion

	}
}
