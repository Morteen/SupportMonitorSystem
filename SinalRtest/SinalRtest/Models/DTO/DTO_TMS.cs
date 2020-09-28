using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinalRtest.Models.DTO
{
    public class DTO_TMS
    {
        public TMS tms { get; set; }
        public List<TF_Services> TF_Services { get; set; }
    }
}
