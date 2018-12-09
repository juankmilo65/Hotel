//using Client3.HotelServiceReference;
using Client3.HotelServiceReferenceIIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client3
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] bytes = File.ReadAllBytes(@"D:\Trabajo\webservice chile\Nuevo.xml");
            byte[] bytes = File.ReadAllBytes(@"D:\Trabajo\webservice chile\MinimunXML.xml");
            //byte[] bytes = File.ReadAllBytes(@"D:\Trabajo\webservice chile\WarningXML.xml");

            string file = Convert.ToBase64String(bytes);
            Response response = new Response();


            using (HotelServiceClient client = new HotelServiceClient())
            {
                client.ClientCredentials.UserName.UserName = "peoplesite";
                client.ClientCredentials.UserName.Password = "password";
                response = client.ValidationXML(file);

                Console.Write(response.Code);
            }
        }
    }
}

