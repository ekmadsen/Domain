using System;
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
        [UsedImplicitly] protected Func<Task<IRepositoryContext>> GetContextAsync { get; set; } // For use by Unit of Work classes.
        [UsedImplicitly] private readonly ILoggedDatabase _database;


        protected RepositoryBase(ILogger Logger, ICorrelationIdAccessor CorrelationIdAccessor, ILoggedDatabase Database)
        {
            this.Logger = Logger;
            CorrelationId = CorrelationIdAccessor.GetCorrelationId();
            _database = Database;
            GetContextAsync = async () =>
            {
                // Return a new, unshared database connection.
                // Unit of Work classes should modify this Func to return a shared (by multiple repositories) database connection.
                var connection = await _database.OpenConnectionAsync(CorrelationId);
                var transaction = connection.BeginTransaction();
                return new RepositoryContext(connection, transaction, true, true);
            };
        }
    }
}
