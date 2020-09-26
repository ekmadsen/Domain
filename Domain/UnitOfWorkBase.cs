using System;
using System.Data;
using System.Threading.Tasks;
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
        private ILoggedDatabase _database;
        private IDbConnection _dbConnection;
        private IDbTransaction _transaction;
        private bool _committed;
        private bool _disposed;


        protected UnitOfWorkBase(ILogger Logger, ICorrelationIdAccessor CorrelationIdAccessor, ILoggedDatabase Database)
        {
            this.Logger = Logger;
            CorrelationId = CorrelationIdAccessor.GetCorrelationId();
            _database = Database;
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
                // Release managed objects.
                _database = null;
            }
            // Release unmanaged objects.
            _transaction?.Dispose();
            _transaction = null;
            _dbConnection?.Dispose();
            _dbConnection = null;
            // Do not release logger.  Its lifetime is controlled by caller.
            _disposed = true;
        }


        public async Task BeginAsync()
        {
            _dbConnection = await _database.OpenConnectionAsync(CorrelationId);
            _transaction = _dbConnection.BeginTransaction();
        }


        public void Commit()
        {
            _transaction.Commit();
            _committed = true;
        }


        public void Rollback() => _transaction.Rollback();
    }
}
