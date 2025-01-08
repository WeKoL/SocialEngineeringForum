using System.Collections.Generic;

namespace MyForum.Data
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
    }
}