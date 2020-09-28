using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SinalRtest.Hubs;
using SinalRtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinalRtest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController:ControllerBase
    {
        private IHubContext<MessageHub> _messageHubContext;
        private readonly TMSDbContext db;

        public MessageController(IHubContext<MessageHub> messageHubContext, TMSDbContext _context)//, TMSDbContext _context
        {
             db = _context;
            _messageHubContext = messageHubContext;
        }
        [HttpGet]
        public IActionResult Get()
        {


            ServicesRunningOnTMS updatetServices = db.RunningServices.SingleOrDefault(x => x.TMS_Id == 1 && x.ServiceId == 1);
            updatetServices.Status = "Test";
            if (!ModelState.IsValid)
            {
                return BadRequest("det er her det feiler: " + ModelState);
            }

            /* if (id != product._id)
             {
                 return BadRequest("Sorry, seems something wrong. Couldn't deter mine record to update.");
             }*/



            db.Entry(updatetServices).State = EntityState.Modified;
            db.SaveChanges();



            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }



            _messageHubContext.Clients.All.SendAsync("Send", "Denne servicen har stoppet");
            //var updateted = db.TMS.Find(1);


            return Ok("Hei hei fra controller");
        }
    
        [HttpPost]
        public IActionResult updateStatusTMS(MyService ms)
            
        {

            /*var Tf_services = db.TF_Services.ToList();
            var servId =Tf_services.SingleOrDefault(s => s.Name == ms.name);
            int test;
         
                 test = servId.Id;*/
           
           
            ServicesRunningOnTMS updatetServices = db.RunningServices.SingleOrDefault(x => x.TMS_Id == ms.TmsId && x.ServiceId == ms.ServiceId);
            updatetServices.Status = ms.status;
            if (!ModelState.IsValid)
            {
                return BadRequest("det er her det feiler: " + ModelState);
            }

            db.Entry(updatetServices).State = EntityState.Modified;
            db.SaveChanges();

            _messageHubContext.Clients.All.SendAsync("Send", "Denne servicen har endret status:"+ms.displayName);
          
            return Ok();

        }
        [Route("[action]")]
        [HttpPost]
        public IActionResult startService(MyService ms)

        {

       
            _messageHubContext.Clients.All.SendAsync("Start",  ms);

            return Ok(ms);

        }





    }
    public class MessagePost
    {
        public virtual string message { get; set; }
    }

   /* public class MyService
    {
        public string Id { get; set; }
        public int TmsId { get; set; }
        public int ServiceId { get; set; }
        public string displayName { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string MachineName { get; set; }


    }*/
}
