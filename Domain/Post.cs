using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication1.Domain
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        // Who created post.
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User  { get; set; }

        public ICollection<PostCategory> PostCategories { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public string Date { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Tags { get; set; }

        public string Status { get; set; } = "Draft";

    }
}
