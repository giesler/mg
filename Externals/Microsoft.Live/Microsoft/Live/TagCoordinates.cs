namespace Microsoft.Live
{
    using System;

    public class TagCoordinates
    {
        private float _Height;
        private float _Width;
        private float _X0;
        private float _Y0;

        public TagCoordinates(float x0, float y0, float width, float height)
        {
            this.X0 = x0;
            this.Y0 = y0;
            this.Width = width;
            this.Height = height;
        }

        public float Height
        {
            get
            {
                return this._Height;
            }
            set
            {
                this._Height = value;
            }
        }

        public float Width
        {
            get
            {
                return this._Width;
            }
            set
            {
                this._Width = value;
            }
        }

        public float X0
        {
            get
            {
                return this._X0;
            }
            set
            {
                this._X0 = value;
            }
        }

        public float Y0
        {
            get
            {
                return this._Y0;
            }
            set
            {
                this._Y0 = value;
            }
        }
    }
}

