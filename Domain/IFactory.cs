using JetBrains.Annotations;

namespace ErikTheCoder.Domain
{
    public interface IFactory<out T>
    {
        [UsedImplicitly] T Create();
    }
}
