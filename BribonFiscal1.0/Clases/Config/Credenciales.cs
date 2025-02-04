public static class Credenciales
{
    private static string userSIS = "x_nano_32@hotmail.com";
    private static string nameSIS = "CGRC";


    //-------MySQLFRD--------//
    private static string userMs = "admin";
    private static string servMs = "qc-megafresh.cgjn6lnuo5l2.us-east-1.rds.amazonaws.com";
    private static string emprMs = "etaxes_xml";
    private static string passMs = "Ferp1102";

    public static string UserSIS
    {
        get
        {
            return userSIS;
        }
        set
        {
            userSIS = value;
        }
    }
    public static string NameSIS
    {
        get
        {
            return nameSIS;
        }
        set
        {
            nameSIS = value;
        }
    }

    public static string UserMs
    {
        get
        {
            return userMs;
        }
        set
        {
            userMs = value;
        }
    }
    public static string ServMs
    {
        get
        {
            return servMs;
        }
        set
        {
            servMs = value;
        }
    }
    public static string EmprMs
    {
        get
        {
            return emprMs;
        }
        set
        {
            emprMs = value;
        }
    }
    public static string PassMs
    {
        get
        {
            return passMs;
        }
        set
        {
            passMs = value;
        }
    }
}