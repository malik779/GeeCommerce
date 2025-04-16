using System.ComponentModel.DataAnnotations;



    public class DiscountMapping
    {
        /// <summary>
        /// Gets the entity identifier
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the discount identifier
        /// </summary>
        public int DiscountId { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int EntityId { get; set; }
    }
    public class AppliedDiscountMapping
    {
        /// <summary>
        /// Gets the entity identifier
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the discount identifier
        /// </summary>
        public int DiscountId { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public Discount Discount { get; set; }
    }