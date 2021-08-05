using Estudos.WebApi.CatalogoJogos.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.WebApi.CatalogoJogos.Data.Mappings
{
    public class JogoMapping:IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Nome)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(a => a.Produtora)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(a => a.Preco)
                .IsRequired();
            

            builder.ToTable("Jogo");
        }
    }
}