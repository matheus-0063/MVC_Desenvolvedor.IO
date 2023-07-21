using MFER.Business.Models;

namespace MFER.Business.Interfaces
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
        // Obter endereço por fornecedor, importante quando for atualizar ou alterar o endereço de um fornecedor
        //Task<Endereco>, irá retornar um Endereco
    }
}
