﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dnc.AppServices
{
    public class PaginatedList<T>
        : List<T>
        where T : class
    {
        public PaginatedList(int pageNo, int size, IEnumerable<T> items)
        {
            PageIndex = pageNo;
            PageSize = size;
            Total = items.Count();
            PageCount = (Total + PageSize - 1) / PageSize;
            AddRange(items);
        }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int Total { get; private set; }
        public int PageCount { get; private set; }
        public bool HasPrevious => PageIndex >= 2;
        public bool HasNext => PageIndex <= PageCount - 1;
    }
}
