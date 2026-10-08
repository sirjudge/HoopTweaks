using Vintagestory.API.Common;

namespace HoopTweaks.Infrastructure;

public abstract class IHoopCommand : ModSystem
{
    public override abstract void Dispose();
}

public abstract class IHoopServerCommand : IHoopCommand
{
    public abstract void RegisterServerCommands();
}

public abstract class IHoopClientCommand : IHoopCommand
{
    public abstract void RegisterClientCommands();
}
