using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyEntityFramework
{
    public class BlogService
    {
        private static BloggingContext _blgContext = new BloggingContext();
        public BloggingContext BlgContext { get { return _blgContext; } }

        public bool Add(Blogs blog)
        {
            BlogService._blgContext.Blogs.Add(blog);
            BlogService._blgContext.SaveChanges();
            return true;
        }

        public IOrderedQueryable<Blogs> Query()
        {
            return from b in BlogService._blgContext.Blogs
                   orderby b.BlogId
                   select b;
        }
    }
}
