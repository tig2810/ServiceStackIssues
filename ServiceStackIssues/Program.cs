using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;

namespace ServiceStackIssues
{
    class Program
    {
        static void Print(string text, params object[] args)
        {
            Console.Write("{0:HH:mm:ss.fff} >>> ", DateTime.Now);
            Console.WriteLine(text, args);
        }

        static int SendRequest(string url, bool compressed)
        {
            WebClient client = new WebClient();
            DateTime date = DateTime.Now;

            url += "&Compressed=" + (compressed ? "true" : "false");

            client.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

            Print("Sending request...");
            var data = client.DownloadData(url);
            Print("Got response in {0} s", (DateTime.Now - date).TotalSeconds);
            int size = data.Length;
            Print("Response Size ({0}): {1,10:n0}", compressed ? "Compressed" : "Uncompressed", size);

            JsonServiceClient serviceClient = new JsonServiceClient();
            date = DateTime.Now;
            var response = serviceClient.Get<OrderResponseDto>(url);
            Print("Total time (Serialize + Transmit + Deserialize): {0} s", (DateTime.Now - date).TotalSeconds);

            return size;
        }

        static void SendRequest(string url)
        {            
            int uncompressedSize = SendRequest(url, false);            
            int compressedSize = SendRequest(url, true);

            Print("Compression Ratio: {0:n2}%", (1 - (double)compressedSize / uncompressedSize) * 100);
        }
        
        static void Main(string[] args)
        {
            var app = new AppHost();
            app.Start();
            
            Process.Start(app.URL);

            //JsonServiceClient client = new JsonServiceClient(app.URL);
            //var response = client.Get<HttpWebResponse>("/json/reply/OrderRequestDto");

            /* ServiceStack */

            Console.WriteLine("Following tests are done using ServiceStack serializer");
            Console.WriteLine();

            Print("TEST: Buyer attached to each order:");
            SendRequest(app.URL + "json/reply/OrderRequestDto?AttachBuyerToOrder=true");

            Console.WriteLine();

            Print("TEST: Buyer was not set to order (need assembler):");
            SendRequest(app.URL + "json/reply/OrderRequestDto?AttachBuyerToOrder=false");

            /* JSON.NET */

            Console.WriteLine();
            Console.WriteLine("Following tests are done using JSON.NET serializer");
            Console.WriteLine();

            JsConfig<OrderResponseDto>.RawSerializeFn = t => JsonConvert.SerializeObject(t);
            JsConfig<OrderResponseDto>.RawDeserializeFn = t => JsonConvert.DeserializeObject<OrderResponseDto>(t);

            Print("TEST: Buyer attached to each order:");
            SendRequest(app.URL + "json/reply/OrderRequestDto?AttachBuyerToOrder=true");

            Console.WriteLine();

            Print("TEST: Buyer was not set to order (need assembler):");
            SendRequest(app.URL + "json/reply/OrderRequestDto?AttachBuyerToOrder=false");

            "Press any key to exit...".Print();
            Console.ReadLine();
        }
    }
}
