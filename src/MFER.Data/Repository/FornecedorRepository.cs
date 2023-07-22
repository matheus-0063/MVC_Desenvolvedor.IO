using MFER.Business.Interfaces;
using MFER.Business.Models;
using MFER.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFER.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(MeuDBContext context) : base(context) { }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await _entitySet.AsNoTracking().Include(e => e.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id);
            //Estou indo na tabela Fornecedor fazendo un JOIN com a tabela Endereco(Include)
            //e buscando o primeiro Fornecedor (FirstOrDefaultAsync) que tenha o id passado no parametro
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await _entitySet.AsNoTracking()
                .Include(e => e.Endereco)
                .Include(p => p.Produtos)
                .FirstOrDefaultAsync(f => f.Id == id);
            //Estou indo na tabela Fornecedor fazendo un JOIN com a tabela Endereco(Include)
            // mais um JOIN com a tabela Produtos (Include)
            //e buscando o primeiro Fornecedor (FirstOrDefaultAsync) que tenha o id passado no parametro
        }
    }
}
