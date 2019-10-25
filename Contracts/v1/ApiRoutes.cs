using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Contracts.v1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        // This class doesn't ever need to be instantiated

        public static class Posts
        {
            public const string GetAll = Base + "/posts";

            public const string Get = Base + "/posts/{postId}";

            public const string GetAllPostsCategories = Base + "/posts/postsCategories";

            public const string GetPostCategories = Base + "/postCategories/{postId}";

            public const string Update = Base + "/posts/{postId}";

            public const string Delete = Base + "/posts/{postId}";

            public const string Create = Base + "/posts";

        }

        public static class Identity {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

            public const string Refresh = Base + "/identity/refresh";
        }
    }
}
