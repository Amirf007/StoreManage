using StoreManage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.Categories
{
    public static class CategoryFactory
    {
        public static Category CreateCategory()
        {
            return new Category
            {
                Title = "لبنیات"
            };
        }
    }
}
