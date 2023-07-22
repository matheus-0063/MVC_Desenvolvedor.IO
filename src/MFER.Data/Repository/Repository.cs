using MFER.Business.Interfaces;
using MFER.Business.Models;
using MFER.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MFER.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
        //primeiro <TEntity> parametro genérico
        //a classe Repository irá implementar a Interface IRepository onde segundo TEntity precisa ser filha de Entity
        //new = permite criar uma TEntity nova
    {
        protected readonly MeuDBContext _meuDBContext; // Necessário para ter acesso ao nosso contexto
        protected readonly DbSet<TEntity> _entitySet; // Atalho para a lógica dos métodos nao ficar tão verboso

        public Repository(MeuDBContext meuDBContext) //Injetando o Context
        {
            _meuDBContext = meuDBContext;
            _entitySet = meuDBContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entitySet.AsNoTracking()
                .Where(predicate)
                .ToListAsync();
            // retorna uma lista de objetos, no qual este objeto tenha algum parametro passado no predicate
            // .AsNoTracking() = retorna a leitura do banco, mas sem o tracking o que vai trazer mais performace 
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await _entitySet.FindAsync(id);
            //retorna o obj que tem o id passado por parametro
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await _entitySet.ToListAsync();
            //retorna todos os objetos 
        }
        public virtual async Task Adicionar(TEntity obj)
        {
            _entitySet.Add(obj); //Está adicionando o obj 
            await SaveChanges(); //Está salvando o novo obj
        }

        public virtual async Task Atualizar(TEntity obj)
        {
            _entitySet.Update(obj); //Está atualizando o obj
            await SaveChanges(); //Está salvando as alterações do obj
        }

        public virtual async Task Remover(Guid id)
        {
            _entitySet.Remove(new TEntity { Id = id }); //Remove o obj cujo o Id seja igual o id passado por parametro
            await SaveChanges(); // Está salvando as alterações do obj
        }

        public async Task<int> SaveChanges()
        {
            return await _meuDBContext.SaveChangesAsync();
            // o método SaveChanges é um metodo auxiliar para salvar os objetos no Context usando o SaveChangesAsync()
        }

        public void Dispose()
        {
            _meuDBContext?.Dispose();
            // Limpar/reciclar recursos não gerenciados aqui (por exemplo, fechar arquivos ou desconectar do banco de dados).
        }
    }
}
