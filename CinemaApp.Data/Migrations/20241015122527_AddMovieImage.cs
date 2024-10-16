
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("456bca7c-f5ed-4f04-b605-862243a9542d"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("61d25010-c811-4598-8305-48f6b012ac52"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("d97f2700-da76-4a92-b0b6-a2dee595adbd"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("c355b716-b854-4b3a-9979-b4d794a4c8bf"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("ff55476a-9c0e-4a33-910c-c0ee670075f6"));

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Movies",
                type: "nvarchar(2083)",
                maxLength: 2083,
                nullable: true,
                defaultValue: "~/images/noImgPhoto.jpg");

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("047f7f15-4794-41ce-ae5b-41ec91f0469d"), "Plovdiv", "Cinema City" },
                    { new Guid("2f44344e-33bb-4ee1-beba-40ffb4ce273d"), "Varna", "Cinemax" },
                    { new Guid("3966ea65-0d31-407a-9120-7a428bae2fdc"), "Karlovo", "Cinema Lorld" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Director", "Duration", "Genre", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("62073de5-4164-4b42-bfd6-eed4ee041c51"), "An amazing movie", "Mike Newel", 157, "Fantasy", new DateTime(2005, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Poter" },
                    { new Guid("e4f299d6-589e-49d7-8c31-f1c1aa32fbd7"), "An amazing movie", "Peter Huston", 178, "Fantasy", new DateTime(2001, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lord of the Rings" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("047f7f15-4794-41ce-ae5b-41ec91f0469d"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("2f44344e-33bb-4ee1-beba-40ffb4ce273d"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("3966ea65-0d31-407a-9120-7a428bae2fdc"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("62073de5-4164-4b42-bfd6-eed4ee041c51"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("e4f299d6-589e-49d7-8c31-f1c1aa32fbd7"));

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Movies");

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("456bca7c-f5ed-4f04-b605-862243a9542d"), "Karlovo", "Cinema Lorld" },
                    { new Guid("61d25010-c811-4598-8305-48f6b012ac52"), "Plovdiv", "Cinema City" },
                    { new Guid("d97f2700-da76-4a92-b0b6-a2dee595adbd"), "Varna", "Cinemax" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Director", "Duration", "Genre", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("c355b716-b854-4b3a-9979-b4d794a4c8bf"), "An amazing movie", "Peter Huston", 178, "Fantasy", new DateTime(2001, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lord of the Rings" },
                    { new Guid("ff55476a-9c0e-4a33-910c-c0ee670075f6"), "An amazing movie", "Mike Newel", 157, "Fantasy", new DateTime(2005, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Poter" }
                });
        }
    }
}
