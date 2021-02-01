namespace MovieLibrary.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class AllCategoriesViewModel
    {
        public IEnumerable<OutputCategoriesViewModel> Categories { get; set; }
    }
}
