#region Using directives

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using msn2.net.Pictures.Controls.TerraServer;
using System.IO;
using System.Threading;

#endregion

namespace msn2.net.Pictures.Controls.Mapping
{
    public class TopoMapReader
    {
        private TerraService service;

        public event ImageReadEventHandler ImageRead;
        public event MapInfoReadEventHandler MapInfoRead;

        public TopoMapReader()
        {
            service = new TerraService();
        }

        public static LonLatPt CreateLonLatPt(double longitude, double lattitude)
        {
            LonLatPt pt = new LonLatPt();
            pt.Lon = longitude;
            pt.Lat = lattitude;
            return pt;
        }

        public void GetMap(LonLatPt upperLeft, LonLatPt lowerRight, Theme theme, Scale scale, Guid requestId)
        {
            AreaBoundingBox box = GetRegion(upperLeft, lowerRight, theme, scale);

            TileId centerTileId = box.Center.TileMeta.Id;

            int tilesWide = box.NorthEast.TileMeta.Id.X - box.NorthWest.TileMeta.Id.X;
            int tilesTall = box.NorthWest.TileMeta.Id.Y - box.SouthWest.TileMeta.Id.Y;

            if (this.MapInfoRead != null)
            {
                this.MapInfoRead(this, new MapInfoReadEventArgs(tilesTall, tilesWide));
            }
            int x = box.NorthWest.TileMeta.Id.X;
            int y = box.NorthWest.TileMeta.Id.Y;
            for (int xOffset = 0; xOffset < tilesWide; xOffset++)
            {
                for (int yOffset = 0; yOffset < tilesTall; yOffset++)
                {
                    TileId tileId = this.CreateTileId(x + xOffset, y + yOffset, centerTileId);
                    StartGetFileThread(new TileRequestData(tileId, xOffset, yOffset, requestId));
                }
            }
        }

        private TileId CreateTileId(int x, int y, TileId sourceTile)
        {
            return this.CreateTileId(x, y, sourceTile.Scene, sourceTile.Theme, sourceTile.Scale);
        }

        private TileId CreateTileId(int x, int y, int scene, Theme theme, Scale scale)
        {
            TileId tileId = new TileId();
            tileId.X = x;
            tileId.Y = y;
            tileId.Scale = scale;
            tileId.Scene = scene;
            tileId.Theme = theme;
            return tileId;
        }

        private void StartGetFileThread(TileRequestData tileData)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetTileThread), tileData);
        }

        private void GetTileThread(object oTileData)
        {
            TileRequestData tileData = (TileRequestData) oTileData;

            Image image = GetTile(tileData.TileId.Theme, tileData.TileId.Scale, tileData.TileId.Scene,
                tileData.TileId.X, tileData.TileId.Y);

            if (this.ImageRead != null)
            {
                ImageReadEventArgs e = new ImageReadEventArgs(image, tileData);
                this.ImageRead(this, e);
            }
        }

        private AreaBoundingBox GetRegion(LonLatPt upperLeft, LonLatPt lowerRight, 
            Theme theme, Scale scale)
        {
            AreaBoundingBox abb = service.GetAreaFromRect(upperLeft,
                   lowerRight, theme, scale);

            return abb;
        }

        private Image GetTile(Theme theme, Scale scale, int scene, int x, int y)
        { 
            TileId tileId = new TileId();
            tileId.Scale = scale;
            tileId.Theme = theme;
            tileId.Scene = scene;
            tileId.X = x;
            tileId.Y = y;
            byte [] imageBytes = service.GetTile(tileId);

            MemoryStream ms = new MemoryStream(imageBytes);
            Image image = Image.FromStream(ms);
            ms.Close();

            return image;
        }
    }

    public class TileRequestData
    {
        private TileId tileId;
        public TileId TileId
        {
            get
            {
                return tileId;
            }

            set
            {
                tileId = value;
            }
        }

        private int xOffset;
        public int XOffset
        {
            get
            {
                return xOffset;
            }

            set
            {
                xOffset = value;
            }
        }

        private int yOffset;
        public int YOffset
        {
            get
            {
                return yOffset;
            }

            set
            {
                yOffset = value;
            }
        }

        private Guid requestId;
        public Guid RequestId
        {
            get
            {
                return requestId;
            }

            set
            {
                requestId = value;
            }
        }


        public TileRequestData(TileId tileId, int xOffset, int yOffset, Guid requestId)
        {
            this.tileId = tileId;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.requestId = requestId;
        }
    }

    public delegate void ImageReadEventHandler(object sender, ImageReadEventArgs e);
    public delegate void MapInfoReadEventHandler(object sender, MapInfoReadEventArgs e);

    public class MapInfoReadEventArgs : EventArgs
    {
        private int tilesTall;
        public int TilesTall
        {
            get
            {
                return tilesTall;
            }

            set
            {
                tilesTall = value;
            }
        }

        private int tilesWide;
        public int TilesWide
        {
            get
            {
                return tilesWide;
            }

            set
            {
                tilesWide = value;
            }
        }


        public MapInfoReadEventArgs(int tilesTall, int tilesWide)
        {
            this.tilesTall = tilesTall;
            this.tilesWide = tilesWide;
        }
    }

    public class ImageReadEventArgs : EventArgs
    {
        private Image image;
        public Image Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }

        private TileRequestData tileRequestData;
        public TileRequestData TileRequestData
        {
            get
            {
                return tileRequestData;
            }

            set
            {
                tileRequestData = value;
            }
        }


        public ImageReadEventArgs(Image image, TileRequestData tileRequestData)
        {
            this.image = image;
            this.tileRequestData = tileRequestData;
        }
    }
}
