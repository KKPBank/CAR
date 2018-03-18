using CARTestLogService.CARWS;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace CARTestLogService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //NotifyRequestFile();
                //RequestResult();
                ASMXCreateActivityLog();
                //NotifyResult();
                //TestGetResult();
                //CreateActivityLogResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static void NotifyRequestFile()
        {
            Console.WriteLine("NotifyText");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                dynamic jsonObject = new JObject();
                jsonObject.system = "INS";
                jsonObject.serviceName = "CreateActivityLog";
                jsonObject.dataDate = "20170120";
                jsonObject.path = "http://www.google.co.th";
                jsonObject.getResult = true;

                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage resp = client.PostAsync("http://10.202.104.51/CARLogService1.5/CARLogService.svc/CreateActivityLog", content).Result;
                Console.WriteLine(resp.IsSuccessStatusCode);
                if (resp.IsSuccessStatusCode)
                {
                    // Parse the response body. 
                    resp.Content.ReadAsStringAsync().Wait();
                    string jsonString = resp.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(jsonString);
                }
            }
        }
        private static void RequestResult()
        {
            Console.WriteLine("Request File Result");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //dynamic jsonObject = new JObject();
                //jsonObject.System = "INT";
                //jsonObject.DataDate = "20170216";

                //var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                //HttpResponseMessage resp = client.PostAsync("http://10.202.104.141/INSWeb/InterfaceFiles/Download?FileId=4983", content).Result;
                HttpResponseMessage resp = client.GetAsync("http://10.202.104.141/INSWeb/InterfaceFiles/Download?FileId=4983").Result;
                Console.WriteLine(resp.IsSuccessStatusCode);
                if (resp.IsSuccessStatusCode)
                {
                    // Parse the response body. 
                    resp.Content.ReadAsStringAsync().Wait();
                    string jsonString = resp.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(jsonString);
                }
            }
        }

        private static void NotifyResult()
        {
            Console.WriteLine("Notify Result to Source System");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                dynamic jsonObject = new JObject();
                jsonObject.system = "INS";
                jsonObject.serviceName = "HttpCreateActivityLog";
                jsonObject.dataDate = "20170216";
                jsonObject.path = "http://aucappd01b/CARLogService1.5/CARLogService.svc/CreateActivityLogResult";

                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage resp = client.PostAsync("http://10.202.104.141/insweb/InterfaceFiles/NotificationCar", content).Result;
                Console.WriteLine(resp.IsSuccessStatusCode);
                if (resp.IsSuccessStatusCode)
                {
                    // Parse the response body. 
                    resp.Content.ReadAsStringAsync().Wait();
                    string jsonString = resp.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(jsonString);
                }
            }
        }

        private static void TestGetResult()
        {
            Console.WriteLine("Notify Result to Source System");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage resp = client.GetAsync("http://10.202.104.141/INSWeb/InterfaceFiles/Download?FileId=5487").Result;
                Console.WriteLine(resp.IsSuccessStatusCode);
                if (resp.IsSuccessStatusCode)
                {
                    // Parse the response body. 
                    resp.Content.ReadAsStringAsync().Wait();
                    string jsonString = resp.Content.ReadAsStringAsync().Result;
                    var jsonSerialiser = new JavaScriptSerializer();
                    Console.WriteLine(jsonString);
                }
            }
        }

        private static void ASMXCreateActivityLog()
        {
            CASLogServiceSoapClient ws = new CASLogServiceSoapClient();
            LogServiceHeader header = new LogServiceHeader();
            header.ReferenceNo = "2017021400000941";
            header.ServiceName = "CreateActivityLog";
            header.SystemCode = "INS";
            header.TransactionDateTime = DateTime.Now;

            CreateActivityLogData data = new CreateActivityLogData();
            
            data.ReferenceNo = "2017021400000941";
            data.ActivityDateTime = DateTime.Now;
            data.ChannelID = "BRANCH";
            data.SubscriptionTypeID = 18;
            data.SubscriptionID = "636227624986824703AA";
            data.TicketID = "170424315610";
            data.ReferenceAppID = "0001-OL-17-0007982";
            data.PDMProductGroupID = "31";
            data.PDMProductSubGroupID = "01";
            data.PDMProductID = "001";
            data.PDMCampaignID = "310100115030001";
            data.TypeID = 6;
            data.AreaID = 8;
            data.SubAreaID = 684;
            data.ActivityTypeID = 13;


            var result = ws.CreateActivityLog(header, data);
            Console.WriteLine(result);
        }
        
        private static void CreateActivityLogResult()
        {
            Console.WriteLine("Notify Result to Source System");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //system=INS,serviceName=HttpCreateActivityLog,dataDate=20200101,refNo=INS00000853--

                dynamic jsonObject = new JObject();
                jsonObject.system = "INS";
                jsonObject.serviceName = "HttpCreateActivityLog";
                jsonObject.dataDate = "20200101";
                jsonObject.refNo = "INS00000853";

                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage resp = client.PostAsync("http://localhost:24190/CARLogService.svc/CreateActivityLogResult", content).Result;
                Console.WriteLine(resp.IsSuccessStatusCode);
                if (resp.IsSuccessStatusCode)
                {
                    // Parse the response body. 
                    resp.Content.ReadAsStringAsync().Wait();
                    string jsonString = resp.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(jsonString);
                }
            }
        }

    }
}


