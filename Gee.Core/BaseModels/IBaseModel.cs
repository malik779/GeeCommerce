
    public interface IBaseModel
    {
        public string? Name { get; set; }


        public string? Description { get; set; }

        public string? MetaKeywords { get; set; }


        public string? MetaDescription { get; set; }


        public string? MetaTitle { get; set; }


        public string? SeName { get; set; }
        public int PageSize { get; set; }


        public bool AllowCustomersToSelectPageSize { get; set; }


        public string? PageSizeOptions { get; set; }
        public bool PriceRangeFiltering { get; set; }


        public decimal PriceFrom { get; set; }


        public decimal PriceTo { get; set; }
        public bool ShowOnHomepage { get; set; }


        public bool IncludeInTopMenu { get; set; }

        public bool Published { get; set; }


        public bool Deleted { get; set; }

        public bool ManuallyPriceRange { get; set; }

    }
