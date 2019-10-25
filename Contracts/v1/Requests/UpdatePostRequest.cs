using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Contracts.v1.Requests
{
    public class UpdatePostRequest
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string Date { get; set; } = DateTime.Now.ToString("yyyy-MM-dd, HH:mm");

        public string Image { get; set; }

        public string Content { get; set; }

        public string Tags { get; set; }
    }
}
