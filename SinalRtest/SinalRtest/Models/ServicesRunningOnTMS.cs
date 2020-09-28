using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SinalRtest.Models
{
    public class ServicesRunningOnTMS
    {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TMS_Id { get; set; }
        public int ServiceId { get; set; }
        public string Status { get; set; }
    }
}
