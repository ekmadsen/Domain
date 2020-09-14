using System;
using System.Data;
using ErikTheCoder.Logging;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    public abstract class RepositoryBase : IRepository
    {
        [UsedImplicitly] protected readonly ILogger Logger;
        [UsedImplicitly] protected readonly Guid CorrelationId;
        [UsedImplicitly] public IDbConnection DbConnection; // For use by Unit of Work classes.
        private bool _disposed;


        protected RepositoryBase(ILogger Logger, ICorrelationIdAccessor CorrelationIdAccessor)
        {
            this.Logger = Logger;
            CorrelationId = CorrelationIdAccessor.GetCorrelationId();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool Disposing)
        {
            if (_disposed) return;
            if (Disposing)
            {
                // No managed objects to release.
            }
            // Release unmanaged objects.
            DbConnection?.Dispose();
            DbConnection = null;
            // Do not release logger.  Its lifetime is controlled by caller.
            _disposed = true;
        }
    }
}
