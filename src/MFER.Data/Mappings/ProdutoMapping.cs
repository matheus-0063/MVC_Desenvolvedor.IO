using MFER.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MFER.Data.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id); //Definir qual a Primary Key da Propriedade

            builder.Property(p => p.Nome) //Define qual propriedade iremos configurar
                .IsRequired() //Define que está propriedade é obrigatória 
                .HasColumnType("varchar(200)"); //Define quantos caracteres este campo pode ter

            builder.Property(p => p.Descricao) 
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(p => p.Imagem)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.ToTable("Produtos"); // Defini o nome da Tabela, sempre no plural

        }
    }
}
