using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Contracts.v1;
using WebApplication1.Contracts.v1.Requests;
using WebApplication1.Contracts.v1.Responses;
using WebApplication1.Domain;
using WebApplication1.Extensions;
using WebApplication1.Services;

namespace WebApplication1.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        private readonly UserManager<IdentityUser> _userManager;

        public PostController(IPostService postService, UserManager<IdentityUser> userManager)
        {
            _postService = postService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetPostsAsync());
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [Authorize(Roles = "Admin, Poster")]
        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
        {
            IdentityUser author =  await _userManager.FindByIdAsync(HttpContext.GetUserId());
            var post = new Post
            {
                Title = request.Title,
                UserId = HttpContext.GetUserId(),
                Author = author.UserName,
                Date = request.Date,
                Image = request.Image,
                Content = request.Content,
                Tags = request.Tags
            };

            await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new PostResponse { Id = post.Id };
            return Created(locationUri, response);
        }

        [Authorize(Roles = "Admin, Poster")]
        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new {error = "You do now own this post."});
            }

            IdentityUser author = await _userManager.FindByIdAsync(HttpContext.GetUserId());
            var post = await _postService.GetPostByIdAsync((postId));
            post.Title = request.Title;
            post.UserId = HttpContext.GetUserId();
            post.Author = author.UserName;
            post.Date = request.Date;
            post.Image = request.Image;
            post.Content = request.Content;
            post.Tags = request.Tags;

            var updated = await _postService.UpdatePostAsync(post);

            if (updated)
                return Ok();

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do now own this post." });
            }

            var deleted = await _postService.DeletePostAsync(postId);

            if (deleted)
                // Successfully deleted.
                return NoContent();

            return NotFound();
        }

        [HttpGet(ApiRoutes.Posts.GetAllPostsCategories)]
        public async Task<IActionResult> GetAllPostsCategories()
        {
            return Ok(await _postService.GetPostsAsync());
        }

    }
}
