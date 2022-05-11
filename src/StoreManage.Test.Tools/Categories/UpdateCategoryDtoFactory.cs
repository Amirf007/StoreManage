using StoreManage.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.Categories
{
    public static class UpdateCategoryDtoFactory
    {
        public static UpdateCategoryDto GenerateUpdateCategoryDto(string title)
        {
            return new UpdateCategoryDto
            {
                Title = title
            };
        }
    }
}
