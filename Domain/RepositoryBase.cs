using System;
using System.Data.Common;
using System.Threading.Tasks;
using ErikTheCoder.Data;
using ErikTheCoder.Logging;
using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    public abstract class RepositoryBase : IRepository
    {
        [UsedImplicitly] protected readonly ILogger Logger;
        [UsedImplicitly] protected readonly Guid CorrelationId;
        [UsedImplicitly] private readonly ILoggedDatabase _database;
        [UsedImplicitly] public Func<Task<(DbConnection Connection, bool IsSharedConnection)>> GetDbConnectionAsync; // For use by Unit of Work classes.

        
        protected RepositoryBase(ILogger Logger, ICorrelationIdAccessor CorrelationIdAccessor, ILoggedDatabase Database)
        {
            this.Logger = Logger;
            CorrelationId = CorrelationIdAccessor.GetCorrelationId();
            _database = Database;
            GetDbConnectionAsync = async () =>
            {
                // Return a new, unshared database connection.
                // Unit of Work classes should modify this Func to return a shared (by multiple repositories) database connection.
                var connection = await _database.OpenConnectionAsync(CorrelationId);
                return (connection, false);
            };
        }


        public async Task<(DbConnection Connection, DbTransaction Transaction, bool DisposeDbResources)> GetDbResourcesAsync()
        {
            var (connection, isSharedConnection) = await GetDbConnectionAsync();
            DbTransaction transaction;
            bool disposeDbResources;
            if (isSharedConnection)
            {
                transaction = null;
                disposeDbResources = false;
            }
            else
            {
                transaction = connection.BeginTransaction();
                disposeDbResources = true;
            }
            return (connection, transaction, disposeDbResources);
        }
    }
}
