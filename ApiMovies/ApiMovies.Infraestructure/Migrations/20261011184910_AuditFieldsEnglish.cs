using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMovies.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuditFieldsEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Category_categoriaId",
                table: "Movie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie",
                table: "Movie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Movie",
                newName: "Pelicula");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categoria");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Pelicula",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Pelicula",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_categoriaId",
                table: "Pelicula",
                newName: "IX_Pelicula_categoriaId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categoria",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Categoria",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Pelicula",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pelicula",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Pelicula",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Categoria",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Categoria",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Categoria",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pelicula",
                table: "Pelicula",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categoria",
                table: "Categoria",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pelicula_Categoria_categoriaId",
                table: "Pelicula",
                column: "categoriaId",
                principalTable: "Categoria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pelicula_Categoria_categoriaId",
                table: "Pelicula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pelicula",
                table: "Pelicula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categoria",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Pelicula");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Pelicula");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Pelicula");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Categoria");

            migrationBuilder.RenameTable(
                name: "Pelicula",
                newName: "Movie");

            migrationBuilder.RenameTable(
                name: "Categoria",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Movie",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Movie",
                newName: "FechaCreacion");

            migrationBuilder.RenameIndex(
                name: "IX_Pelicula_categoriaId",
                table: "Movie",
                newName: "IX_Movie_categoriaId");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Category",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Category",
                newName: "FechaCreacion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie",
                table: "Movie",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Category_categoriaId",
                table: "Movie",
                column: "categoriaId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
