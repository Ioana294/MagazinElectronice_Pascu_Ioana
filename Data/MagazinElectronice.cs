using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MagazinElectronice_Pascu_Ioana.Models;

namespace MagazinElectronice_Pascu_Ioana.Data
{
    public class MagazinElectronice : DbContext
    {
        public MagazinElectronice(DbContextOptions<MagazinElectronice> options) : base(options) { }
        public DbSet<Client> Clienti { get; set; }
        public DbSet<Comanda> Comenzi { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Oferta> Oferte { get; set; }
        public DbSet<Membru> Membri { get; set; }
        public DbSet<Marca> Marci { get; set; }
        public DbSet<MarcaDevice> MarcaDevices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Comanda>().ToTable("Comanda");
            modelBuilder.Entity<Device>().ToTable("Device");
            modelBuilder.Entity<Oferta>().ToTable("Oferta");
            modelBuilder.Entity<Membru>().ToTable("Membru");
            modelBuilder.Entity<Marca>().ToTable("Marca");
            modelBuilder.Entity<MarcaDevice>().ToTable("MarcaDevice");

            modelBuilder.Entity<MarcaDevice>().HasKey(c => new { c.DeviceID, c.MarcaID });
        }
    }
}
