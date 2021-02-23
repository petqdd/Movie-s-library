namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Categories;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IRepository<MoviesCategory> moviesCategoriesRepository;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Movie> moviesRepository,
            IRepository<MoviesCategory> moviesCategoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.moviesRepository = moviesRepository;
            this.moviesCategoriesRepository = moviesCategoriesRepository;
        }

        public async Task CreateCategoryAsync(InputCreateCategoryViewModel category)
        {
            var currentCategory = new Category
            {
                Name = category.Name,
            };

            await this.categoriesRepository.AddAsync(currentCategory);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public AllCategoriesViewModel GetAllCategories()
        {
            var categories = new AllCategoriesViewModel
            {
                Categories = this.categoriesRepository
                                 .All()
                                 .Select(x => new OutputCategoriesViewModel
                                 {
                                     Name = x.Name,
                                     MoviesCount = this.moviesCategoriesRepository
                                                      .AllAsNoTracking()
                                                      .Where(y => y.Category.Name == x.Name)
                                                      .Count(),
                                 })
                                 .ToList(),
            };
            return categories;
        }

        public bool IsExisting(string name)
        {
            bool isExisting = this.categoriesRepository.All().Any(x => x.Name == name);
            return isExisting;
        }

        public InputCreateCategoryViewModel GetCategoryForEdit(string category)
        {
            return this.categoriesRepository
                       .All()
                       .Where(x => x.Name == category)
                       .Select(x => new InputCreateCategoryViewModel
                       {
                           Name = x.Name,
                       })
                       .FirstOrDefault();
        }

        public async Task EditCategoryAsync(string category, InputCreateCategoryViewModel model)
        {
            var currentCategory = this.categoriesRepository
                                       .All()
                                       .Where(x => x.Name == category)
                                       .FirstOrDefault();
            currentCategory.Name = model.Name;
            await this.categoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.categoriesRepository.All()
                                            .Select(x => new
                                            {
                                                x.Id,
                                                x.Name,
                                            })
                                            .OrderBy(x => x.Name)
                                            .ToList()
                                            .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
