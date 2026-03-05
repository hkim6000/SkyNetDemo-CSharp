using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{
    public class HR_WKDAYEV : WebGridEV
    {
        public HR_WKDAYEV()
        {
            MyPageType = "HR_WKDAY";
        }

        public override void InitialViewData()
        {
            // Dim InitValue1 As String = "{0|0}{1|1||color:#ff6600}{2|2||color:#1C82BA}"
            string InitValue1 = "SQL@ SELECT SNO,SD01 FROM XysOption where SD03='0' AND CODE = 'DAYTYPE' ORDER BY SNO";

            ViewPart.UIControl = new UIControl
            {
                KeyLockOnEdit = true,
                UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit
            };

            ViewPart.UIControl.Set(new UIControl.Item[] {
                new UIControl.Item { Name = "WKDAY", Label = "WKDAY", UIType = UITypes.Date, Styles = "width:120px;", IsKey = true, IsRequired = true, LineSpacing = 10 },
                new UIControl.Item { Name = "DAYTYPE", Label = "DAYTYPE", UIType = UITypes.Dropdown, Styles = "width:134px;", InitialValues = InitValue1, LineSpacing = 10 },
                new UIControl.Item { Name = "DAYMEMO", Label = "DAYMEMO", Styles = "width:300px;", Attributes = "maxlength:50;", LineSpacing = 10 }
            });

            string SSQL = " Select WKDAY,DAYTYPE,DAYMEMO from HR_WKDAY  where WKDAY = N'" + PartialData + "' ";
            ViewPart.BindData(SSQL);
        }

        public override string InitialViewHtml()
        {
            MenuList ViewMenuItems = GetViewMenuItems();
            Wrap ViewButtons = GetViewButtons(new string[] { "save" });

            Label label = new Label();
            label.Wrap.SetStyles("font-weight:700;font-size:22px;margin:12px;");
            label.Wrap.InnerText = ViewPart.UIControl.UIMode == UIModes.New ? Translator.Format("new") : Translator.Format("edit");

            FilterSection filter = new();
            filter.ModalWrap = true;
            filter.Wrap.SetStyle(HtmlStyles.marginTop, "110px");
            filter.Wrap.SetStyle(HtmlStyles.width, "90%");
            filter.Menu = ViewMenuItems;
            filter.FilterHtml = label.HtmlText;

            HtmlElementBox elmBox = new HtmlElementBox();
            elmBox.SetAttribute(HtmlAttributes.id, References.Elements.ElmBox);
            elmBox.SetStyle(HtmlStyles.width, "90%");
            elmBox.SetStyle(HtmlStyles.margin, "auto");
            elmBox.SetStyle(HtmlStyles.marginTop, "8px");
            elmBox.SetStyle(HtmlStyles.marginBottom, "80px");
            elmBox.Wrap.SetStyle(HtmlStyles.margin, "50px 30px 30px 50px");

            elmBox.AddItem(ViewPart.UIControl, 20);
            elmBox.AddItem(ViewButtons, 20);

            return filter.HtmlText + elmBox.HtmlText;
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string WKDAY = ViewPart.Field("WKDAY").value;
            string DAYTYPE = ViewPart.Field("DAYTYPE").value;

            if (string.IsNullOrEmpty(WKDAY) || string.IsNullOrEmpty(DAYTYPE))
            {
                rtnvlu = "msg_required";
            }
            return rtnvlu;
        }

        protected override string PutSaveData()
        {
            List<string> SQL = new List<string>();

            if (ViewPart.Data == null)
            {
                SQL.Add(" insert into HR_WKDAY(WKDAY,DAYTYPE,DAYMEMO)  " +
                        " values(@WKDAY,@DAYTYPE,@DAYMEMO) ");
            }
            else
            {
                SQL.Add(" Update HR_WKDAY set " +
                      "   DAYTYPE = @DAYTYPE, " +
                      "   DAYMEMO = @DAYMEMO  " +
                      " Where  WKDAY = @WKDAY   ");
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@WKDAY", Value = ViewPart.Field("WKDAY").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DAYTYPE", Value = ViewPart.Field("DAYTYPE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DAYMEMO", Value = ViewPart.Field("DAYMEMO").value, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }

        protected override string VerifyDelete()
        {
            return string.Empty;
        }

        protected override string PutDeleteData()
        {
            List<string> SQL = new List<string>
        {
           " delete from HR_WKDAY where  WKDAY = @WKDAY  "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@WKDAY", Value = ViewPart.Field("WKDAY").value, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }

}
