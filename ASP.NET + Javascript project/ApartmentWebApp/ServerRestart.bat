dotnet-ef database drop --project App.DAL.EF --startup-project WebApp
dotnet-ef migrations remove --project App.DAL.EF --startup-project WebApp --context AppDbContext
dotnet-ef migrations add --project App.DAL.EF --startup-project WebApp --context AppDbContext Initial
dotnet-ef database update --project App.DAL.EF --startup-project WebApp