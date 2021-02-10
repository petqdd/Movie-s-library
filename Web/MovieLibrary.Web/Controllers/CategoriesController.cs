namespace MovieLibrary.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Categories;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(InputCreateCategoryViewModel category)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else
            {
                if (!this.categoriesService.IsExisting(category.Name))
                {
                    await this.categoriesService.CreateCategoryAsync(category);
                    return this.Redirect("/");
                }

                return this.Redirect("/Categories/Error");
            }
        }

        public IActionResult Error()
        {
            return this.View();
        }

        public IActionResult All()
        {
            var categoryViewModel = this.categoriesService.GetAllCategories();
            return this.View(categoryViewModel);
        }

        [HttpGet]
        public IActionResult Edit(string category)
        {
            var viewModel = this.categoriesService.GetCategoryForEdit(category);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string category, InputCreateCategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else
            {
                if (!this.categoriesService.IsExisting(model.Name))
                {
                    await this.categoriesService.EditCategoryAsync(category, model);
                    return this.Redirect("/Categories/All");
                }

                return this.Redirect("/Categories/Error");
            }
        }
    }
}
