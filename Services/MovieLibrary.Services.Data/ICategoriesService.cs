namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Categories;

    public interface ICategoriesService
    {
        Task CreateCategoryAsync(InputCreateCategoryViewModel categoryName);

        AllCategoriesViewModel GetAllCategories();

        bool IsExisting(string name);

        InputCreateCategoryViewModel GetCategoryForEdit(string category);

        Task EditCategoryAsync(string category, InputCreateCategoryViewModel model);

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
