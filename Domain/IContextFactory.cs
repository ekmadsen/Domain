using JetBrains.Annotations;


namespace ErikTheCoder.Domain
{
    [UsedImplicitly]
    public interface IContextFactory<out T, in TContext> : IFactory<T>
    {
        [UsedImplicitly] T Create(TContext Context);
    }
}
