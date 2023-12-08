using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkey.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Languages { get; set; }


        public DateTime Date { get; set; }

        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        

    }
}
