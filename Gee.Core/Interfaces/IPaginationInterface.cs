﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.Interfaces
{
    public partial interface IPaginationInterface
    {
      
            /// <summary>
            /// The current page index (starts from 0)
            /// </summary>
            int PageIndex { get; }
            /// <summary>
            /// The current page number (starts from 1)
            /// </summary>
            int PageNumber { get; }
            /// <summary>
            /// The number of items in each page.
            /// </summary>
            int PageSize { get; }
            /// <summary>
            /// The total number of items.
            /// </summary>
            int TotalItems { get; }
            /// <summary>
            /// The total number of pages.
            /// </summary>
            int TotalPages { get; }
            /// <summary>
            /// The index of the first item in the page.
            /// </summary>
            int FirstItem { get; }
            /// <summary>
            /// The index of the last item in the page.
            /// </summary>
            int LastItem { get; }
            /// <summary>
            /// Whether there are pages before the current page.
            /// </summary>
            bool HasPreviousPage { get; }
            /// <summary>
            /// Whether there are pages after the current page.
            /// </summary>
            bool HasNextPage { get; }
        }


        /// <summary>
        /// Generic form of <see cref="IPageableModel"/>
        /// </summary>
        /// <typeparam name="T">Type of object being paged</typeparam>
        public partial interface IPagination<T> : IPaginationInterface
    {

        }
  }

