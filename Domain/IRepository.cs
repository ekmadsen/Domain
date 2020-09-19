using System.Data.Common;
using System.Threading.Tasks;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    public interface IRepository
    {
        [UsedImplicitly] Task<(DbConnection Connection, DbTransaction Transaction, bool DisposeDbResources)> GetDbResourcesAsync();
    }
}
