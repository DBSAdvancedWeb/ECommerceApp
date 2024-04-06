using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductApi.Migrations
{
    /// <inheritdoc />
    public partial class LoadProductData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] {"Id", "Name","Category", "Description","ImageUrl","Price","DateAdded"},
                values: new object[,]
                {
                    { Guid.NewGuid(), "Fitness Trackers", "Jewellery", "Aliexpress fintness trackers", "imgs/aliexpress-fitness-trackers.jpg", 89.99, DateTime.Now },
                    { Guid.NewGuid(), "Black Bag", "Bags", "Black over the shoulder bag", "imgs/black-bag-over-the-shoulder.jpg", 59.95, DateTime.Now },
                    { Guid.NewGuid(), "Runners","Runners", "Black Sneakers with white sole", "imgs/black-sneakers-with-white-sole.jpg", 40.00, DateTime.Now },
                    { Guid.NewGuid(), "Headphones", "Headphones", "Volume Control Headphones", "imgs/volume-control-headphones.jpg", 179.99, DateTime.Now },
                    { Guid.NewGuid(), "Sport Runners", "Runners", "All black sports runners", "imgs/right-foot-all-black-sneaker.jpg", 44.99, DateTime.Now },
                    { Guid.NewGuid(), "QR Codes", "Tech", "QR Codes In Store", "imgs/qr-codes-in-store.jpg", 2.99, DateTime.Now },
                    { Guid.NewGuid(), "Gemstone Necklace","Jewellery", "Purple Gemstone necklace", "imgs/purple-gemstone-necklace.jpg", 19.95, DateTime.Now },
                    { Guid.NewGuid(), "Modern Watch", "Jewellery", "Modern time piece watch", "imgs/modern-time-pieces.jpg", 45.00, DateTime.Now },
                    { Guid.NewGuid(), "Leather Jacket", "Clothes", "Kids Leather Jacket", "imgs/kids-leather-jacket.jpg", 119.99, DateTime.Now }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                    table: "Products",
                    keyColumn: "Id",
                    keyValues: null);
        }
    }
}
