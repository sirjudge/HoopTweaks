using Vintagestory.API.Common;

namespace HoopTweaks.Infrastructure;

public abstract class IHoopSystem : ModSystem
{
    public override abstract void Dispose();
}

public abstract class IHoopServerSystem : IHoopSystem
{
    public abstract void RegisterServerCommands();
}

public abstract class IHoopClientSystem : IHoopSystem
{
    public abstract void RegisterClientCommands();
}
