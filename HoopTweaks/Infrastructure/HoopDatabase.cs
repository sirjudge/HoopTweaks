#nullable disable
using System;
using Vintagestory.API.Common;
using Microsoft.Data.Sqlite;

namespace HoopTweaks.Infrastructure;
public class HoopDatabase : IDisposable
{
    public bool _disposing;
    internal ILogger Logger { set; get; }
    public SqliteConnection Connection { set; get; }

    public HoopDatabase(ILogger logger) {
        Logger = logger;
        Connection = new SqliteConnection("Data Source=/home/nico/.cache/hoops.db;Mode=ReadWriteCreate;Cache=Shared");
        try{
            Connection.Open();
        }
        catch (Exception e){
            Logger.Error($"Could not open hoops.db file with connectionString:{Connection.ConnectionString} with data source:{Connection.DataSource}");
            Logger.LogException(EnumLogType.Error, e);
            throw;
        }
    }

    public void Dispose()
    {
        if (_disposing) return;

        _disposing = true;
        Connection?.Dispose();
        Connection = null;

        if (Logger is not null){
            Logger = null;
        }

        _disposing = false;
    }
}


