using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.Categories.Contracts
{
    public interface CategoryRepository : Repository
    {
        void Add(Category category);
        bool IsExistCategoryTitle(string title);
        Category GetbyId(int id);
        IList<GetCategoryDto> GetAll();
        void Remove(Category category);
    }
}
