using MFER.Business.Models;
using System.Linq.Expressions;

namespace MFER.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    // Interface gerenerica que recebera um objeto(<TEntity>) obriga que o
    // repositorio libere a memória( : IDisposable) onde (where) o objeto (TEntity)
    // só pode ser usada se for uma classe filha de (Entity)
    {
        Task Adicionar(TEntity obj);
        //Existe um método Adicionar onde qualquer entidade que vai ser rebecida pelo parametro 
        //desde que ela seja filha de Entity, vai ser aceita
        //Quando tem apenas Task, retorna um void
        Task<TEntity> ObterPorId(Guid id);
        //Após passar o id por parametro, irá retornar uma Task do tipo <TEntity>
        //Aqui é Task<TEntity> irá retornar um Entidade
        Task<List<TEntity>> ObterTodos();
        //Como é para listar todos os objetos, não é necessário parametro
        //Aqui é Task<List<TEntity>> irá retornar uma lista de Entidades
        Task Atualizar(TEntity obj);
        //Atualizar recebera o objeto como parametro para fazer o Update
        //Quando tem apenas Task, retorna um voi
        Task Remover(Guid id);
        //Para remover, precisamos do id do objeto
        //Quando tem apenas Task, retorna um void
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
        //Expression = pode receber uma expressão lambda, que vai comparar com o objeto(TEntity) e retornar um bool
        //Isso possibilita que a gente passe uma expressão lambda dentro deste método para buscar qualquer
        //entidade por qualquer parametro
        //Ele sempre retornará uma coleçao de TEntity
        Task<int> SaveChanges();
        //
        //Aqui é Task<int> e irá retornar inteiros ou seja, a quantidade de linhas afetadas

    }
}
