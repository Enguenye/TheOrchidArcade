using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheOrchidArchade.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    revenue = table.Column<double>(type: "float", nullable: true),
                    isDeveloper = table.Column<bool>(type: "bit", nullable: false),
                    creditCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Revenue = table.Column<double>(type: "float", nullable: true),
                    DeveloperId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DownloadUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_games_AspNetUsers_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reviews_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "creditCardNumber", "isDeveloper", "revenue" },
                values: new object[,]
                {
                    { "1b467a74-ead5-4f49-b26b-7888348b34f3", 0, "2e28e301-5b95-44e5-ba2d-cc16fed215c8", "dev1@test.com", false, false, null, null, null, null, null, false, "369a351f-de5d-4217-b23a-ea3c6b3403eb", false, "Developer1", null, true, 0.0 },
                    { "335924cd-2032-4617-9727-82f430250505", 0, "a5705548-baa0-4ef2-b0c8-8c151973322f", "client1@test.com", false, false, null, null, null, "AQAAAAIAAYagAAAAEGZ6Z2QZrR0CXOLW8i95LxiUw2kOGmEmVcPWMN31PUmZFacT7AEg3QdKvZtPsEPTrw==", null, false, "e5fba439-fce3-455b-9490-e6db020fdbcf", false, "Client1", null, false, 0.0 },
                    { "534ea39b-b93b-444c-846c-a2d97eb0d8ca", 0, "2577aaf4-9a7d-4e08-b176-cb35538c4067", "client3@test.com", false, false, null, null, null, "AQAAAAIAAYagAAAAEGZ6Z2QZrR0CXOLW8i95LxiUw2kOGmEmVcPWMN31PUmZFacT7AEg3QdKvZtPsEPTrw==", null, false, "35194141-3e90-4bb5-96ab-ad4d8c65069f", false, "Client3", null, false, 0.0 },
                    { "a176f533-5e1f-43a9-b0e4-03633b5eb945", 0, "0fb764fa-a6bb-47a7-8e40-6f44f3c346ea", "dev2@test.com", false, false, null, null, null, "AQAAAAIAAYagAAAAEGZ6Z2QZrR0CXOLW8i95LxiUw2kOGmEmVcPWMN31PUmZFacT7AEg3QdKvZtPsEPTrw==", null, false, "245ad8b2-ce95-4d77-bfd6-9e112861adab", false, "Developer2", null, true, 0.0 },
                    { "b3670cd6-bb24-42d5-bd9f-55be4da6220d", 0, "5a991975-7f15-4165-943f-6d45025648a7", "client2@test.com", false, false, null, null, null, "AQAAAAIAAYagAAAAEGZ6Z2QZrR0CXOLW8i95LxiUw2kOGmEmVcPWMN31PUmZFacT7AEg3QdKvZtPsEPTrw==", null, false, "e9aa2f6c-9625-4c33-8eaf-77effb41213c", false, "Client2", null, false, 0.0 },
                    { "c7286475-489f-4ed5-b65c-1ee1367cfcf7", 0, "8fd0aeb4-ec75-4e86-b735-298a214b3f90", "dev3@test.com", false, false, null, null, null, "AQAAAAIAAYagAAAAEGZ6Z2QZrR0CXOLW8i95LxiUw2kOGmEmVcPWMN31PUmZFacT7AEg3QdKvZtPsEPTrw==", null, false, "15b26fd7-fd3c-49e2-bc4a-709769e70f65", false, "Developer3", null, true, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "games",
                columns: new[] { "Id", "CoverImage", "Description", "DeveloperId", "DownloadUrl", "Genre", "Price", "Revenue", "Title" },
                values: new object[,]
                {
                    { "2cb3f9cc-3ae8-43ab-bd5a-54d487f53454", "https://i.imgur.com/QmpRPyE.png", "The frogs got teleported away from their home. Grow or shrink the objects of the environment to help them get back.", "a176f533-5e1f-43a9-b0e4-03633b5eb945", "https://enguenye.itch.io/froggyfreeway", "Puzzles", 0.0, 0.0, "Froggy Freeway" },
                    { "2fa3c2ca-9c74-45dc-82ff-74628c7380f4", "https://img.itch.zone/aW1nLzEzNzI4MTM2LnBuZw==/original/xZxaEf.png", "The IRS Requisitioned all of your bees! Fight through hordes of government employees in an epic battle of the birds and the bees.", "a176f533-5e1f-43a9-b0e4-03633b5eb945", "https://magicow.itch.io/1098-bee", "Puzzles", 15.6, 0.0, "1098-Bee" },
                    { "69418d94-2854-4dc4-a685-3359f988ab70", "https://i.imgur.com/LWjv79J.png", "The cascade, your home, has been invaded by monsters . Defeat the monsters and purify the cascade back to its normal state.", "1b467a74-ead5-4f49-b26b-7888348b34f3", "https://drive.google.com/file/d/1kEO_qjonwWC4CIQxBDLAy2qfe1n1gl57/view?usp=drive_link", "Bullet hell", 0.0, 0.0, "Purification" },
                    { "a984c3ad-17a2-4f85-a785-49ee5a5a5dfe", "https://i.imgur.com/8pVgqIR.png", "You're a hacker alien trying to escape a giant ariship. Use your knowledge and your abilities to escape this misterious place.Use the arrows to move and z to open your console.", "1b467a74-ead5-4f49-b26b-7888348b34f3", "https://drive.google.com/file/d/13VCHbObQlezmir5Q4e2wFdb-pVE-LC_u/view?usp=drive_link", "Puzzles", 10.300000000000001, 0.0, "Leave" },
                    { "ce6b8f37-9d64-47aa-8b64-7efafea4feec", "https://i.imgur.com/sWw7n5u.png", "Explore a haunted house riddled with monsters, puzzles and secrets trying to unravel the mysteries of this place.", "1b467a74-ead5-4f49-b26b-7888348b34f3", "https://drive.google.com/file/d/1VJk1Vq4bvS6INPSuOSu2eF3uJmTxRdgo/view?usp=drive_link", "Horror", 5.0, 0.0, "TheAcumulator" },
                    { "eff4b4e5-183b-4996-ace8-37fc3c4cd4d2", "https://i.imgur.com/ikEFkww.png", "You're a Monkey on a Monocycle", "1b467a74-ead5-4f49-b26b-7888348b34f3", "https://drive.google.com/file/d/1bAt1OLeVvi3Dr5yF6eWzaFXSJLy_YJjM/view?usp=sharing", "Platformer", 4.0, 0.0, "MonoCycle" }
                });

            migrationBuilder.InsertData(
                table: "reviews",
                columns: new[] { "Id", "Description", "GameId", "Rating", "UserId" },
                values: new object[,]
                {
                    { "018c485a-3b2d-41ad-9f17-13dd4b18318a", "Awesome game, I loved the Alien", "a984c3ad-17a2-4f85-a785-49ee5a5a5dfe", 5, "335924cd-2032-4617-9727-82f430250505" },
                    { "1b6b6e5e-ff6d-4993-b64e-1ae38602550d", "Pretty terrifying", "ce6b8f37-9d64-47aa-8b64-7efafea4feec", 5, "335924cd-2032-4617-9727-82f430250505" },
                    { "5a241740-9d97-4357-b4ee-ae19eb9f6a17", "Pretty cool concept", "69418d94-2854-4dc4-a685-3359f988ab70", 4, "335924cd-2032-4617-9727-82f430250505" },
                    { "98171394-4c50-48d7-bd64-7439c03a7d94", "This game sucks!!!", "eff4b4e5-183b-4996-ace8-37fc3c4cd4d2", 1, "335924cd-2032-4617-9727-82f430250505" },
                    { "e56af216-9e43-4452-8169-a5c0599d1ebd", "Was not my cup of tea", "a984c3ad-17a2-4f85-a785-49ee5a5a5dfe", 2, "b3670cd6-bb24-42d5-bd9f-55be4da6220d" }
                });

            migrationBuilder.InsertData(
                table: "transactions",
                columns: new[] { "Id", "Date", "GameId", "UserId" },
                values: new object[,]
                {
                    { "0315ebe4-0805-497e-b104-2f9e4709af0b", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2fa3c2ca-9c74-45dc-82ff-74628c7380f4", "335924cd-2032-4617-9727-82f430250505" },
                    { "1f124ae7-1b0c-4288-90a9-9ae78a14b76b", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "69418d94-2854-4dc4-a685-3359f988ab70", "335924cd-2032-4617-9727-82f430250505" },
                    { "42ac79f6-4c30-45e3-9f56-51b2b2ede5eb", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a984c3ad-17a2-4f85-a785-49ee5a5a5dfe", "335924cd-2032-4617-9727-82f430250505" },
                    { "8b8049e8-f333-4bfe-bea9-db0d432cfe60", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ce6b8f37-9d64-47aa-8b64-7efafea4feec", "335924cd-2032-4617-9727-82f430250505" },
                    { "beb7b566-bc1a-4513-9f82-a47f7832db17", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "eff4b4e5-183b-4996-ace8-37fc3c4cd4d2", "335924cd-2032-4617-9727-82f430250505" },
                    { "c06b9f1e-95d5-45d3-844e-f8e2b2682aa8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2cb3f9cc-3ae8-43ab-bd5a-54d487f53454", "335924cd-2032-4617-9727-82f430250505" },
                    { "d1340b84-0565-4b53-a8e5-410d82ef6ad6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ce6b8f37-9d64-47aa-8b64-7efafea4feec", "b3670cd6-bb24-42d5-bd9f-55be4da6220d" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_games_DeveloperId",
                table: "games",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_GameId",
                table: "reviews",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_UserId",
                table: "reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_GameId",
                table: "transactions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserId",
                table: "transactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
