using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Domain
{
    public class PostCategory
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
