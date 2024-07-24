namespace usingDI.Tenants
{
    public interface ITenantService
    {
        public string GetTenantId();
        //SoftwareAsAServiv SaaS
    }
    public class SQLTenant : ITenantService
    {
        public string GetTenantId()
        {
            return "SQL Connection";
        }
    }

    public class OracleTenant : ITenantService
    {
        public string GetTenantId()
        {
            return "Oracle Connection";
        }
    }
}
