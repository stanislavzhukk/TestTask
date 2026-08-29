using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookingRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Halls_HallId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingAmenity_Amenities_AmenityId",
                table: "BookingAmenity");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingAmenity_Booking_BookingId",
                table: "BookingAmenity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingAmenity",
                table: "BookingAmenity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "BookingAmenity",
                newName: "BookingAmenities");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_BookingAmenity_AmenityId",
                table: "BookingAmenities",
                newName: "IX_BookingAmenities_AmenityId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_HallId_StartTime_EndTime",
                table: "Bookings",
                newName: "IX_Bookings_HallId_StartTime_EndTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingAmenities",
                table: "BookingAmenities",
                columns: new[] { "BookingId", "AmenityId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingAmenities_Amenities_AmenityId",
                table: "BookingAmenities",
                column: "AmenityId",
                principalTable: "Amenities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingAmenities_Bookings_BookingId",
                table: "BookingAmenities",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Halls_HallId",
                table: "Bookings",
                column: "HallId",
                principalTable: "Halls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingAmenities_Amenities_AmenityId",
                table: "BookingAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingAmenities_Bookings_BookingId",
                table: "BookingAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Halls_HallId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingAmenities",
                table: "BookingAmenities");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameTable(
                name: "BookingAmenities",
                newName: "BookingAmenity");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_HallId_StartTime_EndTime",
                table: "Booking",
                newName: "IX_Booking_HallId_StartTime_EndTime");

            migrationBuilder.RenameIndex(
                name: "IX_BookingAmenities_AmenityId",
                table: "BookingAmenity",
                newName: "IX_BookingAmenity_AmenityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingAmenity",
                table: "BookingAmenity",
                columns: new[] { "BookingId", "AmenityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Halls_HallId",
                table: "Booking",
                column: "HallId",
                principalTable: "Halls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingAmenity_Amenities_AmenityId",
                table: "BookingAmenity",
                column: "AmenityId",
                principalTable: "Amenities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingAmenity_Booking_BookingId",
                table: "BookingAmenity",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
