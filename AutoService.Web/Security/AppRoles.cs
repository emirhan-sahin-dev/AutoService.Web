namespace AutoService.Web.Security;

public static class AppRoles
{
    public const string Admin = "Admin";

    public const string ServiceAdvisor = "Service Advisor";

    public const string Mechanic = "Mechanic";

    public const string AdminOrServiceAdvisor =
        Admin + "," + ServiceAdvisor;

    public const string AllStaff =
        Admin + "," + ServiceAdvisor + "," + Mechanic;
}