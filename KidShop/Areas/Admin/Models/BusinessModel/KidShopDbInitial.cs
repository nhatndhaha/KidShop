using KidShop.Areas.Admin.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.BusinessModel
{
    public class KidShopDbInitial : DropCreateDatabaseIfModelChanges<KidShopDbContext>
    {
        protected override void Seed(KidShopDbContext context)
        {
            var acc1 = new Account() { FirstName = "Đức Chính", LastName = "Nguyễn", Password = "f6fdffe48c908debf4c3bd36c32e72", Avatar = "use7en.png", Email = "u.se7en@gmail.com", Allowed = true, Role = 0 };
            context.Account.Add(acc1);
            context.SaveChanges();
        }
    }
}