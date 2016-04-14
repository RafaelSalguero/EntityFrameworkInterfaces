using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkInterfaces
{
    /// <summary>
    /// Una interfaz que envuelve a un DbSet para su posible uso en pruebas unitarias.
    /// Los metodos que deseen manipular una tabla deberan depender de esta interfaz en lugar de un DbSet.
    /// Los metodos que solo filtren resultados deberan depender de IQueryable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITable<T>
    {
        /// <summary>
        /// Borra una entidad de la tabla
        /// </summary>
        /// <param name="Entity"></param>
        void Remove(T Entity);

        /// <summary>
        /// Agrega un elemento a la tabla
        /// </summary>
        /// <param name="Entity"></param>
        void Add(T Entity);

        /// <summary>
        /// Obtiene todos los elementos de la tabla
        /// </summary>
        IQueryable<T> All { get; }
    }
    /// <summary>
    /// Implementación de un ITable a partir de un List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListTable<T> : ITable<T>
    {
        public ListTable(IList<T> List)
        {
            this.list = List;
        }
        readonly IList<T> list;

        IQueryable<T> ITable<T>.All
        {
            get
            {
                return list.AsQueryable();
            }
        }

        void ITable<T>.Remove(T Entity)
        {
            list.Remove(Entity);
        }

        void ITable<T>.Add(T Entity)
        {
            list.Add(Entity);
        }
    }

    /// <summary>
    /// Implementación de un ITable a partir de un DbSet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbSetTable<T> : ITable<T>
        where T : class
    {
        public DbSetTable(DbSet<T> Set)
        {
            this.Set = Set;
        }
        private readonly DbSet<T> Set;

        IQueryable<T> ITable<T>.All
        {
            get
            {
                return Set;
            }
        }

        void ITable<T>.Add(T Entity)
        {
            Set.Add(Entity);
        }

        void ITable<T>.Remove(T Entity)
        {
            Set.Remove(Entity);
        }
    }
}
