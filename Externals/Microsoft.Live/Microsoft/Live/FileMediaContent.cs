namespace Microsoft.Live
{
    using System;

    public class FileMediaContent
    {
        private int _BitRate;
        private int _Duration;
        private int _FileSize;
        private int _Height;
        private string _Type;
        private int _Width;

        public int BitRate
        {
            get
            {
                return this._BitRate;
            }
            internal set
            {
                this._BitRate = value;
            }
        }

        public int Duration
        {
            get
            {
                return this._Duration;
            }
            internal set
            {
                this._Duration = value;
            }
        }

        public int FileSize
        {
            get
            {
                return this._FileSize;
            }
            internal set
            {
                this._FileSize = value;
            }
        }

        public int Height
        {
            get
            {
                return this._Height;
            }
            internal set
            {
                this._Height = value;
            }
        }

        public string Type
        {
            get
            {
                return this._Type;
            }
            internal set
            {
                this._Type = value;
            }
        }

        public int Width
        {
            get
            {
                return this._Width;
            }
            internal set
            {
                this._Width = value;
            }
        }
    }
}

