using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.Linq;
using System.Net.Mail;

namespace CamLib
{
    public class CamAlertManager
    {
        public void InsertAlert(string fileName)
        {
            using (CamDataDataContext data = new CamDataDataContext())
            {
                int id = (from d in data.Alerts
                          where d.Filename == Path.GetFileName(fileName)
                          select d.Id).FirstOrDefault();

                if (id == 0)
                {
                    string name = Path.GetFileName(fileName);

                    // format: do-not-reply@logitech.com Driveway - Home - 2012-06-18 12.34 pm.jpg
                    string dateStamp = name.Substring(name.IndexOf("201")).Trim().Replace(".jpg", "").Replace(".", ":");

                    using (MemoryStream ms = new MemoryStream())
                    {
                        Alert alert = new Alert
                        {
                            Filename = Path.GetFileName(fileName),
                            Timestamp = DateTime.Parse(dateStamp),
                            ReceiveTime = DateTime.Now
                        };

                        data.Alerts.InsertOnSubmit(alert);
                    }

                    data.SubmitChanges();
                }
            }
        }

        public Alert GetAlertByFilename(string fileName)
        {
            Alert alert = null;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                alert = data.Alerts.FirstOrDefault(i => i.Filename == fileName);
            }

            return alert;
        }

        public Alert GetAlert(int id)
        {
            Alert alert = null;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                alert = data.Alerts.FirstOrDefault(i => i.Id == id);
            }

            return alert;
        }

        public int GetNextAlertId(Alert alert)
        {
            int nextId = 0;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                nextId = (from d in data.Alerts
                          where d.Timestamp > alert.Timestamp
                          orderby d.Timestamp
                          select d.Id).FirstOrDefault();
            }

            return nextId;
        }

        public int GetPreviousAlertId(Alert alert)
        {
            int previousId = 0;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                previousId = (from d in data.Alerts
                          where d.Timestamp < alert.Timestamp
                            && d.Timestamp > DateTime.Now.Date.AddDays(-7)
                          orderby d.Timestamp descending
                          select d.Id).FirstOrDefault();
            }

            return previousId;
        }

        public List<Alert> GetAlertsSinceDate(DateTime date)
        {
            List<Alert> alerts = null;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                alerts = data.Alerts.Where(i => i.Timestamp > date).ToList();
            }

            return alerts;
        }
    }
}
