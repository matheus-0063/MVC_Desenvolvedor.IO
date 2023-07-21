using MFER.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFER.Business.Interfaces
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        Task<Fornecedor> ObterFornecedorEndereco(Guid id);
        //Essa Task vai obter o Fornecedor e o Endereço no mesmo objeto
        //Task<Fornecedor> logo ela irá retornar um Fornecedor
        Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id);
        //Essa Task vai obter o Fornecedor, o seu Endereço e sua lista de Produtos
        //Task<Fornecedor> logo ela irá retornar um Fornecedor
    }
}
