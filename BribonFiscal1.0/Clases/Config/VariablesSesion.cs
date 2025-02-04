public static class VariablesSesion
{

    private static string estado_cta_bancaria_det_id = "null";
    private static string cfdi_movto_bancario_id = "null"; //datagrid2 seleccionadao en el frm manual
    private static string uuid_vta_manual = "null";

    public static string Edo_Cta_Bancaria_Det_Id
    {
        get { return estado_cta_bancaria_det_id; }
        set { estado_cta_bancaria_det_id = value; }
    }
    public static string Cfdi_Movto_Mancario_Id
    {
        get { return cfdi_movto_bancario_id; }
        set { cfdi_movto_bancario_id = value; }
    }

    public static string Uuid_Vta_Manual
    {
        get { return uuid_vta_manual; }
        set { uuid_vta_manual = value; }
    }
}
