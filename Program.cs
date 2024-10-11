using Cardano.Sync;
using Cardano.Sync.Extensions;
using Microsoft.EntityFrameworkCore;
using Cardano.Sync.Reducers;
using Argus_BalanceByAddressAPI.Data;
using Cardano.Sync.Data.Models;
using Argus_BalanceByAddressAPI.Reducer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCardanoIndexer<BalanceAddressDbContext>(builder.Configuration);
//Does this add in .AddDbContext as well? - check -> Yes, it has the UseNpgsql portion (read more into it later)
//Errors if IReducer doesnt have <argument>
builder.Services.AddSingleton<IReducer<IReducerModel>, BalanceAddressReducer>(); 

var app = builder.Build();

//this is from Tan's example, in the Argus.Example there's none for this portion
using IServiceScope scope = app.Services.CreateScope();
BalanceAddressDbContext dbContext = scope.ServiceProvider.GetRequiredService<BalanceAddressDbContext>();
dbContext.Database.Migrate(); //automigrates

app.MapGet("/", () => "Hello World!");

app.Run();



