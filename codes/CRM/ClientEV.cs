using ASPNETCoreWeb.codes.XysBases;

namespace ASPNETCoreWeb.codes.CRM
{

    public class ClientEV : WebBase
    {
        /*
        public ClientEV()
        {
            ViewPart.Fields.AddRange(new NameValueFlag[] {
                new NameValueFlag { name = "ClientId", flag = true },
                new NameValueFlag { name = "ClientName" },
                new NameValueFlag { name = "ClientData", flag = true }
            });
        }

        public override void InitialViewData()
        {
            string SSQL = " Select ClientId,ClientName,ClientData from Client where ClientId = N'" + PartialData + "'";

            ViewPart.BindData(SSQL);
        }

        public override string InitialViewHtml()
        {
            MenuList ViewMenuItems = GetViewMenuItems();
            Wrap ViewButtons = GetViewButtons(ViewPart.Data == null ? new string[] { "save" } : null);

            Label label = new Label();
            label.Wrap.SetStyles("font-weight:700;font-size:22px;margin:12px;");
            label.Wrap.InnerText = ViewPart.Data == null ? Translator.Format("newclient") : Translator.Format("editclient");

            ToolKit.FilterSection filter = new ToolKit.FilterSection();
            filter.ModalWrap = true;
            filter.Wrap.SetStyle(HtmlStyles.marginTop, "110px");
            filter.Wrap.SetStyle(HtmlStyles.width, "90%");
            filter.Menu = ViewMenuItems;
            filter.FilterHtml = label.HtmlText;

            Texts text = new Texts(Translator.Format("clientname"), ViewPart.Field("ClientName").name, TextTypes.text);
            text.Required = true;
            text.Text.SetStyle(HtmlStyles.width, "200px");
            text.Text.SetAttribute(HtmlAttributes.maxlength, "200");
            text.Text.SetAttribute(HtmlAttributes.value, ViewPart.Field("ClientName").value);
            text.Wrap.SetStyle(HtmlStyles.paddingLeft, "4px");

            UIControl UIControl = new UIControl();
            DynamicUIControl("client", ViewPart.Field("ClientData").value, UIControl);

            HtmlElementBox elmBox = new HtmlElementBox();
            elmBox.SetAttribute(HtmlAttributes.id, References.Elements.ElmBox);
            elmBox.SetStyle(HtmlStyles.width, "90%");
            elmBox.SetStyle(HtmlStyles.margin, "auto");
            elmBox.SetStyle(HtmlStyles.marginTop, "8px");
            elmBox.SetStyle(HtmlStyles.marginBottom, "80px");
            elmBox.Wrap.SetStyle(HtmlStyles.margin, "50px 30px 30px 50px");

            elmBox.AddItem(text, 0);
            elmBox.AddItem(UIControl, 20);
            elmBox.AddItem(ViewButtons, 20);

            string ViewHtml = filter.HtmlText + elmBox.HtmlText;
            return ViewHtml;
        }

        public ApiResponse SaveData()
        {
            string ClientName = ViewPart.Field("ClientName").value;

            ApiResponse _ApiResponse = new ApiResponse();
            if (string.IsNullOrEmpty(ClientName) || DynamicUIDataCheck("client") == true)
            {
                _ApiResponse.PopUpWindow(DialogMsgRequred, References.Elements.PageContents);
            }
            else
            {
                string rlt = PutSaveData();
                if (string.IsNullOrEmpty(rlt))
                {
                    _ApiResponse.PopUpWindow(DialogMsgSaved("m=ClientMV"), References.Elements.PageContents);
                }
                else
                {
                    _ApiResponse.PopUpWindow(DialogMsg(rlt), References.Elements.PageContents);
                }
            }

            return _ApiResponse;
        }

        private string PutSaveData()
        {
            List<string> SQL = new List<string>();

            ViewPart.Field("ClientData").value = DynamicUIData("client");

            if (ViewPart.Data == null)
            {
                ViewPart.Field("ClientId").value = NewID();
                SQL.Add(" Insert into Client(ClientId,ClientName,ClientData,SysDte,SysUsr) " +
                        " values( @ClientId,@ClientName,@ClientData, getdate(), @SYSUSR) ");
            }
            else
            {
                SQL.Add(" Update Client set " +
                        " ClientName = @ClientName, ClientData = @ClientData, " +
                        " SYSDTE = getdate(), SYSUSR = @SYSUSR " +
                        " WHERE ClientId = @ClientId");
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@ClientId", Value = ViewPart.Field("ClientId").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@ClientName", Value = ViewPart.Field("ClientName").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@ClientData", Value = ViewPart.Field("ClientData").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSUSR", Value = AppKey.UserId, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }

        public ApiResponse DeleteData()
        {
            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.PopUpWindow(DialogQstDelete("ClientEV/ConfirmDeleteData"), References.Elements.PageContents);
            return _ApiResponse;
        }

        public ApiResponse ConfirmDeleteData()
        {
            ApiResponse _ApiResponse = new ApiResponse();
            string rlt = PutDeleteData();
            if (string.IsNullOrEmpty(rlt))
            {
                _ApiResponse.PopUpWindow(DialogMsgDeleted("m=ClientMV"), References.Elements.PageContents);
            }
            else
            {
                _ApiResponse.PopUpWindow(DialogMsg(rlt), References.Elements.PageContents);
            }
            return _ApiResponse;
        }

        private string PutDeleteData()
        {
            List<string> SQL = new List<string> 
            {
                " delete from Client WHERE ClientId = @ClientId "
            };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@ClientId", Value = ViewPart.Field("ClientId").value, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
        */
    }
}
