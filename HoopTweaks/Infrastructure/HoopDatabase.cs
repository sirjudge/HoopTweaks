#nullable disable
using System;
using Vintagestory.API.Common;
using Microsoft.Data.Sqlite;

namespace HoopTweaks.Infrastructure;
public class HoopDatabase(ILogger logger) : IDisposable
{
    public bool _disposing;
    internal ILogger Logger { set; get; }
    public SqliteConnection Connection = new SqliteConnection("Data Source=hoops.db");

    internal int GetOrdinalFromColumnName(string columnname, SqliteDataReader reader) 
        => reader.GetOrdinal(columnname);

    public void Dispose()
    {
        if (_disposing) return;

        Connection = null;
        Logger = null;
    }
}


