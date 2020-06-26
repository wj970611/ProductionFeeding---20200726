using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models.K3Items
{
    /// <summary>
    /// 预排和投料的基类
    /// </summary>
    public class MPSItemBase: INotifyPropertyChanged
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public K3CPItemBase CPItem { get; set; }

        public string SubFNumber { get; set; }
        public string SubFName { get; set; }
        public string SubFModel { get; set; }
        public int SubFItemId { get; set; }

        public string BillNo { get; set; }

        public int QJId { get; set; }
        public int OnePlanQTY { get; set; }


        [MaxLength(30)]
        public string PlanStamp { get; set; }

        public int Qty { get; set; }


        public string DXQ { get; internal set; }
        public bool IsDeleted { get; set; }


        /// <summary>
        /// 投料人
        /// </summary>
        /// 
        [MaxLength(30)]
        public string Feeder { get; set; }
 
       



        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }


    }
}
