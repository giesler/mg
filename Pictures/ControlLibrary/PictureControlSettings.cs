using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public class PictureControlSettings : ApplicationSettingsBase
    {
        public PictureControlSettings()
        {
        }

        [UserScopedSetting()]
        [DefaultSettingValue("200")]
        public int Slideshow_Editor_Left
        {
            get
            {
                return (int)this["Slideshow_Editor_Left"];
            }
            set
            {

                this["Slideshow_Editor_Left"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("200")]
        public int Slideshow_Editor_Top
        {
            get
            {
                return (int)this["Slideshow_Editor_Top"];
            }
            set
            {
                this["Slideshow_Editor_Top"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public bool Slideshow_Editor_IsOpen
        {
            get
            {
                return (bool)this["Slideshow_Editor_IsOpen"];
            }
            set
            {
                this["Slideshow_Editor_IsOpen"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("200")]
        public int Slideshow_People_Left
        {
            get
            {
                return (int)this["Slideshow_People_Left"];
            }
            set
            {
                this["Slideshow_People_Left"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("200")]
        public int Slideshow_People_Top
        {
            get
            {
                return (int)this["Slideshow_People_Top"];
            }
            set
            {
                this["Slideshow_People_Top"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public bool Slideshow_People_IsOpen
        {
            get
            {
                return (bool)this["Slideshow_People_IsOpen"];
            }
            set
            {
                this["Slideshow_People_IsOpen"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("200")]
        public int Slideshow_Group_Left
        {
            get
            {
                return (int)this["Slideshow_Group_Left"];
            }
            set
            {

                this["Slideshow_Group_Left"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("200")]
        public int Slideshow_Group_Top
        {
            get
            {
                return (int)this["Slideshow_Group_Top"];
            }
            set
            {
                this["Slideshow_Group_Top"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public bool Slideshow_Group_IsOpen
        {
            get
            {
                return (bool)this["Slideshow_Group_IsOpen"];
            }
            set
            {
                this["Slideshow_Group_IsOpen"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("200")]
        public int Slideshow_Category_Left
        {
            get
            {
                return (int)this["Slideshow_Category_Left"];
            }
            set
            {
                this["Slideshow_Category_Left"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("200")]
        public int Slideshow_Category_Top
        {
            get
            {
                return (int)this["Slideshow_Category_Top"];
            }
            set
            {
                this["Slideshow_Category_Top"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public bool Slideshow_Category_IsOpen
        {
            get
            {
                return (bool)this["Slideshow_Category_IsOpen"];
            }
            set
            {
                this["Slideshow_Category_IsOpen"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Last_Login_Email
        {
            get
            {
                return (string) this["Last_Login_Email"];
            }
            set
            {
                this["Last_Login_Email"] = value;
            }
        }


        public static int GetSafeLeft(Form form, int suggestedLeft)
        {
            Rectangle formRectange = Screen.FromControl(form).WorkingArea;

            if (suggestedLeft < formRectange.Left)
            {
                return 20;
            }

            if (suggestedLeft > formRectange.Right - form.Width - 20)
            {
                return formRectange.Right - form.Width - 20;
            }

            return suggestedLeft;
        }

        public static int GetSafeTop(Form form, int suggestedTop)
        {
            Rectangle formRectange = Screen.FromControl(form).WorkingArea;

            if (suggestedTop < formRectange.Top)
            {
                return 20;
            }

            if (suggestedTop > formRectange.Bottom - form.Height - 20)
            {
                return formRectange.Bottom - form.Height - 20;
            }

            return suggestedTop;
        }

    }
}
