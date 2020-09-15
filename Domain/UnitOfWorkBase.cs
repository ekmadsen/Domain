using System;
using System.Data;
using System.Data.Common;
using ErikTheCoder.Data;
using ErikTheCoder.Logging;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    [UsedImplicitly]
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        [UsedImplicitly] protected readonly ILogger Logger;
        [UsedImplicitly] protected readonly Guid CorrelationId;
        private DbConnection _dbConnection;
        private IDbTransaction _transaction;
        private bool _committed;
        private bool _disposed;


        protected UnitOfWorkBase(ILogger Logger, ICorrelationIdAccessor CorrelationIdAccessor, ILoggedDatabase Database)
        {
            this.Logger = Logger;
            CorrelationId = CorrelationIdAccessor.GetCorrelationId();
            _dbConnection = Database.OpenConnection(CorrelationId);
            _transaction = _dbConnection.BeginTransaction();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool Disposing)
        {
            if (_disposed) return;
            if (!_committed)
            {
                // Rollback uncommitted transaction.
                _transaction?.TryRollback();
            }
            if (Disposing)
            {
                // No managed objects to release.
            }
            // Release unmanaged objects.
            _transaction?.Dispose();
            _transaction = null;
            _dbConnection?.Dispose();
            _dbConnection = null;
            // Do not release logger.  Its lifetime is controlled by caller.
            _disposed = true;
        }


        public void Commit()
        {
            _committed = _transaction?.TryCommit() ?? false;
        }


        void IUnitOfWork.Rollback() => _transaction?.TryRollback();
    }
}
