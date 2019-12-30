using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Helpers
{
    public class Paginate<TEntity> : ApiResult where TEntity : class
    {

        private IEnumerable<TEntity> _data;
        public long PageSize { get; set; }
        public long TotalPages
        {
            get
            {
                long mod = (TotalRegs == 0 || PageSize == 0 ? 0 : TotalRegs % PageSize);
                return (TotalRegs == 0 || PageSize == 0 ? 0 : (TotalRegs / PageSize)) + (mod > 0 ? 1 : 0);
            }
        }
        public long PageNumber { get; set; }

        public long TotalRegs { get; set; }
        public IEnumerable<TEntity> Data
        {
            get => _data;
            set => _data = value;
        }
    }
}
