using Argus_BalanceByAddressAPI.Data.Models;
using Cardano.Sync.Data;
using Microsoft.EntityFrameworkCore;

namespace Argus_BalanceByAddressAPI.Data;

public class BalanceAddressDbContext  
(
    DbContextOptions<BalanceAddressDbContext> options, //migrations ownt work without options constructer (i think)
    IConfiguration configuration    
) : CardanoDbContext(options, configuration) //check why this is here and where this comes from, especially CardanoDbContext
{
    //based on JPGStore.Data.Models.JPGStoreSyncDbContext.cs
    //make the table
    public DbSet<BalanceAddress> BalanceAddress { get; set; }

    //based on both JPGStore.Data.Models.JPGStoreSyncDbContext.cs and Argus.Sync.Example.Data.CardanoTestDbContext.cs
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    
    {
        base.OnModelCreating(modelBuilder);

        //where the entity type BalanceAddress is a class representing a table in the DB
        //from looking at JPGStore code, we follow  by adding entity.HasKey -> primary key (Address)
        //JPGStore also uses .OwnsOne and .HasIndex - note why later
        modelBuilder.Entity<BalanceAddress>(entity => {
            entity.HasKey(e => e.Address);
            entity.OwnsOne(e => e.Balance); //becasue there was a sudden error saying LoveLace doesnt have a PK
        });

        //In JPGStore code - learn why its necessary
        //base.OnModelCreating(modelBuilder); - its not in Levvy?
    }

}

//Note that I add the connectionStrings portion manually