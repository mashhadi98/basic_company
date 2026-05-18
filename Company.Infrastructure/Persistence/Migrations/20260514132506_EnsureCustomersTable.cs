using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EnsureCustomersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.Customers', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Customers](
        [Id] uniqueidentifier NOT NULL,
        [CompanyName] nvarchar(200) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [CompanyImage] nvarchar(512) NULL,
        [SortOrder] int NOT NULL DEFAULT 0,
        [IsPublished] bit NOT NULL DEFAULT 0,
        [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] datetime2 NULL,
        [CreatedBy] nvarchar(256) NULL,
        [UpdatedBy] nvarchar(256) NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_Customers_IsPublished] ON [dbo].[Customers] ([IsPublished]);
    CREATE INDEX [IX_Customers_SortOrder] ON [dbo].[Customers] ([SortOrder]);
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.Customers', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Customers];
END");
        }
    }
}
