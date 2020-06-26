using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    /// <summary>
    /// 热处理机器
    /// </summary>
    /// 
    [Table("wjj_RclMachine")]
  public  class RclMachine
    {
        public int ID { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 停用日期
        /// </summary>
        public List<RclRunCalendar> ForbiddenDate { get; set; }
        public void Start ()
        {

        }
        public void Stop()
        {

        }

    }
}
