using System;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for WebBrowserTitleBarControl.
	/// </summary>
	public class TitleBarControl: System.Windows.Forms.ButtonBase
	{
		#region Constructors

		public TitleBarControl(string name, string text, EventHandler onClick)
		{			
			InternalConstructor(name, text, onClick, null);
		}

		public TitleBarControl(string name, EventHandler onClick, PaintEventHandler onPaint)
		{
			InternalConstructor(name, "", onClick, onPaint);
		}

		private void InternalConstructor(string name, string text, EventHandler onClick, PaintEventHandler onPaint)
		{
			this.BackColor		= System.Drawing.Color.Transparent;
			this.FlatStyle		= System.Windows.Forms.FlatStyle.Popup;
			this.Font			= Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Size			= new System.Drawing.Size(14, 14);
			this.Name			= name;
			this.Text			= text;
			if (onClick != null)
				this.Click		+= onClick;
			if (onPaint != null)
				this.Paint		+= onPaint;
		}

		#endregion
	}
}
