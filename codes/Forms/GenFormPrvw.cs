using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using ASPNETCoreWeb.codes.XysPages;
using System.Data;

namespace ASPNETCoreWeb.codes.Forms
{

    public class GenFormPrvw : WebPage
    {
        private XForm xForm = null;
        private string FormModel = string.Empty;
        private string FormDesc = string.Empty;
        private string FormData = string.Empty;
        private string FormWritten = string.Empty;
        private string FormAttaches = string.Empty;
        private string FileRefId = string.Empty;
        private List<NameValue> FormValues = null;

        public GenFormPrvw()
        {
            string formId = DecryptString(ParamValue("x"));
            string ssql = " select FormDesc,FormData,FormModel,FileRefId, dbo.XF_FileDownList(FileRefId),dbo.XF_OffetTime(SYSDTE," + CSTimeOffset.ToString() + "),dbo.XF_UserName(SYSUSR) SYSUSR " +
                          " from GenForm where FormId = N'" + formId + "' ";

            DataTable dt = SQLData.SQLDataTable(ssql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FormDesc = dt.Rows[0][0].ToString();
                FormModel = dt.Rows[0][2].ToString();
                FormData = dt.Rows[0][1].ToString();
                FileRefId = string.IsNullOrEmpty(dt.Rows[0][3].ToString()) ? NewID(1) : dt.Rows[0][3].ToString();
                FormAttaches = dt.Rows[0][4].ToString();
                FormWritten = dt.Rows[0][6].ToString() + " (" + dt.Rows[0][5].ToString() + ")";

                FormValues = (List<NameValue>)DeserializeObject(FormData, typeof(List<NameValue>));
                if (FormValues == null) FormValues = new List<NameValue>();

                xForm = (XForm)DeserializeObject(FormModel, typeof(XForm));
            }
        }

        public override void OnInitialized()
        {
            HtmlDoc.AddJsFile("WebScript.js");
            HtmlDoc.AddCSSFile("WebStyle.css");
            HtmlDoc.SetTitle(Translator.Format("preview") + " - " + (xForm != null ? xForm.Title : string.Empty));

            HtmlDoc.HtmlBodyAddOn = xForm == null ? string.Empty : XUIForm(FormAttaches).HtmlText;
        }

        private UIForm XUIForm(string AttachedFiles = "")
        {
            WebBase wb = new WebBase();
            UIForm _UIForm = wb.UIFormFromXForm(xForm, UIModes.View, FormValues, AttachedFiles);
            return _UIForm;
        }
    }

}
