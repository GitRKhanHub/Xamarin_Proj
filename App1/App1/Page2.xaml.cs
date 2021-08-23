using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Net;
using System.IO;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        //static Image image = new Image() { Source = "examp.jpg"};
        //static ImageSource MySource = image.Source;
        public static ImageSource MySource;

        public Page2()
        {
            InitializeComponent();
            
        }








        private void Button_Click(object sender, EventArgs e)
        {
            button1.Text = "Получить изображение";

            WebClient webClient = new WebClient();
            string myurl = "http://demo.macroscop.com/mobile?channelid=" +
                App.Cameras[0].Id + "&oneframeonly=true&login=root";
            Stream stream = webClient.OpenRead(myurl);


            MySource = ImageSource.FromStream(() => stream);
            myLabeltwo.Text = "Кадр с камеры: " +  App.Cameras[0].Name;
            myImage.Source = MySource;
            


        }


    }
}