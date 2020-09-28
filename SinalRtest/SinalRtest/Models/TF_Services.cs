using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SinalRtest.Models
{
    public class TF_Services
    {

         [Key]
      
        public int Id { get; set; }
        public string Name { get; set; }
        public string  DisplayName { get; set; }
        public string Status { get; set; }
    }
}
