using JetBrains.Annotations;

namespace Enemies
{
    public class AttackToken
    {
        public int Id;
        public int Priority;
        [CanBeNull] public ITokenUser Holder;
    }
}