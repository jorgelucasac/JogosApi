using System.Linq;
using Estudos.WebApi.CatalogoJogos.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Estudos.WebApi.CatalogoJogos.Data
{
    //Add-Migration Jogos -OutputDir "Data/Migrations"
    public class JogosContext : DbContext
    {
        public JogosContext(DbContextOptions<JogosContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(JogosContext).Assembly);
            MapearEntidadesEsquecidas(modelBuilder);

            modelBuilder.Entity<Jogo>()
                .HasQueryFilter(a => a.Ativo);
        }


        public DbSet<Jogo> Jogos { get; set; }

        private void MapearEntidadesEsquecidas(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var propriedadesString = entity
                    .GetProperties().Where(p => p.ClrType == typeof(string));

                var propriedadesDecimais = entity
                    .GetProperties().Where(p => p.ClrType == typeof(decimal)
                                                || p.ClrType == typeof(decimal?));

                foreach (var propriedade in propriedadesString)
                    if (string.IsNullOrEmpty(propriedade.GetColumnType())
                        && !propriedade.GetMaxLength().HasValue)
                        //propriedade.SetMaxLength(100);
                        propriedade.SetColumnType("VARCHAR(100)");


                //foreach (var propriedade in propriedadesDecimais)
                //{
                //    if (!propriedade.GetPrecision().HasValue)
                //        propriedade.SetPrecision(18);

                //    if (!propriedade.GetScale().HasValue)
                //        propriedade.SetScale(2);
                //}
            }
        }
    }
}