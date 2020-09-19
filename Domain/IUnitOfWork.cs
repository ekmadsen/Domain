using System;
using System.Threading.Tasks;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        [UsedImplicitly] Task BeginAsync();
        [UsedImplicitly] void Commit();
        [UsedImplicitly] void Rollback();
    }
}
