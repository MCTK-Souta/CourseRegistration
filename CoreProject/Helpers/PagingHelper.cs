using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Helpers
{
    public class PagingHelper
    {
        public static int CalculatePages(int totalSize, int pageSize)
        {
            int pages = totalSize / pageSize;

            if (totalSize % pageSize != 0)
                pages += 1;

            return pages;
        }
    }
}
