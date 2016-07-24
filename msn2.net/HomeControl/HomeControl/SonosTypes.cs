using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msn2.net
{
    public class SonosPlayingData
    {
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string AlbumArtUri { get; set; }

        public override bool Equals(object obj)
        {
            SonosPlayingData data = obj as SonosPlayingData;
            if (data == null)
            {
                return false;
            }

            return this.Title == data.Title && this.Album == data.Album && this.Artist == data.Artist && this.AlbumArtUri == data.AlbumArtUri;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
