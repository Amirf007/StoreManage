using StoreManage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.Categories
{
    public static class CategoriesFactory
    {
        public static List<Category> GenerateCategories()
        {
            return new List<Category>
            {
                new Category
                {
                Title = "لبنیات"
                },
                new Category
                {
                Title = "تنقلات"
                },
                new Category
                {
                Title = "شوینده ها"
                }
            };
        }
    }
}
