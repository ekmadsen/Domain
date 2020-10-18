using System;
using System.Data;


namespace ErikTheCoder.Domain
{
    public interface IRepositoryContext : IDisposable
    {
        // ReSharper disable UnusedMemberInSuper.Global
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
        // ReSharper restore UnusedMemberInSuper.Global
    }
}
