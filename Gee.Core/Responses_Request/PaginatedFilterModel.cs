using Gee.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gee.Core.Responses
{
    public partial record PaginatedFilterModel<T> : Response<T> where T : class
    {
        public PaginatedFilterModel(T data, HttpStatusCode status = 0, string? message = null) : base(data, status, message)
        {
        }

        protected PaginatedFilterModel(Response<T> original) : base(original)
        {
        }

        #region Methods

        /// <summary>
        /// Ctor
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="pagedList">Entities (models)</param>
        public virtual void LoadPagedList<TModel>(IPagedList<TModel> pagedList)
        {
            FirstItem = (pagedList.PageIndex * pagedList.PageSize) + 1;
            HasNextPage = pagedList.HasNextPage;
            HasPreviousPage = pagedList.HasPreviousPage;
            LastItem = Math.Min(pagedList.TotalCount, ((pagedList.PageIndex * pagedList.PageSize) + pagedList.PageSize));
            PageNumber = pagedList.PageIndex + 1;
            PageSize = pagedList.PageSize;
            TotalItems = pagedList.TotalCount;
            TotalPages = pagedList.TotalPages;
        }

        #endregion


        /// <summary>
        /// The current page index (starts from 0)
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (PageNumber > 0)
                    return PageNumber - 1;

                return 0;
            }
        }

        /// <summary>
        /// The current page number (starts from 1)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The number of items in each page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The total number of items.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// The index of the first item in the page.
        /// </summary>
        public int FirstItem { get; set; }

        /// <summary>
        /// The index of the last item in the page.
        /// </summary>
        public int LastItem { get; set; }

        /// <summary>
        /// Whether there are pages before the current page.
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Whether there are pages after the current page.
        /// </summary>
        public bool HasNextPage { get; set; }
        public IDictionary<string, object?> Filters { get; set; } = new Dictionary<string, object?>();
    }
}