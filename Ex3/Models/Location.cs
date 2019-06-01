using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Ex3.Models
{
    public class Location
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Rudder { get; set; }
        public double Throttle { get; set; }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Location");
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Rudder", this.Rudder.ToString());
            writer.WriteElementString("Throttle", this.Throttle.ToString());
            writer.WriteEndElement();
        }
        public void addYourselfToXml(XDocument xDocument)
        {
            XElement root = xDocument.Element("Locations");
            IEnumerable<XElement> rows = root.Descendants("Location");
            XElement firstRow = rows.Last();
            firstRow.AddAfterSelf(
               new XElement("Location",
               new XElement("Lat", Lat.ToString()),
               new XElement("Lon", Lon.ToString()),
               new XElement("Rudder", Rudder.ToString()),
               new XElement("Throttle", Throttle.ToString())));

            /* XElement root = xDocument.Element("Locations");
             XElement son = new XElement("Location");
             son.Add(new XElement("Lat", Lat.ToString()));
             son.Add(new XElement("Lon", Lon.ToString()));
             son.Add(new XElement("Rudder", Rudder.ToString()));
             son.Add(new XElement("Throttle", Lat.ToString()));
             root.Add(son);*/

        }
    }
}