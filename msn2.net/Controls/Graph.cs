using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for Graph.
	/// </summary>
	public class Graph : System.Windows.Forms.UserControl
	{
		#region Declares

		private System.ComponentModel.Container components = null;
		private ArrayList graphItems = new ArrayList();

		#endregion
		
		#region Constructor

		public Graph()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
			// 
			// Graph
			// 
			this.Name = "Graph";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Graph_Paint);

		}
		#endregion

		#region GraphItems

		public void AddGraphItem(LineGraph g)
		{
            graphItems.Add(g);
			g.LineGraphUpdate += new LineGraphUpdateDelegate(LineGraphUpdate);
			this.Refresh();
		}

		public void LineGraphUpdate(object sender, EventArgs e)
		{
			this.Refresh();
		}

		#endregion
        
		#region Paint

		private void Graph_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);

			// Draw horizontal lines
			using (Pen pen = new Pen(new SolidBrush(Color.DarkGray)))
			{
				for (int i = 0; i < 5; i++)
				{
					int height = Convert.ToInt32( (decimal) this.Height / (decimal) 5.0 * (decimal) i );
					e.Graphics.DrawLine(pen, 0, height, this.Width, height);
				}
			}

			foreach (LineGraph g in graphItems)
			{
				Point[] points = new Point[g.History.Length];

				// Create the array of points to draw
				for (int x = 0; x < g.History.Length; x++)
				{
					// offset for moving pointer
					int itemPosition = (x + g.currentPointer) % g.History.Length;
					points[x].X = Convert.ToInt32(((decimal) x / (decimal) g.History.Length) * (decimal) this.Width);
					points[x].Y = this.Height - Convert.ToInt32(( (decimal) g.History[x] / (decimal) g.maxGraphValue) * (decimal) this.Height);
				}

				// Draw the actual line
				using (Pen pen = new Pen(new SolidBrush(g.color)))
				{
					e.Graphics.DrawCurve(pen, points);
				}
			}
		}

		#endregion

	}

	public class LineGraph
	{
		#region Declares

		public int currentPointer = 0;
		private int[] history;
		public int maxGraphValue = 100;
		public Color color;

		#endregion

		public event LineGraphUpdateDelegate LineGraphUpdate;

		public LineGraph(int numberOfPoints, int maxValue, Color color)
		{
			history					= new int[numberOfPoints];
			maxGraphValue			= maxValue;
			this.color				= color;
		}

		public void AddToHistory(int newValue)
		{
			// Set the value in the graph
			history[currentPointer] = newValue;

			// Update clients
			if (LineGraphUpdate != null)
				LineGraphUpdate(this, EventArgs.Empty);
            
			// Adjust currentPointer to next position
			currentPointer++;
			if (currentPointer == history.Length)
				currentPointer = 0;
		}

		public int[] History
		{
			get { return history; }
		}

	}

	public delegate void LineGraphUpdateDelegate(object sender, EventArgs e);

}
