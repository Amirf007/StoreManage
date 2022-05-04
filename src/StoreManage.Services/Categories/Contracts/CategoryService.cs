using StoreManage.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.Categories.Contracts
{
    public interface CategoryService : Repository
    {
        void Add(AddCategoryDto dto);
        void Update(int id, UpdateCategoryDto dto);
        IList<GetCategoryDto> GetAll();
        GetCategoryDto GetCategory(int id);
    }
}
    