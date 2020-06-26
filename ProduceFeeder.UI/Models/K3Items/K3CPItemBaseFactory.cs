using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models.K3Items
{
    public static class K3CPItemBaseFactory
    {
        public static Dictionary<int, K3CPItemBase> K3CPItemCash = new Dictionary<int, K3CPItemBase>();
        public static async Task<K3CPItemBase> K3CPItemBaseCreateAsync(int itemId)
        {
            if (K3CPItemCash.ContainsKey(itemId))
            {
                return K3CPItemCash[itemId];
            }
            else
            {
                var cpItem = new K3CPItemBase() { K3ItemID = itemId };
                await cpItem.OnItemIDChangedAsync();
                K3CPItemCash[itemId] = cpItem;

                return cpItem;
            }
        }
    }
}
