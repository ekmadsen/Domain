using System;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    [UsedImplicitly]
    public interface IUnitOfWork : IDisposable
    {
        [UsedImplicitly] void Commit();
        [UsedImplicitly] void Rollback();
    }
}
