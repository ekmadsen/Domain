using System.Collections.Generic;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    [UsedImplicitly]
    public interface IRepository<TDomain, TId, in TFactory, in TFilter>
    {
        // Define CRUD methods.
        [UsedImplicitly] TId Create(TDomain Instance, TFactory Factory);
        [UsedImplicitly] TDomain Read(TId Id, TFactory Factory);
        [UsedImplicitly] IEnumerable<TDomain> Read(IList<TId> Ids, TFactory Factory);
        [UsedImplicitly] IEnumerable<TDomain> Read(TFilter Filter, TFactory Factory);
        [UsedImplicitly] void Update(TDomain Instance);
        [UsedImplicitly] void Delete(TId Id);
    }
}
