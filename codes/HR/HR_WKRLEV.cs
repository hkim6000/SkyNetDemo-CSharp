using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{
    public class HR_WKRLEV : WebGridEV
    {
        public HR_WKRLEV()
        {
            MyPageType = "HR_WKRL";
        }

        public override void InitialViewData()
        {
            string InitValue = "{1|RLACCR}";
            string InitValue1 = "{1|RLPAID}";
            string InitValue2 = "{1|RLFLAG|*}";

            ViewPart.UIControl = new UIControl
            {
                KeyLockOnEdit = true,
                UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit
            };

            ViewPart.UIControl.Set(new UIControl.Item[] {
                new UIControl.Item { Name = "RLCD", Label = "RLCD", Styles = "width:100px;", Attributes = "maxlength:4;", IsRequired = true, LineSpacing = 1 },
                new UIControl.Item { Name = "RLNM", Label = "RLNM", Styles = "width:200px;", Attributes = "maxlength:30;", IsRequired = true, LineSpacing = 1 },
                new UIControl.Item { Name = "RLDESC", Label = "RLDESC", Styles = "width:200px;", LineSpacing = 10 },
                new UIControl.Item { Name = "RLACCR", Label = "RLACCR", UIType = UITypes.Checkbox, InitialValues = InitValue, LineSpacing = 1 },
                new UIControl.Item { Name = "RLPAID", Label = "RLPAID", UIType = UITypes.Checkbox, InitialValues = InitValue1, LineSpacing = 1 },
                new UIControl.Item { Name = "RLFLAG", Label = "RLFLAG", UIType = UITypes.Checkbox, InitialValues = InitValue2, LineSpacing = 20 }
            });

            string SSQL = " Select RLCD,RLNM,RLDESC,RLACCR,RLPAID,RLFLAG,SYSDTE,SYSUSR from HR_WKRL where RLCD = N'" + PartialData + "' ";

            ViewPart.BindData(SSQL);
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string RLCD = ViewPart.Field("RLCD").value;
            string RLNM = ViewPart.Field("RLNM").value;

            if (string.IsNullOrEmpty(RLCD) || string.IsNullOrEmpty(RLNM))
            {
                rtnvlu = "msg_required";
            }

            if (!string.IsNullOrEmpty(RLCD) && RLCD.Length != 4)
            {
                rtnvlu = "msg_rlcdlength";
            }

            return rtnvlu;
        }

        protected override string PutSaveData()
        {
            List<string> SQL = new List<string>();

            if (ViewPart.Data == null)
            {
                SQL.Add(" Insert into HR_WKRL( RLCD,RLNM,RLDESC,RLACCR,RLPAID,RLFLAG,SYSDTE,SYSUSR) " +
                        " values ( @RLCD, @RLNM, @RLDESC, @RLACCR, @RLPAID, @RLFLAG, getdate(), @SYSUSR) ");
            }
            else
            {
                SQL.Add(" Update HR_WKRL set " +
                        " RLNM = @RLNM, " +
                        " RLDESC = @RLDESC, " +
                        " RLACCR = @RLACCR, " +
                        " RLPAID = @RLPAID, " +
                        " RLFLAG = @RLFLAG, " +
                        " SYSDTE = getdate(), SYSUSR = @SYSUSR " +
                        " Where  RLCD = @RLCD   ");
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@RLCD", Value = ViewPart.Field("RLCD").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@RLNM", Value = ViewPart.Field("RLNM").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@RLDESC", Value = ViewPart.Field("RLDESC").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@RLACCR", Value = Common.Val(ViewPart.Field("RLACCR").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@RLPAID", Value = Common.Val(ViewPart.Field("RLPAID").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@RLFLAG", Value = Common.Val(ViewPart.Field("RLFLAG").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSUSR", Value = AppKey.UserId, SqlDbType = SqlDbType.NVarChar });

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
           " delete from HR_WKRL where RLCD = @RLCD  "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@RLCD", Value = ViewPart.Field("RLCD").value, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }

}
