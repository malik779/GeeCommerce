﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseModels
{
    public class BaseSearchModel
    {
        #region Ctor

        public BaseSearchModel()
        {
            //set the default values
            Start = 1;
            Length = 10;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a page number
        /// </summary>
        public int Page => (Start!=0? (Start / Length) + 1 : 1);

        /// <summary>
        /// Gets a page size
        /// </summary>
        public int PageSize => Length;

        /// <summary>
        /// Gets or sets a comma-separated list of available page sizes
        /// </summary>
        public string AvailablePageSizes { get; set; }

        /// <summary>
        /// Gets or sets draw. Draw counter. This is used by DataTables to ensure that the Ajax returns from server-side processing requests are drawn in sequence by DataTables (Ajax requests are asynchronous and thus can return out of sequence).
        /// </summary>
        public string Draw { get; set; }

        /// <summary>
        /// Gets or sets skipping number of rows count. Paging first record indicator.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets paging length. Number of records that the table can display in the current draw. 
        /// </summary>
        public int Length { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Set grid page parameters
        /// </summary>
        public void SetGridPageSize()
        {
            //var adminAreaSettings = EngineContext.Current.Resolve<AdminAreaSettings>();
            //SetGridPageSize(adminAreaSettings?.DefaultGridPageSize ?? 0, adminAreaSettings?.GridPageSizes);
            SetGridPageSize(10, "10");
        }

        /// <summary>
        /// Set popup grid page parameters
        /// </summary>
        public void SetPopupGridPageSize()
        {
            //var adminAreaSettings = EngineContext.Current.Resolve<AdminAreaSettings>();
            //SetGridPageSize(adminAreaSettings.PopupGridPageSize, adminAreaSettings.GridPageSizes);
            SetGridPageSize(10, "10");
        }

        /// <summary>
        /// Set grid page parameters
        /// </summary>
        /// <param name="pageSize">Page size; pass null to use default value</param>
        /// <param name="availablePageSizes">Available page sizes; pass null to use default value</param>
        public void SetGridPageSize(int pageSize, string availablePageSizes = null)
        {
            Start = 0;
            Length = pageSize;
            AvailablePageSizes = availablePageSizes;
        }
        public int Id { get; set; }
        #endregion
    }
}
