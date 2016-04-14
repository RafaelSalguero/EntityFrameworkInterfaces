using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkInterfaces
{
    /// <summary>
    /// Un objeto capaz de proveer de ITables
    /// </summary>
    public interface IDatabase : IDisposable
    {
        ITable<T> Set<T>() where T : class;
        /// <summary>
        /// Guarda los cambios en la base de datos
        /// </summary>
        void SaveChanges();
    }

    /// <summary>
    /// Implementacion de IDatabase a partir de un DbContext
    /// </summary>
    public class ContextDatabase : IDatabase
    {
        public ContextDatabase(DbContext Db)
        {
            this.Db = Db;
        }

        private readonly DbContext Db;

        public ITable<T> Set<T>() where T : class
        {
            return new DbSetTable<T>(Db.Set<T>());
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public void SaveChanges()
        {
            Db.SaveChanges();
        }
    }

    /// <summary>
    /// Implementación de IDatabase a partir de un objeto que tiene como propiedades objetos de tipo List&lt;T&gt;
    /// </summary>
    public class InMemoryDatabase : IDatabase
    {
        /// <summary>
        /// Crea un IDatabase a partir de un objeto con propiedades de tipo List
        /// </summary>
        /// <param name="ObjectWithLists">Para cada propiedad del objeto que sea de tipo tipo List&lt;T&gt, se crea un Set de la base de datos el cual tendra los datos que tienen la lista, y las modificaciones a las tablas afectaran a su vez a las listas originales</param>
        public InMemoryDatabase(object ObjectWithLists)
        {
            this.lists = ObjectWithLists.GetType().GetProperties()
                .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                .ToDictionary(x => x.PropertyType.GetGenericArguments()[0], x => x.GetValue(ObjectWithLists));
        }

        private Dictionary<Type, object> lists;

        public ITable<T> Set<T>() where T : class
        {
            return new ListTable<T>((List<T>)lists[typeof(T)]);
        }

        public void Dispose()
        {
        }

        public void SaveChanges()
        {
            //Falta implementar la logica del SaveChanges en el mock
            //throw new NotImplementedException();
        }
    }
}
