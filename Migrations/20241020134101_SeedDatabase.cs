using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheOrchidArchade.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "revenue",
                table: "users",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "reviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "Revenue",
                table: "games",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "games",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl",
                table: "games",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "games",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImage",
                table: "games",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Username", "isDeveloper", "revenue" },
                values: new object[,]
                {
                    { 1, "dev1@test.com", "123", "Developer1", true, 0.0 },
                    { 2, "dev2@test.com", "123", "Developer2", true, 0.0 },
                    { 3, "dev3@test.com", "123", "Developer3", true, 0.0 },
                    { 4, "client1@test.com", "123", "Client1", false, 0.0 },
                    { 5, "client2@test.com", "123", "Client2", false, 0.0 },
                    { 6, "client3@test.com", "123", "Client3", false, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "games",
                columns: new[] { "Id", "CoverImage", "Description", "DeveloperId", "DownloadUrl", "Genre", "Price", "Revenue", "Title" },
                values: new object[,]
                {
                    { 1, "https://i.imgur.com/8pVgqIR.png", "You're a hacker alien trying to escape a giant ariship. Use your knowledge and your abilities to escape this misterious place.Use the arrows to move and z to open your console.", 1, "https://drive.google.com/file/d/13VCHbObQlezmir5Q4e2wFdb-pVE-LC_u/view?usp=drive_link", "Puzzles", 10.300000000000001, 0.0, "Leave" },
                    { 2, "https://i.imgur.com/LWjv79J.png", "The cascade, your home, has been invaded by monsters . Defeat the monsters and purify the cascade back to its normal state.", 1, "https://drive.google.com/file/d/1kEO_qjonwWC4CIQxBDLAy2qfe1n1gl57/view?usp=drive_link", "Bullet hell", 0.0, 0.0, "Purification" },
                    { 3, "https://i.imgur.com/sWw7n5u.png", "Explore a haunted house riddled with monsters, puzzles and secrets trying to unravel the mysteries of this place.", 1, "https://drive.google.com/file/d/1VJk1Vq4bvS6INPSuOSu2eF3uJmTxRdgo/view?usp=drive_link", "Horror", 5.0, 0.0, "TheAcumulator" },
                    { 4, "https://i.imgur.com/ikEFkww.png", "You're a Monkey on a Monocycle", 1, "https://drive.google.com/file/d/1bAt1OLeVvi3Dr5yF6eWzaFXSJLy_YJjM/view?usp=sharing", "Platformer", 4.0, 0.0, "MonoCycle" },
                    { 5, "https://i.imgur.com/QmpRPyE.png", "The frogs got teleported away from their home. Grow or shrink the objects of the environment to help them get back.", 2, "https://enguenye.itch.io/froggyfreeway", "Puzzles", 0.0, 0.0, "Froggy Freeway" },
                    { 6, "https://img.itch.zone/aW1nLzEzNzI4MTM2LnBuZw==/original/xZxaEf.png", "The IRS Requisitioned all of your bees! Fight through hordes of government employees in an epic battle of the birds and the bees.", 2, "https://magicow.itch.io/1098-bee", "Puzzles", 15.6, 0.0, "1098-Bee" }
                });

            migrationBuilder.InsertData(
                table: "reviews",
                columns: new[] { "Id", "Description", "GameId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1, "Awesome game, I loved the Alien", 1, 5, 4 },
                    { 2, "Pretty cool concept", 2, 4, 4 },
                    { 3, "Pretty terrifying", 3, 5, 4 },
                    { 4, "This game sucks!!!", 4, 1, 4 },
                    { 5, "Was not my cup of tea", 1, 2, 5 }
                });

            migrationBuilder.InsertData(
                table: "transactions",
                columns: new[] { "Id", "Date", "GameId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4 },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4 },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4 },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 4 },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 4 },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 4 },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "reviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "reviews",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "reviews",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "reviews",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "reviews",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "transactions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "transactions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "transactions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "transactions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "transactions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "transactions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "transactions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "games",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "games",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "games",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "games",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "games",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "games",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<double>(
                name: "revenue",
                table: "users",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Revenue",
                table: "games",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl",
                table: "games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoverImage",
                table: "games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
