using StoreManage.Entities;
using StoreManage.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private EFDataContext _dataContext;

        public EFCategoryRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Category category)
        {
            _dataContext.Categories.Add(category);
        }

        public IList<GetCategoryDto> GetAll()
        {
            return _dataContext.Categories
                  .Select(_ => new GetCategoryDto
                  {

                     Title = _.Title,

                  }).ToList();
        }

        public Category GetbyId(int id)
        {
            return _dataContext.Categories.Find(id);
        }

        public bool IsExistCategoryTitle(string title)
        {
            return _dataContext.Categories.Any(_ => _.Title == title);
        }

        public void Remove(Category category)
        {
            _dataContext.Categories.Remove(category);
        }
    }
}
