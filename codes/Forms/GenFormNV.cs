using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using ASPNETCoreWeb.codes.XysPages;
using System.Data;
using static SkyNet.WebPage;

namespace ASPNETCoreWeb.codes.Forms
{

    public class GenFormNV : WebBase
    {
        private UIControl MyControl { get; set; } = new UIControl();

        public GenFormNV()
        {
            string InitValue3 = "SQL@ select '-' FormId,'Select a Form' FormTitle, 0 ord union all select FormId,FormTitle,1 ord from XysForm where FormFlag = 1 order by ord,FormTitle";

            MyControl.Set(new UIControl.Item[] {
            new UIControl.Item { Name = "FormId", ValueType = ValueTypes.GUID, IsKey = true, IsVisible = false, LineSpacing = 1 },
            new UIControl.Item { Name = "OriginId", Styles = "width:494px;", Attributes = "onchange:FormChanged('GenFormNV');", UIType = UITypes.Dropdown, InitialValues = InitValue3, LineSpacing = 10 },
            new UIControl.Item { Name = "FormDesc", Styles = "width:480px; height:100px;", UIType = UITypes.TextArea, LineSpacing = 10 }
        });

            for (int i = 0; i < MyControl.Items.Count; i++)
            {
                MyControl.Items[i].Label = Translator.Format(MyControl.Items[i].Name);
            }
        }

        public override string InitialViewHtml()
        {
            MenuList ViewMenuItems = GetViewMenuItems();
            Wrap ViewButtons = GetViewButtons(new string[] { });

            Label label = new Label();
            label.Wrap.SetStyles("font-weight:700;font-size:22px;margin:12px;");
            label.Wrap.InnerText = Translator.Format("newform");

            FilterSection filter = new FilterSection();
            filter.ModalWrap = true;
            filter.Wrap.SetStyle(HtmlStyles.marginTop, "110px");
            filter.Wrap.SetStyle(HtmlStyles.width, "92%");
            filter.Menu = ViewMenuItems;
            filter.FilterHtml = label.HtmlText;

            HtmlElementBox elmBtns = new HtmlElementBox();
            elmBtns.DefaultStyle = false;
            elmBtns.AddItem(ViewButtons, 20);

            HtmlElementBox elmBox = new HtmlElementBox();
            elmBox.SetAttribute(HtmlAttributes.id, References.Elements.ElmBox);
            elmBox.SetStyle(HtmlStyles.width, "92%");
            elmBox.SetStyle(HtmlStyles.margin, "auto");
            elmBox.SetStyle(HtmlStyles.marginTop, "8px");
            elmBox.SetStyle(HtmlStyles.marginBottom, "80px");
            elmBox.Wrap.SetStyle(HtmlStyles.margin, "30px 10px 30px 40px");

            elmBox.AddItem(MyControl, 30);
            elmBox.AddItem(elmBtns);

            string ViewHtml = filter.HtmlText + elmBox.HtmlText;

            HtmlDoc.AddJsFile(WebEnv.HeaderScripts.TreeScript);
            return ViewHtml;
        }

        public ApiResponse FormChanged()
        {
            string OriginId = ParamValue("OriginId");

            ApiResponse _ApiResponse = new ApiResponse();
            if (!string.IsNullOrEmpty(OriginId))
            {
                string ssql = " select FormModel from XysForm where FormId = N'" + OriginId + "' ";
                DataTable xFormDt = SQLData.SQLDataTable(ssql);
                if (xFormDt != null && xFormDt.Rows.Count > 0)
                {
                    XForm xForm = (XForm)DeserializeObject(xFormDt.Rows[0][0].ToString(), typeof(XForm));
                    _ApiResponse.SetElementContents("FormDesc", xForm.Description);
                }
            }
            return _ApiResponse;
        }

        public ApiResponse SaveData()
        {
            string FormId = NewID();
            string OriginId = ParamValue("OriginId");

            ApiResponse _ApiResponse = new ApiResponse();
            if (string.IsNullOrEmpty(OriginId) || OriginId == "-")
            {
                _ApiResponse.PopUpWindow(DialogMsgRequred(), References.Elements.PageContents);
            }
            else
            {
                string rlt = PutSaveData(FormId);
                if (string.IsNullOrEmpty(rlt))
                {
                    _ApiResponse.Navigate2("GenFormEV", FormId);
                }
                else
                {
                    _ApiResponse.PopUpWindow(DialogMsg(rlt), References.Elements.PageContents);
                }
            }

            return _ApiResponse;
        }

        private string PutSaveData(string FormId)
        {
            string OriginId = ParamValue("OriginId");
            string FormDesc = ParamValue("FormDesc");

            string ssql = " select FormTitle,FormModel,FormRef from XysForm where FormId = N'" + OriginId + "' ";
            DataTable xFormDt = SQLData.SQLDataTable(ssql);

            string FormTitle = xFormDt.Rows[0][0].ToString();
            string FormModel = xFormDt.Rows[0][1].ToString();
            string FormRef = xFormDt.Rows[0][2].ToString();
            string FileRefId = NewID(1);
            XForm xForm = (XForm)DeserializeObject(FormModel, typeof(XForm));

            List<string> SQL = new List<string>();

            SQL.Add(" Insert into GenForm(FormId, FormTitle,FormModel,FormData,FormDesc,FormFlag,FormRef,FileRefId,SYSDTE,SYSUSR) " +
                    " values (@FormId, @FormTitle, @FormModel,'',@FormDesc, 0, @FormRef, @FileRefId, getdate(), @SYSUSR) ");

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@FormId", Value = FormId, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@FormTitle", Value = FormTitle, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@FormModel", Value = FormModel, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@FormDesc", Value = FormDesc, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@FormRef", Value = FormRef, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@FileRefId", Value = FileRefId, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSUSR", Value = AppKey.UserId, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }

}
