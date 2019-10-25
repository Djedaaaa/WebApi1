using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Domain;

namespace WebApplication1.Services
{
    // 12
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _dataContext.Posts.OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.Posts.AddAsync(post);

            var created = await _dataContext.SaveChangesAsync();

            return created < 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            // Doesn't instantly update, it says, the next time you save i want you to update record.
            _dataContext.Posts.Update(postToUpdate);

            // Returns int, count of how many records where updated.
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            // Theres no direct deleting, first select record, then delete it.
            var post = await GetPostByIdAsync(postId);

            if (post == null)
                return false;

            _dataContext.Posts.Remove(post);

            // Returns int, count of how many records where deleted.
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

            if (post == null)
                return false;

            // This line defines if user that wants to manipulate the post is the different from the user that created the post, if true, return false
            if (post.UserId != userId)
                return false;

            return true;

        }
        public async Task<List<PostCategory>> GetPostsCategoriesAsync()
        {
            var postsToReturn = await (from post in _dataContext.Posts
                orderby post.Date descending
                select new
                {
                    Id = post.Id,
                    Title = post.Title,
                    Author = post.Author,
                    Date = post.Date,
                    Image = post.Image,
                    Content = post.Content,
                    Tags = post.Tags,
                    Status = post.Status,
                    CategoriesIds = (from postCategory in post.PostCategories
                        join category in _dataContext.Categories
                            on postCategory.CategoryId
                            equals category.CategoryId
                        select category.CategoryId).ToList()
                }).ToListAsync();

            return await _dataContext.PostsCategories.ToListAsync();
        }
    }
}
