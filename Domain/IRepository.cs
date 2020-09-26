using System.Data;
using System.Threading.Tasks;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    public interface IRepository
    {
        [UsedImplicitly] Task<(IDbConnection Connection, IDbTransaction Transaction, bool DisposeDbResources)> GetDbResourcesAsync();
    }
}
