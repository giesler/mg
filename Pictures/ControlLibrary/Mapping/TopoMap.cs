#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using msn2.net.Pictures.Controls.TerraServer;
using System.Drawing;

#endregion

namespace msn2.net.Pictures.Controls.Mapping
{
    public class TopoMap: Panel
    {
        private TopoMapReader topoMapReader;
        private Guid lastRequestId;
        private List<ImageReadEventArgs> imageData;
        private MapInfoReadEventArgs mapInfo;

        public TopoMap()
        {
            this.imageData = new List<ImageReadEventArgs>();

            this.topoMapReader = new TopoMapReader();
            this.topoMapReader.ImageRead += new ImageReadEventHandler(topoMapReader_ImageRead);
            this.topoMapReader.MapInfoRead += new MapInfoReadEventHandler(this.MapInfoRead);

            this.Paint += new PaintEventHandler(this.OnPaint);
            this.Resize += new EventHandler(this.OnResize);
        }

        public void LoadMap()
        {
            LonLatPt upperLeft = TopoMapReader.CreateLonLatPt(-122.2601167, 47.7116);
            LonLatPt lowerRight = TopoMapReader.CreateLonLatPt(-122.11857, 47.622516667);

            Guid reqId = Guid.NewGuid();
            this.lastRequestId = reqId;

            this.topoMapReader.GetMap(upperLeft, lowerRight, Theme.Topo, TerraServer.Scale.Scale16m, reqId);
        }

        void topoMapReader_ImageRead(object sender, ImageReadEventArgs e)
        {
            if (this.InvokeRequired)
            {
                object[] args = new object[] { sender, e };
                this.BeginInvoke(new ImageReadEventHandler(this.topoMapReader_ImageRead), args);
            }
            else
            {
                // Invalidate tile area
                int tileWidth = e.Image.Width;
                int tileHeight = e.Image.Height;

                int xStart = e.TileRequestData.XOffset * tileWidth;
                int yStart = e.TileRequestData.YOffset * tileHeight;

                Rectangle rect = new Rectangle(xStart, yStart, tileWidth, tileHeight);
                this.Invalidate(rect, true);

                this.imageData.Add(e);
            }
        }

        void OnPaint(object sender, PaintEventArgs e)
        {
            if (this.mapInfo != null)
            {
                int scaleWidth = this.Width / this.mapInfo.TilesWide;
                int scaleHeight = this.Height / this.mapInfo.TilesTall;

                if (scaleWidth > scaleHeight)
                {
                    scaleWidth = scaleHeight;
                }
                else
                {
                    scaleHeight = scaleWidth;
                }

                for (int i = 0; i < this.imageData.Count; i++)
                {
                    ImageReadEventArgs imageArgs = this.imageData[i];

                    int tileWidth = imageArgs.Image.Width;
                    int tileHeight = imageArgs.Image.Height;

                    int xStart = imageArgs.TileRequestData.XOffset * scaleWidth;
                    int yStart = (this.mapInfo.TilesTall - 1 - imageArgs.TileRequestData.YOffset) * scaleHeight;

                    Point point = new Point(xStart, yStart);
                    e.Graphics.DrawImage(imageArgs.Image, xStart, yStart, scaleWidth, scaleHeight);

//                Rectangle rect = new Rectangle(xStart, yStart, tileWidth, tileHeight);
//                Font font = new Font("Arial", 10, FontStyle.Bold);
//                string s = string.Format("x: {0}, y: {1}", imageArgs.TileRequestData.TileId.X,
//                    imageArgs.TileRequestData.TileId.Y);
//                SizeF size = e.Graphics.MeasureString(s, font);
//                using (Brush brush = new SolidBrush(Color.White))
//                {
//                    e.Graphics.FillRectangle(brush, xStart, yStart, size.Width, size.Height);
//                }
//                using (Pen pen = new Pen(Color.Red))
//                {
//                    e.Graphics.DrawRectangle(pen, rect);
//                }
//                using (Brush brush = new SolidBrush(Color.Red))
//                {
//                    e.Graphics.DrawString(s, font, brush, rect);
//                }
                }
            }

        }

        void MapInfoRead(object sender, MapInfoReadEventArgs e)
        {
            this.mapInfo = e;
        }

        void OnResize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
