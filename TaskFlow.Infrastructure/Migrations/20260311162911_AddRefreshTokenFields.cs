using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Use IF NOT EXISTS guards so this migration is safe even if
            // columns were already added manually or by a previous run.
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.columns
                    WHERE object_id = OBJECT_ID(N'dbo.RefreshTokens') AND name = 'CreatedByIp'
                )
                BEGIN
                    ALTER TABLE [dbo].[RefreshTokens]
                    ADD [CreatedByIp] nvarchar(max) NULL;
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.columns
                    WHERE object_id = OBJECT_ID(N'dbo.RefreshTokens') AND name = 'ReplacedByToken'
                )
                BEGIN
                    ALTER TABLE [dbo].[RefreshTokens]
                    ADD [ReplacedByToken] nvarchar(max) NULL;
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.columns
                    WHERE object_id = OBJECT_ID(N'dbo.RefreshTokens') AND name = 'RevokedAt'
                )
                BEGIN
                    ALTER TABLE [dbo].[RefreshTokens]
                    ADD [RevokedAt] datetime2 NULL;
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.columns
                    WHERE object_id = OBJECT_ID(N'dbo.RefreshTokens') AND name = 'RevokedByIp'
                )
                BEGIN
                    ALTER TABLE [dbo].[RefreshTokens]
                    ADD [RevokedByIp] nvarchar(max) NULL;
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByIp",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "ReplacedByToken",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "RevokedByIp",
                table: "RefreshTokens");
        }
    }
}
