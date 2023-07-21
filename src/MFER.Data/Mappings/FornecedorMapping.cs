using MFER.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MFER.Data.Mappings
{
    public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasKey(x => x.Id); //Definir qual a Primary Key da Propriedade

            builder.Property(x => x.Nome) //Define qual propriedade iremos configurar
                .IsRequired() //Define que está propriedade é obrigatória
                .HasColumnType("varchar(100)"); //Define quantos caracteres este campo pode ter

            builder.Property(x => x.Documento)
                .IsRequired()
                .HasColumnType("varchar(14)");

            // 1 : 1 => Fornecedor : Endereco
            builder.HasOne(f => f.Endereco)
                .WithOne(e => e.Fornecedor);
            /*O pedaço de codigo acima, é o mesmo que dizer:
                Fornecedor(builder) tem um(HasOne) Endereco(f => f.Endereco)
                    e o Endereco(WithOne) tem um Fornecedor(e => e.Fornecedor*/

            // 1 : N => Fornecedor : Produtos
            builder.HasMany(f => f.Produtos)
                .WithOne(p => p.Fornecedor)
                .HasForeignKey(p => p.FornecedorId);
            /*O pedaço de codigo acima, é o mesmo que dizer:
                Fornecedor(builder) tem um ou varios(HasMany) Produtos(f => f.Produtos)
                    e o Produto(WithOne) tem um Fornecedor(p => p.Fornecedor
                    e a chave estrangeira de conexao(HasForeignKey) é o FornecedorId(p => p.FornecedorId)*/

            builder.ToTable("Fornecedores"); //Define o nome da Tabela
        }
    }
}
