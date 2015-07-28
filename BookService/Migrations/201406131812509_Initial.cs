namespace BookService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Genre = c.String(),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);

            CreateTable(
                "dbo.GlowHubs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CommandRelations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    GlowHubId = c.Int(nullable: false),
                    Command = c.String(nullable: false),
                    NextUserId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.GlowHubs", t => t.GlowHubId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.NextUserId, cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Books", new[] { "AuthorId" });
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
            DropTable("dbo.Users");
            DropTable("dbo.GlowHubs");
            DropForeignKey("dbo.CommandRelations", "UserId", "dbo.Users");
            DropForeignKey("dbo.CommandRelations", "GlowHubId", "dbo.GlowHubs");
            DropTable("dbo.CommandRelations");
        }
    }
}
