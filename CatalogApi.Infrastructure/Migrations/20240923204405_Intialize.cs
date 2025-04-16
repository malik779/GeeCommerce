using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogApi.Service.Migrations
{
    /// <inheritdoc />
    public partial class Intialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    ParentGroupedProductId = table.Column<int>(type: "int", nullable: false),
                    VisibleIndividually = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTemplateId = table.Column<int>(type: "int", nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: true),
                    ShowOnHomepage = table.Column<bool>(type: "bit", nullable: false),
                    MetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowCustomerReviews = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedRatingSum = table.Column<int>(type: "int", nullable: false),
                    NotApprovedRatingSum = table.Column<int>(type: "int", nullable: false),
                    ApprovedTotalReviews = table.Column<int>(type: "int", nullable: false),
                    NotApprovedTotalReviews = table.Column<int>(type: "int", nullable: false),
                    SubjectToAcl = table.Column<bool>(type: "bit", nullable: false),
                    LimitedToStores = table.Column<bool>(type: "bit", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturerPartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gtin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGiftCard = table.Column<bool>(type: "bit", nullable: false),
                    GiftCardTypeId = table.Column<int>(type: "int", nullable: false),
                    OverriddenGiftCardAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequireOtherProducts = table.Column<bool>(type: "bit", nullable: false),
                    RequiredProductIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutomaticallyAddRequiredProducts = table.Column<bool>(type: "bit", nullable: false),
                    IsDownload = table.Column<bool>(type: "bit", nullable: false),
                    DownloadId = table.Column<int>(type: "int", nullable: false),
                    UnlimitedDownloads = table.Column<bool>(type: "bit", nullable: false),
                    MaxNumberOfDownloads = table.Column<int>(type: "int", nullable: false),
                    DownloadExpirationDays = table.Column<int>(type: "int", nullable: true),
                    DownloadActivationTypeId = table.Column<int>(type: "int", nullable: false),
                    HasSampleDownload = table.Column<bool>(type: "bit", nullable: false),
                    SampleDownloadId = table.Column<int>(type: "int", nullable: false),
                    HasUserAgreement = table.Column<bool>(type: "bit", nullable: false),
                    UserAgreementText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurringCycleLength = table.Column<int>(type: "int", nullable: false),
                    RecurringCyclePeriodId = table.Column<int>(type: "int", nullable: false),
                    RecurringTotalCycles = table.Column<int>(type: "int", nullable: false),
                    IsRental = table.Column<bool>(type: "bit", nullable: false),
                    RentalPriceLength = table.Column<int>(type: "int", nullable: false),
                    RentalPricePeriodId = table.Column<int>(type: "int", nullable: false),
                    IsShipEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsFreeShipping = table.Column<bool>(type: "bit", nullable: false),
                    ShipSeparately = table.Column<bool>(type: "bit", nullable: false),
                    AdditionalShippingCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryDateId = table.Column<int>(type: "int", nullable: false),
                    IsTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    TaxCategoryId = table.Column<int>(type: "int", nullable: false),
                    ManageInventoryMethodId = table.Column<int>(type: "int", nullable: false),
                    ProductAvailabilityRangeId = table.Column<int>(type: "int", nullable: false),
                    UseMultipleWarehouses = table.Column<bool>(type: "bit", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    DisplayStockAvailability = table.Column<bool>(type: "bit", nullable: false),
                    DisplayStockQuantity = table.Column<bool>(type: "bit", nullable: false),
                    MinStockQuantity = table.Column<int>(type: "int", nullable: false),
                    LowStockActivityId = table.Column<int>(type: "int", nullable: false),
                    NotifyAdminForQuantityBelow = table.Column<int>(type: "int", nullable: false),
                    BackorderModeId = table.Column<int>(type: "int", nullable: false),
                    AllowBackInStockSubscriptions = table.Column<bool>(type: "bit", nullable: false),
                    OrderMinimumQuantity = table.Column<int>(type: "int", nullable: false),
                    OrderMaximumQuantity = table.Column<int>(type: "int", nullable: false),
                    AllowedQuantities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowAddingOnlyExistingAttributeCombinations = table.Column<bool>(type: "bit", nullable: false),
                    DisplayAttributeCombinationImagesOnly = table.Column<bool>(type: "bit", nullable: false),
                    NotReturnable = table.Column<bool>(type: "bit", nullable: false),
                    DisableBuyButton = table.Column<bool>(type: "bit", nullable: false),
                    DisableWishlistButton = table.Column<bool>(type: "bit", nullable: false),
                    AvailableForPreOrder = table.Column<bool>(type: "bit", nullable: false),
                    PreOrderAvailabilityStartDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CallForPrice = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerEntersPrice = table.Column<bool>(type: "bit", nullable: false),
                    MinimumCustomerEnteredPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumCustomerEnteredPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BasepriceEnabled = table.Column<bool>(type: "bit", nullable: false),
                    BasepriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BasepriceUnitId = table.Column<int>(type: "int", nullable: false),
                    BasepriceBaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BasepriceBaseUnitId = table.Column<int>(type: "int", nullable: false),
                    MarkAsNew = table.Column<bool>(type: "bit", nullable: false),
                    MarkAsNewStartDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarkAsNewEndDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasTierPrices = table.Column<bool>(type: "bit", nullable: false),
                    HasDiscountsApplied = table.Column<bool>(type: "bit", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableStartDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableEndDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    BackorderMode = table.Column<int>(type: "int", nullable: false),
                    DownloadActivationType = table.Column<int>(type: "int", nullable: false),
                    GiftCardType = table.Column<int>(type: "int", nullable: false),
                    LowStockActivity = table.Column<int>(type: "int", nullable: false),
                    ManageInventoryMethod = table.Column<int>(type: "int", nullable: false),
                    RecurringCyclePeriod = table.Column<int>(type: "int", nullable: false),
                    RentalPricePeriod = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
