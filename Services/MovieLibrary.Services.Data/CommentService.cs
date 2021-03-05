namespace MovieLibrary.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Comments;

    public class CommentService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<MoviesComment> moviesCommentsRepository;
        private readonly IRepository<UsersComment> usersCommentsRepository;

        public CommentService(
            IDeletableEntityRepository<Comment> commentsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<MoviesComment> moviesCommentsRepository,
            IRepository<UsersComment> usersCommentsRepository)
        {
            this.commentsRepository = commentsRepository;
            this.usersRepository = usersRepository;
            this.moviesCommentsRepository = moviesCommentsRepository;
            this.usersCommentsRepository = usersCommentsRepository;
        }

        public async Task CreateCommentAsync(InputCommentViewModel model, string userId)
        {
            var comment = new Comment
            {
                Content = model.Content,
                CreateDate = DateTime.UtcNow,
                IsDeleted = false,
            };
            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();

            var commentId = comment.Id;
            await this.usersCommentsRepository.AddAsync(new UsersComment
            {
                CommentId = commentId,
                UserId = userId,
            });
            await this.moviesCommentsRepository.AddAsync(new MoviesComment
            {
                CommentId = commentId,
                MovieId = model.Id,
            });
            await this.commentsRepository.SaveChangesAsync();
        }

        public OutputAllCommentsViewModel GetAllComment(int movieId, int page, int itemPerPage)
        {
            var currentMoviesComments = this.moviesCommentsRepository
                                            .AllAsNoTracking()
                                            .Where(x => x.MovieId == movieId && x.Comments.IsDeleted == false)
                                            .OrderByDescending(x => x.CreatedOn)
                                            .Skip((page - 1) * itemPerPage)
                                            .Take(itemPerPage)
                                            .Select(x => new OutputCommentViewModel
                                            {
                                                CommentId = x.CommentId,
                                                CommentContent = x.Comments.Content,
                                                UserId = this.usersCommentsRepository
                                                                          .AllAsNoTracking()
                                                                          .Where(y => y.CommentId == x.CommentId)
                                                                          .Select(y => y.UserId)
                                                                          .FirstOrDefault(),
                                                CreatedDate = x.Comments.CreateDate,
                                            })
                                            .ToList();
            var comments = new OutputAllCommentsViewModel
            {
                Comments = currentMoviesComments,
                MovieId = movieId,
            };

            foreach (var comment in comments.Comments)
            {
                var userId = comment.UserId;
                var userEmail = this.usersRepository
                                    .AllAsNoTracking()
                                    .Where(x => x.Id == userId)
                                    .Select(x => x.Email)
                                    .FirstOrDefault();
                //var userPhoto = this.usersRepository
                //                    .AllAsNoTracking()
                //                    .Where(x => x.Id == userId)
                //                    .Select(x => x.Photo)
                //                    .FirstOrDefault();
                comment.UserEmail = userEmail;
                //comment.UserPhoto = userPhoto;
            }

            return comments;
        }

        public OutputAllComentsAndMoviesViewModel GetAllComment(int page, int itemPerPage)
        {
            var currentMoviesComments = this.moviesCommentsRepository
                                            .AllAsNoTracking()
                                            .Where(x => x.Comments.IsDeleted == false)
                                            .OrderByDescending(x => x.CreatedOn)
                                            .Skip((page - 1) * itemPerPage)
                                            .Take(itemPerPage)
                                            .Select(x => new OutputCommentAndMovieViewModel
                                            {
                                                MovieId = x.MovieId,
                                                MovieName = x.Movie.Name,
                                                CommentId = x.CommentId,
                                                CommentContent = x.Comments.Content,
                                                UserId = this.usersCommentsRepository
                                                             .AllAsNoTracking()
                                                             .Where(y => y.CommentId == x.CommentId)
                                                             .Select(y => y.UserId)
                                                             .FirstOrDefault(),
                                                CreatedDate = x.Comments.CreateDate,
                                            })
                                            .ToList();
            var comments = new OutputAllComentsAndMoviesViewModel
            {
                Comments = currentMoviesComments,
            };

            foreach (var comment in comments.Comments)
            {
                var userId = comment.UserId;
                var userEmail = this.usersRepository
                                    .AllAsNoTracking()
                                    .Where(x => x.Id == userId)
                                    .Select(x => x.Email)
                                    .FirstOrDefault();
                //var userPhoto = this.usersRepository
                //                    .AllAsNoTracking()
                //                    .Where(x => x.Id == userId)
                //                    .Select(x => x.Photo)
                //                    .FirstOrDefault();
                comment.UserEmail = userEmail;
                //comment.UserPhoto = userPhoto;
            }

            return comments;
        }

        public int GetCommentsCount(int id)
        {
            int count = this.moviesCommentsRepository
                            .AllAsNoTracking()
                            .Where(x => x.MovieId == id && x.Comments.IsDeleted == false)
                            .Count();
            return count;
        }

        public int GetAllCommentsCount()
        {
            var count = this.moviesCommentsRepository
                            .AllAsNoTracking()
                            .Where(x => x.Comments.IsDeleted == false)
                            .Count();
            return count;
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = this.commentsRepository
                              .AllAsNoTracking()
                              .Where(x => x.Id == commentId)
                              .FirstOrDefault();
            this.commentsRepository.Delete(comment);
            await this.commentsRepository.SaveChangesAsync();
        }
    }
}
