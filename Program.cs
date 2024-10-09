using Cardano.Sync;
using Cardano.Sync.Extensions;
using Microsoft.EntityFrameworkCore;
using Cardano.Sync.Reducers;
using Argus_BalanceByAddressAPI.Data;
using Cardano.Sync.Data.Models;
using Argus_BalanceByAddressAPI.Reducer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCardanoIndexer<BalanceAddressDBContext>(builder.Configuration);

//Errors if IReducer doesnt have <argument>
builder.Services.AddSingleton<IReducer<IReducerModel>, BalanceAddressReducer>(); 

var app = builder.Build();

//this is from Tan's example, in the Argus.Example there's none for this portion
using IServiceScope scope = app.Services.CreateScope();
BalanceAddressDBContext dbContext = scope.ServiceProvider.GetRequiredService<BalanceAddressDBContext>();
dbContext.Database.Migrate();

app.MapGet("/", () => "Hello World!");

app.Run();
