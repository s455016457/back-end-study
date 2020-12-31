using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyEntityFramework2
{
    public class Services
    {
        private static BlogggingContext _blogContext = new BlogggingContext();
        public BlogggingContext BlogContext { get { return _blogContext; } }

        public bool AddBlog(Blog blog)
        {
            _blogContext.Blogs.Add(blog);
            _blogContext.SaveChanges();
            return true;
        }

        public IOrderedQueryable<Blog> QueryBlog()
        {
            var query = from b in _blogContext.Blogs
                        orderby b.BlogId
                        select b;
            return query;
        }
    }
}
