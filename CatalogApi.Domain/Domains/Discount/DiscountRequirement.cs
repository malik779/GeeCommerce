using Gee.Core;

public partial class DiscountRequirement : TenantBaseEntity<int, int, int>
{
    /// <summary>
    /// Gets or sets the discount identifier
    /// </summary>
    public int DiscountId { get; set; }

    /// <summary>
    /// Gets or sets the discount requirement rule system name
    /// </summary>
    public string DiscountRequirementRuleSystemName { get; set; }

    /// <summary>
    /// Gets or sets the parent requirement identifier
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Gets or sets an interaction type identifier (has a value for the group and null for the child requirements)
    /// </summary>
    public int? InteractionTypeId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this requirement has any child requirements
    /// </summary>
    public bool IsGroup { get; set; }

    /// <summary>
    /// Gets or sets an interaction type
    /// </summary>
    public RequirementGroupInteractionTypeEnum? InteractionType
    {
        get => (RequirementGroupInteractionTypeEnum?)InteractionTypeId;
        set => InteractionTypeId = (int?)value;
    }
}
