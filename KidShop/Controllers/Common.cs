using KidShop.Areas.Admin.Models.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidShop.Controllers
{
    public class Common
    {
        public static List<int> findChild(int parentId)
        {
            KidShopDbContext db = new KidShopDbContext();
            List<int> list = db.Category.Where(r => r.ParentId == parentId).Select(r => r.CategoryId).Distinct().ToList();
            if (list.Count() != 0)
            {
                List<int> l = new List<int>();
                foreach (int i in list)
                {
                    l.AddRange(findChild(i));
                }
                list.AddRange(l);
            }
            return list;
        }
    }
}