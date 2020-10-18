using System;
using System.Data;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    [UsedImplicitly]
    public sealed class RepositoryContext : IRepositoryContext
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        private readonly bool _commitTransaction;
        private readonly bool _closeConnection;
        private bool _disposed;


        public RepositoryContext(IDbConnection Connection, IDbTransaction Transaction, bool CommitTransaction, bool CloseConnection)
        {
            this.Connection = Connection;
            this.Transaction = Transaction;
            _commitTransaction = CommitTransaction;
            _closeConnection = CloseConnection;
        }


        ~RepositoryContext() => Dispose(false);


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool Disposing)
        {
            if (_disposed) return;
            if (Disposing)
            {
                // No managed objects to release.
            }
            // Release unmanaged objects.
            if (_commitTransaction && (Transaction != null))
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
            if (_closeConnection && (Connection != null))
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
            _disposed = true;
        }
    }
}
