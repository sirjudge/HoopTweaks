using System;
using System.Text;

namespace HoopTweaks.Infrastructure;

public interface IAuditEvent {
    bool Success { get; set; }
    string AuditCode { get; set; }
}

public class  QueryEvent : IAuditEvent
{
    public string? Query;
    public bool Success { get; set; }
    public string AuditCode { get; set; }
    public Exception? Exception;
    public QueryEvent(bool success, string auditCode, string query)
    {
        Success = success;
        AuditCode = auditCode;
        Query = query;
    }

    public QueryEvent(bool success, string auditCode, Exception exception)
    {
        Success = success;
        AuditCode = auditCode;
        Exception = exception;
    }
    public QueryEvent(bool success, string auditCode, Exception exception, string query)
    {
        Success = success;
        AuditCode = auditCode;
        Exception = exception;
        Query = query;
    }

    public override string ToString()
    {
        string successString = Success? "yes" : "no";
        StringBuilder returnStringBuilder = new StringBuilder($"Successful {successString}AuditCode:{AuditCode} ");
        if (Query is not null){
            returnStringBuilder.Append($"Query: {Query}");
        }

        if (Exception is not null){
            returnStringBuilder.Append($"Exception: {Exception}");
        }
        return returnStringBuilder.ToString();
    }
}
