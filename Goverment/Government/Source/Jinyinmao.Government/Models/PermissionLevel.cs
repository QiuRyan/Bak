namespace Jinyinmao.Government.Models
{
    public enum PermissionLevel
    {
        Deny = 0,
        Consumer = 10,
        Developer = 20,
        Administrator = 30
    }

    internal static class PermissionLevelEx
    {
        internal static int GetPermissionLevelValue(this PermissionLevel level)
        {
            return (int)level;
        }
    }
}