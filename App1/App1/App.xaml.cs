using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Net;
using System.Xml.Linq;
using System.Collections.Generic;


using System.IO;

namespace App1
{
    // приложение называется App1
    // есть главная страница MainPage с двумя вкладками Page1 и Page2
    public class Camera 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Sound { get; set; }
        public string ParentName { get; set; }

    }
    public partial class App : Application
    {
        public static List<Camera> Cameras; // для хранения найденных камер
        public static string Spisok; // для хранения готового списка камер в виде строки

        

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            Cameras = CameraListCreator();
            Spisok = MakeSpisok();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static XDocument GetReply_from_http() // получаем конфигурацию по http запросу в виде XDocument
        {
            WebClient client = new WebClient();
            string address = "http://demo.macroscop.com/configex?login=root";
            string reply = client.DownloadString(address);

            reply = reply.Remove(0, reply.IndexOf("<"));

            XDocument My_XmlDoc = XDocument.Parse(reply);

            return My_XmlDoc;
        }

        public static List<Camera> CameraListCreator() // находит камеры
        {

            XDocument Xdoc = GetReply_from_http();
            List<Camera> cameras = new List<Camera>();

            
            foreach (XElement el in Xdoc.Element("Configuration").Element("Channels").Elements("ChannelInfo"))
            {
                Camera C = new Camera();
                C.Id = el.Attribute("Id").Value;
                C.Name = el.Attribute("Name").Value;
                C.Sound = el.Attribute("IsSoundOn").Value;
                foreach (XElement elel in Xdoc.Element("Configuration").Element("RootSecurityObject").Element("ChildSecurityObjects").
                    Elements("SecObjectInfo"))
                {
                    foreach (XElement elelel in elel.Element("ChildChannels").Elements("ChannelId"))
                    {
                        if (elelel.Value == C.Id)
                        {
                            C.ParentName = elel.Attribute("Name").Value;
                        }
                    }
                }
                cameras.Add(C);
            }
            return cameras;

        }

        public static string MakeSpisok() //собирает список камер в виде строки
        {
            string temp = "";
            int i = 1;
            foreach (Camera c in Cameras)
            {
                temp += i + ") " + c.Name + ", " + c.Sound + ", " + c.ParentName + "\n";
                i++;
            }
            return temp;
        }
    }
}
