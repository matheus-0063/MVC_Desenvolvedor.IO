using MFER.Business.Interfaces;
using MFER.Business.Models;
using MFER.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MFER.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
        //Além de implementar a Repository<Produto> que iá trazer os métodos criados na repository genérica
        //também precisamos implementar o IProdutoRepository para implementar os métodos especificos
    {
        public ProdutoRepository(MeuDBContext context) : base(context) { }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            return await _entitySet.AsNoTracking().Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);
            //Estou indo na tabela Produtos fazendo un JOIN com a tabela Forncedor(Include)
            //e buscando o primeiro Produto (FirstOrDefaultAsync) que tenha o id passado no parametro
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await _entitySet.AsNoTracking().Include(f => f.Fornecedor)
                .OrderBy(p => p.Nome).ToListAsync();
            //Estou indo na tabela Produtos fazendo un JOIN com a tabela Forncedor(Include)
            //ordenado pelo nome do Produto (.OrderBy(p => p.Nome)) e listando todos os produtos
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(p => p.FornecedorId == fornecedorId);
            //Estou buscando todos os produtos que tenham o mesmo Id do Forncedor passado por parametro
        }
    }
}
