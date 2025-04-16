using Gee.Core.Domain;
using Gee.Core;


    public partial class CategoryTemplate : TenantBaseEntity<int,int,int>,ISoftDeletedEntity
    {
        /// <summary>
        /// Gets or sets the template name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the view path
        /// </summary>
        public string ViewPath { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int? DisplayOrder { get; set; }
        public bool Deleted { get; set; }
   }
