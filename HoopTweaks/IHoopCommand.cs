using Vintagestory.API.Client;
using Vintagestory.API.Server;

namespace HoopTweaks;
public abstract class IHoopCommand;
public abstract class IHoopServerCommand : IHoopCommand
{
    public abstract void RegisterServerCommands(ICoreServerAPI api);
}

public abstract class IHoopClientCommand : IHoopCommand
{
    public abstract void RegisterClientCommands(ICoreClientAPI api);
}
