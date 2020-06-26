using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Models.YuPai;
using ProduceFeeder.UI.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ProduceFeeder.UI.Models.ItemsContainer
{
    /// <summary>
    /// 投料的集合
    /// </summary>
    public class ICMOContainer : IMaterialWatcher,IBearingMatch
    { 
        public static List<ItemPOCO> icmoBomcustomerGroup = new List<ItemPOCO>();

        int cpItemId;
        public ICMOContainer(int cpitemId)
        {
            cpItemId = cpitemId;
            OnItemsChanged();
        }
        private List<ICMO> GetAllCP()
        {
            return new ICMORepository().GetAllCP().Where(w => w.FCancellation == false && w.FStatus == 0 && w.FItemId == cpItemId).ToList() ;
        }
        public List<BOMItemBase> bomitems = new List<BOMItemBase>();      /// <summary>
                                                                          /// BOM分解
                                                                          /// </summary>
        private void CPDeBOMDecompose()
        {
            var cpitems = GetAllCP();
            foreach (var item in cpitems)
            {

                bomitems = bomitems.Concat(new K3CPItemBase { K3ItemID=item.FItemId}.GetComponetItemsConsum(item.FauxQty)).ToList();
            }
        }

        private void BOMItemGroup()
        {
            icmoBomcustomerGroup = bomitems.GroupBy(g => g.K3ItemID)
                .Select(s => new ItemPOCO { ItemID = s.Key, Qty = s.Sum(m => m.FAuxQty) }).ToList();
        }


        public void Insert(ICMO item)
        {
            OnItemsChanged();
        }
        public void Update(ICMO item)
        {


            OnItemsChanged();
        }
        public void Delete(ICMO item)
        {

            OnItemsChanged();
        }

        private void OnItemsChanged()
        {
            GetAllCP();
            CPDeBOMDecompose();
            BOMItemGroup();
        }
    }
}

