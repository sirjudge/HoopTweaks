namespace HoopTweaks.Infrastructure;
public class AuditEvent(bool success, string auditCode)
{
    public bool Success = success;
    public string AuditCode = auditCode;
}

public class QueryEvent(bool success, string auditCode, string query) : AuditEvent(success, auditCode)
{
    public string Query = query;

}



