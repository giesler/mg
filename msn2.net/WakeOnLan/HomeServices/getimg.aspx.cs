﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeServices
{
    public partial class getimg : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Response.Clear();
            Response.ContentType = "image/jpeg";

            string cam = "1";
            if (Request.QueryString["c"] != null)
            {
                cam = Request.QueryString["c"];
            }

            string address = "http://invalid/";
            bool rotate = true;

            if (cam == "1")
            {
                address = "http://192.168.1.212:81/image/cc1";
            }
            else if (cam == "2")
            {
                address = "http://192.168.1.212:81/image/cc2";
            }
            else if (cam == "3")
            {
                rotate = false;
                address = "http://192.168.1.212:81/image/cc3";
            }
            else if (cam == "dw1")
            {
                rotate = false;
                address = "http://192.168.1.212:81/image/dw1";
            }
            else if (cam.ToLower() == "front")
            {
                rotate = false;
                address = "http://192.168.1.212:81/image/Front";
            }
            else if (cam.ToLower() == "side")
            {
                rotate = false;
                address = "http://192.168.1.212:81/image/side";
            }

            if (Request.QueryString["r"] == "1")
            {
                rotate = true;
            }

            int maxHeight = 0;
            if (Request.QueryString["h"] != null)
            {
                maxHeight = int.Parse(Request.QueryString["h"]);
            }

            if (Environment.MachineName.ToLower() == "server0")
            {
                address = address.Replace("192.168.1.212", "127.0.0.1");
            }

            int retries = 3;
            Exception error = null;
            while (retries > 0)
            {
                try
                {
                    string url = string.Format("{0}?time={1}",
                        address, new Random().Next(100000));
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                    req.Method = WebRequestMethods.Http.Get;
                    req.Credentials = new NetworkCredential("admin", "aaaCharlie", "");

                    Trace.Write("Requesting " + url);

                    using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                    {
                        if (response.ContentLength > 0)
                        {
                            using (Stream s = response.GetResponseStream())
                            {
                                using (System.Drawing.Image img = System.Drawing.Image.FromStream(s))
                                {
                                    if (rotate)
                                    {
                                        img.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                    }

                                    if (maxHeight == 0 || maxHeight > img.Height)
                                    {
                                        img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                    else
                                    {
                                        int w = (int)((double)img.Width / (double)img.Height * (double)maxHeight);

                                        using (Bitmap thumb = new Bitmap(img, new Size(w, maxHeight)))
                                        {
                                            thumb.Save(Response.OutputStream, ImageFormat.Jpeg);
                                        }

                                    }

                                    Response.End();
                                }
                            }
                        }
                    }

                    error = null;
                }
                catch (Exception ex)
                {
                    error = ex;
                    retries--;
                    if (retries > 0)
                    {
                        Trace.Write(string.Format("Retries left {0}: {1}", retries, ex.ToString()));
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                }
            }

            if (error != null)
            {
                throw error;
            }
        }
    }
}
        