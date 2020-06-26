using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
  public  class RclRunCalendar
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }

        [ForeignKey("RclMachine")]
        public int RclID { get; set; }
        public RclMachine RclMachine { get; set; }
    }
}
