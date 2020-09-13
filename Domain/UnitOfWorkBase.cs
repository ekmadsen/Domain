using System;
using System.Data;
using ErikTheCoder.Logging;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    [UsedImplicitly]
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        [UsedImplicitly] protected readonly ILogger Logger;
        [UsedImplicitly] protected readonly Guid CorrelationId;
        private IDbTransaction _transaction;
        private bool _committed;
        private bool _disposed;


        protected UnitOfWorkBase(ILogger Logger, Guid CorrelationId, IDbConnection Connection)
        {
            this.Logger = Logger;
            this.CorrelationId = CorrelationId;
            _transaction = Connection.BeginTransaction();
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
                try
                {
                    _transaction.Rollback();
                }
                catch
                {
                    // Ignore exception.
                }
            }
            if (Disposing)
            {
                // Free managed objects.
            }
            // Free unmanaged objects.
            _transaction?.Dispose();
            _transaction = null;
            _disposed = true;
        }


        public void Commit()
        {
            _transaction.Commit();
            _committed = true;
        }


        void IUnitOfWork.Rollback() => _transaction.Rollback();
    }
}
