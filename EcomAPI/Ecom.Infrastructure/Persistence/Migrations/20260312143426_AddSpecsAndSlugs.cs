using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecom.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecsAndSlugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Products",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Categories",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Brands",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SpecificationKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySpecificationKeys",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    SpecificationKeyId = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySpecificationKeys", x => new { x.CategoryId, x.SpecificationKeyId });
                    table.ForeignKey(
                        name: "fk_categories_category_spec_keys",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_spec_keys_category_spec_keys",
                        column: x => x.SpecificationKeyId,
                        principalTable: "SpecificationKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecificationKeys",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    SpecificationKeyId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecificationKeys", x => new { x.ProductId, x.SpecificationKeyId });
                    table.ForeignKey(
                        name: "fk_products_product_spec_keys",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_spec_keys_product_spec_keys",
                        column: x => x.SpecificationKeyId,
                        principalTable: "SpecificationKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440000"),
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2026, 3, 11, 14, 34, 24, 939, DateTimeKind.Utc).AddTicks(8793), "$2a$11$0BpqjZFT.q60sSuxJMf2xO8TeYEVng5d.xqQHgsBJTI6oZbI0VSNG" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug_Unique",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug_Unique",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Slug_Unique",
                table: "Brands",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategorySpecificationKeys_SpecificationKeyId",
                table: "CategorySpecificationKeys",
                column: "SpecificationKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecificationKeys_SpecificationKeyId",
                table: "ProductSpecificationKeys",
                column: "SpecificationKeyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorySpecificationKeys");

            migrationBuilder.DropTable(
                name: "ProductSpecificationKeys");

            migrationBuilder.DropTable(
                name: "SpecificationKeys");

            migrationBuilder.DropIndex(
                name: "IX_Products_Slug_Unique",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Slug_Unique",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Slug_Unique",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Brands");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440000"),
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2026, 3, 9, 5, 41, 32, 150, DateTimeKind.Utc).AddTicks(667), "$2a$11$Jcq3THwEEsqt929Vx6q1yO0NTFrioO2sRBYo1GDO53.3y5U5xcwum" });
        }
    }
}
