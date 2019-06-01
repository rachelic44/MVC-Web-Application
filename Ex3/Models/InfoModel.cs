using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace Ex3.Models
{
    public class InfoModel
    {

        private static InfoModel s_instace = null;
        private TcpClient client;
        private BinaryWriter binaryWriter;
        private BinaryReader binaryReader;
        public string ip { get; set; }
        public int port { get; set; }
        private string lastIp;
        private int lastPort;


        public static InfoModel Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new InfoModel();
                }
                return s_instace;
            }
        }



        private bool ConnectionCalled = false;
        private Location location;
        public Location Location
        {
            get
            {
                if (lastIp != ip || lastPort != port) { ConnectionCalled = false; }

                if (!ConnectionCalled)
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                    TcpClient client = new TcpClient();
                    this.client = client;

                    client.Connect(ep);
                    this.binaryWriter = new BinaryWriter(this.client.GetStream());
                    this.binaryReader = new BinaryReader(client.GetStream());

                    ConnectionCalled = true;
                    lastIp = ip;
                    lastPort = port;

                }


                NetworkStream ns = client.GetStream();
                byte[] bufW = Encoding.ASCII.GetBytes("get /position/latitude-deg\r\n");
                ns.Write(bufW, 0, bufW.Length);
                System.IO.StreamReader line1 = new StreamReader(ns);
                string buffer = line1.ReadLine();
                location.Lat = splitIt(buffer);

                bufW = Encoding.ASCII.GetBytes("get /position/longitude-deg\r\n");
                ns.Write(bufW, 0, bufW.Length);
                line1 = new StreamReader(ns);
                buffer = line1.ReadLine();
                location.Lon = splitIt(buffer);

                bufW = Encoding.ASCII.GetBytes("get /controls/engines/current-engine/throttle\r\n");
                ns.Write(bufW, 0, bufW.Length);
                line1 = new StreamReader(ns);
                buffer = line1.ReadLine();
                location.Throttle = splitIt(buffer);

                bufW = Encoding.ASCII.GetBytes("get /controls/flight/rudder\r\n");
                ns.Write(bufW, 0, bufW.Length);
                line1 = new StreamReader(ns);
                buffer = line1.ReadLine();
                location.Rudder = splitIt(buffer);

                return location;


            }
            private set { }
        }

        public double splitIt(string line)
        {
            string[] words = line.Split(' ');
            string ans = words[2].Remove(0, 1);
            ans = ans.Remove(ans.Length - 1, 1);
            double ret = Convert.ToDouble(ans);
            return ret;
        }

        private InfoModel()
        {
            location = new Location();

        }




    }
}