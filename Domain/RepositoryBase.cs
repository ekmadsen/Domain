using System;
using ErikTheCoder.Logging;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    public abstract class RepositoryBase
    {
        [UsedImplicitly] protected readonly ILogger Logger;
        [UsedImplicitly] protected readonly Guid CorrelationId;


        protected RepositoryBase(ILogger Logger, Guid CorrelationId)
        {
            this.Logger = Logger;
            this.CorrelationId = CorrelationId;
        }
    }
}
