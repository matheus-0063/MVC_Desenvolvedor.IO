using MFER.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFER.Business.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);
        //Irá obter uma lista de todos os produtos daquele fornecedor
        //Task<IEnumerable<Produto>>, irá retornar uma lista de produtos
        Task<IEnumerable<Produto>> ObterProdutosFornecedores();
        //Irá retornar uma lista de um unico produto, e quais fornecedores tem aquele produto
        //Task<IEnumerable<Produto>>, irá retornar uma lista de produtos
        Task<Produto> ObterProdutoFornecedor(Guid id);
        //Task<Produto> irá retornar um produto de um fornecedor
    }
}
