/*By: Shany-yael Dagan 307894899, Racheli Copperman 315597575  */
using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Net;

/* The main controller*/
namespace Ex3.Controllers
{


    public class FirstController : Controller
    {
        private static int saveIt = 0;
        private static XmlTextReader reader = null;
        private static string fileName;
        private static int x = -180;
        private static int y = -90;

        [HttpGet]
        public ActionResult display(string ip, int port, int time, int total, string name)
        {
            Session["save"] = 0;
            saveIt = 0;
            IPAddress ipAdd;


            if (!IPAddress.TryParse(ip, out ipAdd))
            {
                /* If we couldnt read the ip, then go to 4'th mission - read the data from the xml file */
                fileName = ip;
                Session["time"] = port;
                string path2 = Server.MapPath("~/App_Data/" + fileName + ".xml");
                reader = new XmlTextReader(path2);
                return View("load");
            }


            InfoModel.Instance.ip = ip;
            InfoModel.Instance.port = port;


            Session["time"] = time;
            Session["total"] = total;

            return View();
        }

        [HttpGet]
        public ActionResult save(string ip, int port, int time, int total, string name)
        {

            InfoModel.Instance.ip = ip;
            InfoModel.Instance.port = port;
            Session["time"] = time;
            Session["total"] = total;


            saveIt = 1;
            Session["save"] = 1;
            fileName = name;


            return View("display");
        }



        [HttpGet]
        public ActionResult OpenView(string ip, int port, int time)
        {
            return View();
        }




        [HttpPost]
        public string GetLocation()
        {

            // var loc = InfoModel.Instance.Location;
            Random r = new Random();
            var loc = new Location(); //double p1; double p2; if(x==-80) { p1 = -80;p2 = -80; } else { p1 = r.Next(-90, 700); p2 = r.Next(-90, 700); }
            loc.Lon = x; loc.Lat = y; x += 5; y += 5;
            string xmled = ToXml(loc);

            if (saveIt == 1)
            {

                string path2 = Server.MapPath("~/App_Data/" + fileName + ".xml");
                if (!System.IO.File.Exists(path2))
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.NewLineOnAttributes = true;
                    using (XmlWriter xmlWriter = XmlWriter.Create(path2, xmlWriterSettings))
                    {
                        xmlWriter.WriteStartElement("Locations");
                        loc.ToXml(xmlWriter);
                        xmlWriter.WriteEndElement();
                        xmlWriter.Flush();
                        xmlWriter.Close();
                    }
                }
                else
                {
                    XDocument xDocument = XDocument.Load(path2);
                    loc.addYourselfToXml(xDocument);
                    xDocument.Save(path2);


                }
            }
            return xmled;

        }

        private string ToXml(Location loc)
        {
            //Initiate XML stuff
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(stringBuilder, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Locations");
            loc.ToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return stringBuilder.ToString();
        }

        [HttpPost]
        public string GetElement()
        {

            string lat = string.Empty; string lon = string.Empty; string ra = string.Empty; string th = string.Empty;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    if (string.IsNullOrEmpty(lat))
                    {
                        lat = reader.Value;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(lon))
                    {
                        lon = reader.Value;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(th))
                    {
                        th = reader.Value;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(ra))
                    {
                        ra = reader.Value;
                        break;
                    }
                }

            }
            if (string.IsNullOrEmpty(lat))
            {
                reader.Close();
                return "";
            }
            Location location = new Location();
            location.Lat = Convert.ToDouble(lat); location.Lon = Convert.ToDouble(lon);
            location.Rudder = Convert.ToDouble(ra); location.Throttle = Convert.ToDouble(th);
            return ToXml(location);
        }


        /*   [HttpPost]
           public string stop()
           {
               reader.Close();
               return "";

           }*/
        /*     [HttpGet]
     public ActionResult load(string ip, int port, int time)
     {
         string path2 = Server.MapPath("~/App_Data/" + fileName + ".xml");
         reader = new XmlTextReader(path2);
         return View();
     }*/



    }
}