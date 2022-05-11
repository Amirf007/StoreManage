using StoreManage.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.Categories
{
    public static class AddCategoryDtoFactory
    {
        public static AddCategoryDto GenerateAddCategoryDto()
        {
            return new AddCategoryDto
            {
                Title = "لبنیات"
            };
        }
    }
}
