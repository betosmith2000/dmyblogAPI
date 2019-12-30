using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Helpers
{
    public class PaginateParams
    {
        public PaginateParams()
        {

        }
        const int MaxPageSize = 50;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        public int PageNumber { get; set; } = 0;
    }
}
