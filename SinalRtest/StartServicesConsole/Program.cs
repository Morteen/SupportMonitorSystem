using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace StartServicesConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            //HttpClient client = new HttpClient();
        

        Console.WriteLine("Det kjører her: " );
            var connection = new HubConnectionBuilder()
                  .WithUrl("https://localhost:44306/signalRmessage")
                  .Build();
            Console.WriteLine("Det kjører her etter Build ");
            connection.StartAsync().Wait();
            Console.WriteLine("Det kjører her etter startconnection ");
            connection.InvokeCoreAsync("Send",args: new[] { "Hei hei fra Console app" });
            Console.WriteLine("Det kjører her etter InvokeAsync ");
            connection.On("Start", (MyService service) =>
             {
                 Console.WriteLine("Beskjed fra web klient: TmsId: "+service.TmsId+" ServiceId:"+service.ServiceId+" Displayname: "+service.displayName+" Action: "+service.action);

                 if (service.action == "restart")
                 {
                     Console.WriteLine("Action er : " + service.action);
                     restartService(service);
                 }
                 else if (service.action == "stop") {
                     Console.WriteLine("Action er : " + service.action);
                     stopService(service);
                 }
                 else
                 {

                     startService(service);


                 }
              
                

             });

         



            Console.ReadKey();
        }



        private static void startService(MyService service)
        {
            var srv = new ServiceController(service.name);
            if (srv.Status != ServiceControllerStatus.Running)
            {

                Console.WriteLine("Sjekker om man har riktig service: " + srv.DisplayName);
                //Gir kommando om å start servicen
                srv.Start();
              
                //Sender beskjed til frontend at servicen forsøker å starte
                RunAsync(new MyService { ServiceId = service.ServiceId, TmsId = service.TmsId, displayName = srv.DisplayName, status = "Starting", name = srv.ServiceName, MachineName = srv.MachineName }).Wait();
                srv.WaitForStatus(ServiceControllerStatus.Running);


                // StartAttempt(srv.ServiceName).Wait();
                RunAsync(new MyService { ServiceId = service.ServiceId, TmsId = service.TmsId, displayName = srv.DisplayName, status = srv.Status.ToString(), name = srv.ServiceName, MachineName = srv.MachineName }).Wait();


            }
            else if (srv.Status == ServiceControllerStatus.Running)
            {
                Console.WriteLine("Servicen kjører allerede");
            }
            else { Console.WriteLine("Noe går galt"); }

        }




        private static void stopService(MyService service)
        {
            var srv = new ServiceController(service.name);
            if (srv.Status == ServiceControllerStatus.Running)
            {

                Console.WriteLine("Sjekker om man har riktig service: " + srv.DisplayName);
                //Gir kommando om å start servicen
                srv.Stop();
                Console.WriteLine("Har gitt beskjed om å stoppe: " + srv.DisplayName);
                srv.WaitForStatus(ServiceControllerStatus.Stopped);
                Console.WriteLine("Har fått stoppet status: " + srv.DisplayName);
                //Sender beskjed til frontend at servicen har blitt stoppet
                RunAsync(new MyService { ServiceId = service.ServiceId, TmsId = service.TmsId, displayName = srv.DisplayName, status = "Stopped", name = srv.ServiceName, MachineName = srv.MachineName }).Wait();
             


           
            }
            else { Console.WriteLine("Noe går galt i stopService "); }

        }




        private static void restartService(MyService service) {

            var srv = new ServiceController(service.name);
            if (srv.Status == ServiceControllerStatus.Running)
            {

                Console.WriteLine("Sjekker om man har riktig service: " + srv.DisplayName);
                //Gir kommando om å start servicen
                try
                {
                    if ((srv.Status.Equals(ServiceControllerStatus.Running)) || (srv.Status.Equals(ServiceControllerStatus.StartPending)))
                    {
                        srv.Stop();
                    }
                    srv.WaitForStatus(ServiceControllerStatus.Stopped);
                    srv.Start();
                    srv.WaitForStatus(ServiceControllerStatus.Running);

                    // RunAsync(new MyService { ServiceId = service.ServiceId, TmsId = service.TmsId, displayName = srv.DisplayName, status = "Starting", name = srv.ServiceName, MachineName = srv.MachineName }).Wait();
                    RunAsync(new MyService { ServiceId = service.ServiceId, TmsId = service.TmsId, displayName = srv.DisplayName, status = srv.Status.ToString(), name = srv.ServiceName, MachineName = srv.MachineName }).Wait();
                }
                catch
                {
                    Console.WriteLine("Det skjedde noe galt som havnet i catch i  restartService");
                }




            }

            else { 
                Console.WriteLine("Noe går galt i restartService Status er ikke Running");
            }


        }






        private static async Task RunAsync(MyService service)
        {




            ///Dette er vanlig HTTP client og ikke signalR

            using (var client = new HttpClient())
            {
              

                client.BaseAddress = new Uri("https://localhost:44306/Message");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth","xyz");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                HttpResponseMessage response;
                Console.WriteLine("Sjekker hva som er i fred med å sendes: " + service.displayName+ "Status : "+service.status);



                response = await client.PostAsJsonAsync("https://localhost:44306/Message", service);
               
                if (response.IsSuccessStatusCode)
                {

                    Console.WriteLine("Denne tjenesten har  endret status: " + service.displayName+"  Status:"+service.status);
                }
                else { Console.WriteLine("Det er noe galt: " + response.StatusCode); }

            }
        }











        //Async method to be awaited
        public static Task  sendStatusAsync(ServiceController srv)
        {
            Task.Delay(1000);
            RunAsync(new MyService { ServiceId = 10, TmsId = 4, displayName = srv.DisplayName, status = srv.Status.ToString(), name = srv.ServiceName, MachineName = srv.MachineName }).Wait();
             
            

            return Task.CompletedTask;
        }

      






        private static async Task StartAttempt(string name) {

            // var srv = new ServiceController("WebsiteStatus");
            bool test = false;
            for (int i=0;i<20;i++)
            {
                var MySrv = new ServiceController(name);
              

                   
                Console.WriteLine("Sjekker om den går:" + MySrv.Status);
                if (MySrv.Status == ServiceControllerStatus.Running &&test ==false)
                {
                    sendStatusAsync(MySrv).Wait();
                  
                    test = true;
                    break;
                }
                Thread.Sleep(1000);


            }
        }














    }
















    public class MyService
    {
        public string Id { get; set; }
        public int TmsId { get; set; }
        public int ServiceId { get; set; }
        public string displayName { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string MachineName { get; set; }
        public string action { get; set; }


    }
   
}
