﻿using System;
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
        object insertLock = new object();
        
        public void AddAlert(DateTime timestamp, string fileName, DateTime receiveTime)
        {
            using (CamDataDataContext data = new CamDataDataContext())
            {
                Alert current = data.Alerts.Where(i => i.Filename == fileName).FirstOrDefault();
                if (current == null)
                {
                    lock (this.insertLock)
                    {
                        current = data.Alerts.Where(i => i.Filename == fileName).FirstOrDefault();
                        if (current == null)
                        {
                            Alert alert = new Alert
                            {
                                Timestamp = timestamp,
                                Filename = fileName,
                                ReceiveTime = receiveTime
                            };
                            data.Alerts.InsertOnSubmit(alert);
                            data.SubmitChanges();
                        }
                    }
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
                var q = data.ExecuteQuery(typeof(Alert), "select * From dbo.Alert with (nolock) where id = " + id.ToString());
                
                

                alert = data.Alerts.FirstOrDefault(i => i.Id == id);
            }

            return alert;
        }

        public int GetNextAlertIdById(int id)
        {
            int nextId = 0;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                nextId = (from d in data.Alerts
                          where d.Id > id
                          orderby d.Id
                          select d.Id).FirstOrDefault();
            }

            return nextId;
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

        public int GetPreviousAlertIdById(int id)
        {
            int previousId = 0;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                previousId = (from d in data.Alerts
                              where d.Id < id
                              orderby d.Id descending
                              select d.Id).FirstOrDefault();
            }

            return previousId;
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

        public List<Alert> GetAlertsBeforeDate(DateTime date)
        {
            List<Alert> alerts = null;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                alerts = data.Alerts.Where(i => i.Timestamp < date).ToList();
            }

            return alerts;
        }

        public List<Alert> GetAlertsOnDate(DateTime date)
        {
            using (CamDataDataContext data = new CamDataDataContext())
            {
                return data.Alerts.Where(i => i.Timestamp > date.Date && i.Timestamp < date.Date.AddDays(1)).ToList();
            }
        }

        public List<Alert> GetAlerts(DateTime dateTimeStartUtc, DateTime dateTimeEndUtc)
        {
            using (CamDataDataContext data = new CamDataDataContext())
            {
                return data.Alerts.Where(i => i.Timestamp > dateTimeStartUtc && i.Timestamp < dateTimeEndUtc).ToList();
            }
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

        public void DeleteAlert(int id)
        {
            using (CamDataDataContext data = new CamDataDataContext())
            {
                Alert alert = data.Alerts.FirstOrDefault(a => a.Id == id);
                if (alert != null)
                {
                    data.Alerts.DeleteOnSubmit(alert);
                    data.SubmitChanges();
                }
            }
        }
    }
}