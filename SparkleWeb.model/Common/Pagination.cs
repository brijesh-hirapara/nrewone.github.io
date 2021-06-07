using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.Model.Common
{
    public class Pagination
    {

        const int maxPageSize = 1000;
        public int PageNumber { get; set; } = 1;
        private int _pageSize=500;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }

        }
    }
}
