namespace Delicious.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Menus", "Dishes_Id", "dbo.Dishes");
            DropForeignKey("dbo.Menus", "Restaurants_Id", "dbo.Restaurants");
            DropForeignKey("dbo.RestaurantsPlaces", "Restaurants_Id", "dbo.Restaurants");
            DropForeignKey("dbo.Orders", "RestaurantsPlaces_Id", "dbo.RestaurantsPlaces");
            DropForeignKey("dbo.RestaurantsPlaces", "Places_Id", "dbo.Places");
            DropForeignKey("dbo.Orders", "Users_Id", "dbo.Users");
            DropIndex("dbo.Menus", new[] { "Dishes_Id" });
            DropIndex("dbo.Menus", new[] { "Restaurants_Id" });
            DropIndex("dbo.RestaurantsPlaces", new[] { "Places_Id" });
            DropIndex("dbo.RestaurantsPlaces", new[] { "Restaurants_Id" });
            DropIndex("dbo.Orders", new[] { "RestaurantsPlaces_Id" });
            DropIndex("dbo.Orders", new[] { "Users_Id" });
            //DropColumn("dbo.Menus", "Dish_Id");
            //DropColumn("dbo.Menus", "Restaurant_Id");
            //DropColumn("dbo.RestaurantsPlaces", "Restaurant_Id");
            //DropColumn("dbo.RestaurantsPlaces", "Place_Id");
            //DropColumn("dbo.Orders", "RestaurantPlace_Id");
            //DropColumn("dbo.Orders", "User_Id");
            //RenameColumn(table: "dbo.Menus", name: "Dishes_Id", newName: "DishId");
            //RenameColumn(table: "dbo.Menus", name: "Restaurants_Id", newName: "RestaurantId");
            //RenameColumn(table: "dbo.RestaurantsPlaces", name: "Restaurants_Id", newName: "RestaurantId");
            //RenameColumn(table: "dbo.Orders", name: "RestaurantsPlaces_Id", newName: "RestaurantPlaceId");
            //RenameColumn(table: "dbo.RestaurantsPlaces", name: "Places_Id", newName: "PlaceId");
            //RenameColumn(table: "dbo.Orders", name: "Users_Id", newName: "UserId");
            //AlterColumn("dbo.Menus", "DishId", c => c.Int(nullable: true));
            //AlterColumn("dbo.Menus", "RestaurantId", c => c.Int(nullable: true));
            //AlterColumn("dbo.RestaurantsPlaces", "PlaceId", c => c.Int(nullable: true));
            //AlterColumn("dbo.RestaurantsPlaces", "RestaurantId", c => c.Int(nullable: true));
            //AlterColumn("dbo.Orders", "RestaurantPlaceId", c => c.Int(nullable: true));
            //AlterColumn("dbo.Orders", "UserId", c => c.Int(nullable: true));
            CreateIndex("dbo.Menus", "RestaurantId");
            CreateIndex("dbo.Menus", "DishId");
            CreateIndex("dbo.RestaurantsPlaces", "RestaurantId");
            CreateIndex("dbo.RestaurantsPlaces", "PlaceId");
            CreateIndex("dbo.Orders", "UserId");
            CreateIndex("dbo.Orders", "RestaurantPlaceId");
            //AddForeignKey("dbo.Menus", "DishId", "dbo.Dishes", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.Menus", "RestaurantId", "dbo.Restaurants", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.RestaurantsPlaces", "RestaurantId", "dbo.Restaurants", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.Orders", "RestaurantPlaceId", "dbo.RestaurantsPlaces", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.RestaurantsPlaces", "PlaceId", "dbo.Places", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.Orders", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.RestaurantsPlaces", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.Orders", "RestaurantPlaceId", "dbo.RestaurantsPlaces");
            DropForeignKey("dbo.RestaurantsPlaces", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Menus", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Menus", "DishId", "dbo.Dishes");
            DropIndex("dbo.Orders", new[] { "RestaurantPlaceId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.RestaurantsPlaces", new[] { "PlaceId" });
            DropIndex("dbo.RestaurantsPlaces", new[] { "RestaurantId" });
            DropIndex("dbo.Menus", new[] { "DishId" });
            DropIndex("dbo.Menus", new[] { "RestaurantId" });
            AlterColumn("dbo.Orders", "UserId", c => c.Int());
            AlterColumn("dbo.Orders", "RestaurantPlaceId", c => c.Int());
            AlterColumn("dbo.RestaurantsPlaces", "RestaurantId", c => c.Int());
            AlterColumn("dbo.RestaurantsPlaces", "PlaceId", c => c.Int());
            AlterColumn("dbo.Menus", "RestaurantId", c => c.Int());
            AlterColumn("dbo.Menus", "DishId", c => c.Int());
            RenameColumn(table: "dbo.Orders", name: "UserId", newName: "Users_Id");
            RenameColumn(table: "dbo.RestaurantsPlaces", name: "PlaceId", newName: "Places_Id");
            RenameColumn(table: "dbo.Orders", name: "RestaurantPlaceId", newName: "RestaurantsPlaces_Id");
            RenameColumn(table: "dbo.RestaurantsPlaces", name: "RestaurantId", newName: "Restaurants_Id");
            RenameColumn(table: "dbo.Menus", name: "RestaurantId", newName: "Restaurants_Id");
            RenameColumn(table: "dbo.Menus", name: "DishId", newName: "Dishes_Id");
            AddColumn("dbo.Orders", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "RestaurantPlaceId", c => c.Int(nullable: false));
            AddColumn("dbo.RestaurantsPlaces", "PlaceId", c => c.Int(nullable: false));
            AddColumn("dbo.RestaurantsPlaces", "RestaurantId", c => c.Int(nullable: false));
            AddColumn("dbo.Menus", "RestaurantId", c => c.Int(nullable: false));
            AddColumn("dbo.Menus", "DishId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "Users_Id");
            CreateIndex("dbo.Orders", "RestaurantsPlaces_Id");
            CreateIndex("dbo.RestaurantsPlaces", "Restaurants_Id");
            CreateIndex("dbo.RestaurantsPlaces", "Places_Id");
            CreateIndex("dbo.Menus", "Restaurants_Id");
            CreateIndex("dbo.Menus", "Dishes_Id");
            AddForeignKey("dbo.Orders", "Users_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.RestaurantsPlaces", "Places_Id", "dbo.Places", "Id");
            AddForeignKey("dbo.Orders", "RestaurantsPlaces_Id", "dbo.RestaurantsPlaces", "Id");
            AddForeignKey("dbo.RestaurantsPlaces", "Restaurants_Id", "dbo.Restaurants", "Id");
            AddForeignKey("dbo.Menus", "Restaurants_Id", "dbo.Restaurants", "Id");
            AddForeignKey("dbo.Menus", "Dishes_Id", "dbo.Dishes", "Id");
        }
    }
}
