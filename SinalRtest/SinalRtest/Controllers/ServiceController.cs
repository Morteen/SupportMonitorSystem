using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SinalRtest.Models;
using SinalRtest.Models.DTO;

namespace SinalRtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase

    {


        private readonly TMSDbContext db;
       
       

        public ServiceController(TMSDbContext _context)
        {
            db = _context;
         /*   var allreadyExist = db.TMS.Any(x => x.Id == 1);
            if (!allreadyExist)
            {

              
                db.TMS.Add(new TMS {  Name = "Gausdal", Category = "Transfleet" });
               db.SaveChanges();
            }
            var allreadyExist1 = db.TMS.Any(x => x.Id == 2);
            if (!allreadyExist1)
            {

                db.TMS.Add(new TMS {  Name = "Brødrene Dahl", Category = "Transfleet" });
                db.SaveChanges();
            }
            var allreadyExist2 = db.TMS.Any(x => x.Id == 3);
            if (!allreadyExist2)
            {
                db.TMS.Add(new TMS {  Name = "Suldaltransport",Category = "LogNett"});
                db.SaveChanges();
            }
            var allreadyExist3 = db.TMS.Any(x => x.Id == 4);
            if (!allreadyExist3)
            {
                db.TMS.Add(new TMS { Name = "Movator", Category = "Transfleet" });
                db.SaveChanges();
            }


            var allreadyExistServ = db.TF_Services.Any(x => x.Id == 1);
            if (!allreadyExistServ)
            {


                db.TF_Services.Add(new TF_Services { Name = "Transfleetcoreservcie", DisplayName = "Transfleet CoreService",Status="Stopped" });
                db.SaveChanges();
            }

            var allreadyExistServ1 = db.TF_Services.Any(x => x.Id == 2);
            if (!allreadyExistServ1)
            {


                db.TF_Services.Add(new TF_Services { Name = "Transfleetxdbimport", DisplayName = "Transfleet Xdb  import",Status = "stoppet" });
                db.SaveChanges();
            }
            var allreadyExistServ2 = db.TF_Services.Any(x => x.Id == 3);
            if (!allreadyExistServ2)
            {


                db.TF_Services.Add(new TF_Services { Name = "Transfleetdbxexport", DisplayName = "Transfleet Dbx  export",Status = "Running" });
                db.SaveChanges();
            }
            var allreadyExistServ3 = db.TF_Services.Any(x => x.Id == 4);
            if (!allreadyExistServ3)
            {


                db.TF_Services.Add(new TF_Services { Name = "Transfleetfileconverter", DisplayName = "Transfleet Fileconverter  ",Status="Running" });
                db.SaveChanges();
            }


            var allreadyExistJoinTable = db.RunningServices.Any(x => x.Id == 1);
            if (!allreadyExistJoinTable)
            {


                db.RunningServices.Add(new ServicesRunningOnTMS { TMS_Id=1,ServiceId=1, Status = "Stopped" });
               
              
                db.SaveChanges();
            }
            var allreadyExistJoinTable1 = db.RunningServices.Any(x => x.Id == 2);
            if (!allreadyExistJoinTable1)
            {


              
                db.RunningServices.Add(new ServicesRunningOnTMS { TMS_Id = 1, ServiceId = 2, Status = "Running" });
            
                db.SaveChanges();
            }
            var allreadyExistJoinTable2 = db.RunningServices.Any(x => x.Id == 3);
            if (!allreadyExistJoinTable2)
            {



                db.RunningServices.Add(new ServicesRunningOnTMS { TMS_Id = 1, ServiceId = 3, Status = "Stopped" });

                db.SaveChanges();
            }
            var allreadyExistJoinTable3 = db.RunningServices.Any(x => x.Id == 4);
            if (!allreadyExistJoinTable3)
            {



                db.RunningServices.Add(new ServicesRunningOnTMS { TMS_Id = 2, ServiceId = 3, Status = "Stopped" });

                db.SaveChanges();
            }
            var allreadyExistJoinTable4 = db.RunningServices.Any(x => x.Id == 5);
            if (!allreadyExistJoinTable4)
            {



                db.RunningServices.Add(new ServicesRunningOnTMS { TMS_Id = 3, ServiceId = 3, Status = "Stopped" });

                db.SaveChanges();
            }

            var allreadyExistJoinTable5 = db.RunningServices.Any(x => x.Id == 6);
            if (!allreadyExistJoinTable5)
            {



                db.RunningServices.Add(new ServicesRunningOnTMS { TMS_Id = 4, ServiceId = 3, Status = "Stopped" });

                db.SaveChanges();
            }
            var allreadyExistJoinTable6 = db.RunningServices.Any(x => x.Id == 7);
            if (!allreadyExistJoinTable6)
            {
                db.RunningServices.Add(new ServicesRunningOnTMS { TMS_Id = 1, ServiceId = 5, Status = "Running" });
                db.RunningServices.Add(new ServicesRunningOnTMS { TMS_Id = 1, ServiceId = 6, Status = "Running" });

                db.SaveChanges();



            }*/
          










        }










        [HttpGet("{tmsId:int}")]
        public IActionResult Get(int tmsId)
        {
            var tms = db.TMS.Find(tmsId);
            if (tms == null)
            {
                return BadRequest();
            }

           // List<DTO_TMS> dto_tms = new List<DTO_TMS>();
            List<TMS> tmsList = db.TMS.ToList();
            List<TF_Services> list = db.TF_Services.ToList();
            List<ServicesRunningOnTMS> RunningServices = db.RunningServices.ToList();


            var newDto = new DTO_TMS();
                newDto.tms = tms;
                newDto.TF_Services = new List<TF_Services>();
                foreach (ServicesRunningOnTMS sr in RunningServices.Where(x => x.TMS_Id == tms.Id))
                {
                    var service = list.FirstOrDefault(x => x.Id == sr.ServiceId); 
                    service.Status = sr.Status;
                   service.Id = sr.ServiceId;
                    newDto.TF_Services.Add(new TF_Services { Id=service.Id,Name = service.Name, DisplayName = service.DisplayName, Status = sr.Status });

                }

              


            
            return Ok(newDto);

        }
       

        [HttpGet("{category}")]
        public async Task<ActionResult<DTO_TMS>> GetList(string category)

        {   List<DTO_TMS>dto_tms = new List<DTO_TMS>();
            List<TMS> tmsList =  db.TMS.ToList();
            List<TF_Services> list =  db.TF_Services.ToList();
            List<ServicesRunningOnTMS> RunningServices =  db.RunningServices.ToList();
         

            if (category == "")
            {

                foreach (TMS tms in tmsList)
                {

                    var newDto = new DTO_TMS();
                    newDto.tms = tms;
                    newDto.TF_Services = new List<TF_Services>();
                    foreach (ServicesRunningOnTMS sr in RunningServices)
                    {
                        if (sr.TMS_Id == tms.Id)
                        {
                            var service = list.Find(x => x.Id == sr.ServiceId);
                            service.Status = sr.Status;
                            newDto.TF_Services.Add(service);
                           
                        }
                    }


                    dto_tms.Add(newDto);





                }
            }
            else
            {
                // My_TF_Services = db.TF_Services.ToList();


                foreach (TMS tms in tmsList.Where(x => x.Category == category))
                {

                    var newDto = new DTO_TMS();
                    newDto.tms = tms;
                    newDto.TF_Services = new List<TF_Services>();
                    foreach (ServicesRunningOnTMS sr in RunningServices.Where(x => x.TMS_Id == tms.Id))
                    {
                        var service = list.FirstOrDefault(x=>x.Id==sr.ServiceId);
                        service.Status = sr.Status;
                        newDto.TF_Services.Add(new TF_Services { Name = service.Name, DisplayName = service.DisplayName, Status = sr.Status });
                       
                    }

                    dto_tms.Add(newDto);

                }
            

            }
         

            if (category == null)
            {
                return NotFound();
            }

            return Ok(dto_tms);
        }

    }
}