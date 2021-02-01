namespace MovieLibrary.Web.ViewModels.Ratings
{
    public class OutputRatingsViewModel
    {
        public int Rating { get; set; }

        public int[] Ratings
        {
            get { return new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; }
        }

        public int MovieId { get; set; }

        public bool IsVoted { get; set; }
    }
}
