using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using ASPNETCoreWeb.codes.XysPages;
using System.Data;

namespace ASPNETCoreWeb.codes.Forms
{

    public class GenFormEV : WebBase
    {
        private XForm xForm = null;
        private string FormModel = string.Empty;
        private string FormDesc = string.Empty;
        private string FormData = string.Empty;
        private string FormWritten = string.Empty;
        private string FormAttaches = string.Empty;
        private string FileRefId = string.Empty;
        private List<NameValue> FormValues = null;

        public override void InitialViewData()
        {
            string ssql = " select FormDesc,FormData,FormModel,FileRefId, dbo.XF_FileDownList(FileRefId),dbo.XF_OffetTime(SYSDTE," + CSTimeOffset.ToString() + "),dbo.XF_UserName(SYSUSR) SYSUSR " +
                          " from GenForm where FormId = N'" + RequestData + "' ";

            DataTable dt = SQLData.SQLDataTable(ssql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FormDesc = dt.Rows[0][0].ToString();
                FormData = dt.Rows[0][1].ToString();
                FormModel = dt.Rows[0][2].ToString();
                FileRefId = string.IsNullOrEmpty(dt.Rows[0][3].ToString()) ? NewID(1) : dt.Rows[0][3].ToString();
                FormAttaches = dt.Rows[0][4].ToString();
                FormWritten = dt.Rows[0][6].ToString() + " (" + dt.Rows[0][0].ToString() + ")";

                FormValues = (List<NameValue>)DeserializeObject(FormData, typeof(List<NameValue>));
                if (FormValues == null) FormValues = new List<NameValue>();

                xForm = (XForm)DeserializeObject(FormModel, typeof(XForm));
            }
        }

        public override string InitialViewHtml()
        {
            HtmlDoc.SetTitle(xForm?.Title ?? string.Empty);
            HtmlDoc.HtmlBodyText = string.Empty;

            MenuIcon mIcon = new MenuIcon();
            mIcon.Wrap.SetAttribute(HtmlEvents.onclick, CallAction("ToggleMenu"));
            mIcon.Wrap.SetStyle(HtmlStyles.margin, "4px");
            mIcon.Type = MenuIcon.Types.Bar;

            Hidden mStatus = new Hidden("mstatus");
            mStatus.SetAttribute(HtmlAttributes.value, "0");

            Hidden FormId = new Hidden("FormId");
            FormId.SetAttribute(HtmlAttributes.value, RequestData);

            Hidden FormFileId = new Hidden("FormFileId");
            FormFileId.SetAttribute(HtmlAttributes.value, FileRefId);

            TextArea Desc = new TextArea(Translator.Format("description") + " - " + FormWritten);
            Desc.Text.InnerText = FormDesc;
            Desc.Text.SetStyle(HtmlStyles.height, "50px");
            Desc.Text.SetStyle(HtmlStyles.width, "96%");
            Desc.Wrap.SetStyle(HtmlStyles.marginLeft, "10px");
            Desc.Text.SetAttribute(HtmlAttributes.id, "FormDesc");

            HtmlElementBox elmBox = new HtmlElementBox();
            elmBox.SetAttribute(HtmlAttributes.id, References.Elements.ElmBox);
            elmBox.SetStyle(HtmlStyles.position, "fixed");
            elmBox.SetStyle(HtmlStyles.top, "8px");
            elmBox.SetStyle(HtmlStyles.left, "16px");
            elmBox.SetStyle(HtmlStyles.width, "42px");
            elmBox.SetStyle(HtmlStyles.height, "44px");
            elmBox.SetStyle(HtmlStyles.backgroundColor, "#fefefe");
            elmBox.SetStyle(HtmlStyles.borderColor, "#222");
            elmBox.SetStyle(HtmlStyles.overflow, "hidden");

            elmBox.AddItem(mIcon);
            elmBox.AddItem(GetViewButtons(null, "float:right; margin:10px;"), 16);
            elmBox.AddItem(Desc);
            elmBox.AddItem(mStatus);
            elmBox.AddItem(FormId);
            elmBox.AddItem(FormFileId);

            string ViewHtml = (xForm == null ? string.Empty : XUIForm(FormAttaches).HtmlText) + elmBox.HtmlText;
            return ViewHtml;
        }

        public ApiResponse FileDownLoad()
        {
            return FileDownLoadNorm();
        }

        public ApiResponse ToggleMenu()
        {
            string mstatus = ParamValue("mstatus");
            ApiResponse _ApiResponse = new ApiResponse();

            switch (Common.Val(string.IsNullOrEmpty(mstatus) ? "0" : mstatus))
            {
                case 0:
                    _ApiResponse.SetElementValue("mstatus", "1");
                    _ApiResponse.SetElementStyle(References.Elements.ElmBox, "width", "70%");
                    _ApiResponse.SetElementStyle(References.Elements.ElmBox, "height", "160px");
                    break;
                case 1:
                    _ApiResponse.SetElementValue("mstatus", "0");
                    _ApiResponse.SetElementStyle(References.Elements.ElmBox, "width", "42px");
                    _ApiResponse.SetElementStyle(References.Elements.ElmBox, "height", "44px");
                    break;
            }

            return _ApiResponse;
        }

        private UIForm XUIForm(string AttachedFiles = "")
        {
            UIForm _UIForm = UIFormFromXForm(xForm, UIModes.Edit, FormValues, AttachedFiles);
            return _UIForm;
        }

        public ApiResponse SaveData()
        {
            string FormId = ParamValue("FormId");
            string FormFileId = ParamValue("FormFileId");

            ApiResponse _ApiResponse = new ApiResponse();
            if (string.IsNullOrEmpty(FormId))
            {
                _ApiResponse.PopUpWindow(DialogMsgRequred());
            }
            else
            {
                string rlt = PutSaveData();
                if (string.IsNullOrEmpty(rlt))
                {
                    rlt = UploadFile("Form$Files", FormFileId); // "Form$Files" : reserved file element id by system
                    _ApiResponse.PopUpWindow(DialogMsgSaved(string.Empty));
                }
                else
                {
                    _ApiResponse.PopUpWindow(DialogMsg(rlt));
                }
            }

            return _ApiResponse;
        }

        private string PutSaveData()
        {
            string FormId = ParamValue("FormId");
            string FormDescParam = ParamValue("FormDesc");
            string FormFileId = ParamValue("FormFileId");

            string ssql = " select FormModel from GenForm where FormId = N'" + FormId + "' ";
            string Model = SQLData.SQLFieldValue(ssql);
            xForm = (XForm)DeserializeObject(Model, typeof(XForm));

            UIForm _UIForm = XUIForm();
            List<NameValue> NVList = _UIForm.GetElementValues();
            string FormDataStr = SerializeObject(NVList, typeof(List<NameValue>));

            List<string> SQL = new List<string>();
            SQL.Add(" Update GenForm set " +
                    "   FormDesc = @FormDesc, " +
                    "   FormData = @FormData, " +
                    "   FileRefId = (case when @FileRefId='' then FileRefId else @FileRefId end), " +
                    "   SYSDTE = getdate(), SYSUSR = @SYSUSR " +
                    " Where  FormId = @FormId  ");

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@FormId", Value = FormId, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@FormDesc", Value = FormDescParam, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@FormData", Value = FormDataStr, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@FileRefId", Value = FormFileId, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSUSR", Value = AppKey.UserId, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }

        public ApiResponse DeleteData()
        {
            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.PopUpWindow(DialogQstDelete("GenFormEV/ConfirmDeleteData"));
            return _ApiResponse;
        }

        public ApiResponse ConfirmDeleteData()
        {
            ApiResponse _ApiResponse = new ApiResponse();
            string rlt = PutDeleteData();
            if (string.IsNullOrEmpty(rlt))
            {
                _ApiResponse.PopUpWindow(DialogMsgDeleted("m=GenForm", "GenForm/Navigate2"));
            }
            else
            {
                _ApiResponse.PopUpWindow(DialogMsg(rlt));
            }
            return _ApiResponse;
        }

        private string PutDeleteData()
        {
            string FormId = ParamValue("FormId");

            List<string> SQL = new List<string>
        {
            " delete from GenForm where FormId = @FormId "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@FormId", Value = FormId, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }

        public ApiResponse Preview()
        {
            string FormId = ParamValue("FormId");
            string xparam = EncryptString(FormId);
            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.NewWindow("GenFormPrvw?x=" + xparam);
            return _ApiResponse;
        }
    }


}
