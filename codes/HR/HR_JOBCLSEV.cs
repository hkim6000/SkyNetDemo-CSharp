using Microsoft.Data.SqlClient;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_JOBCLSEV : WebGridEV
    {
        public HR_JOBCLSEV()
        {
            MyPageType = "HR_JOBCLS";
        }

        public override void InitialViewData()
        {
            string InitValue = "OCCUCLS";
            string InitValue1 = "{1|SD03|*}";

            ViewPart.UIControl = new UIControl
            {
                KeyLockOnEdit = true,
                UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit
            };

            ViewPart.UIControl.Set(new UIControl.Item[] {
            new UIControl.Item { Name = "CODE", Label = "id", IsKey = true, IsRequired = true, IsVisible = false, LineSpacing = 1, ValueType = ValueTypes.Defalt, InitialValues = InitValue },
            new UIControl.Item { Name = "SNO", Label = "code", Styles = "width:100px;", Attributes = "maxlength:10;", IsKey = true, IsRequired = true, LineSpacing = 10 },
            new UIControl.Item { Name = "SD01", Label = "name", Styles = "width:250px;", Attributes = "maxlength:50;", IsRequired = true, LineSpacing = 10 },
            new UIControl.Item { Name = "SD02", Label = "alias", Styles = "width:100px;", Attributes = "maxlength:10;", LineSpacing = 10 },
            new UIControl.Item { Name = "SD03", Label = "inuse", UIType = UITypes.Checkbox, InitialValues = InitValue1, LineSpacing = 20 }
        });

            string SSQL = " Select CODE,SNO,SD01,SD02,SD03 from XysOption  where CODE=N'OCCUCLS' and SNO = N'" + PartialData + "' ";
            ViewPart.BindData(SSQL);
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string SNO = ViewPart.Field("SNO").value;
            string SD01 = ViewPart.Field("SD01").value;

            if (string.IsNullOrEmpty(SNO) || string.IsNullOrEmpty(SD01))
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
                SQL.Add(" insert into XysOption(CODE, SNO, SD01, SD02, SD03, SD04, SD05, SD06, SD07)  " +
                        " values(@CODE,@SNO,@SD01,@SD02,@SD03,@SD04,@SD05,@SD06,@SD07) ");
            }
            else
            {
                SQL.Add(" Update XysOption set " +
                      "   SD01 = @SD01, " +
                      "   SD02 = @SD02, " +
                      "   SD03 = @SD03, " +
                      "   SD04 = @SD04, " +
                      "   SD05 = @SD05, " +
                      "   SD06 = @SD06, " +
                      "   SD07 = @SD07 " +
                      " Where  CODE = @CODE and SNO = @SNO  ");
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@CODE", Value = ViewPart.Field("CODE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SNO", Value = ViewPart.Field("SNO").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SD01", Value = ViewPart.Field("SD01").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SD02", Value = ViewPart.Field("SD02").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SD03", Value = ViewPart.Field("SD03").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SD04", Value = string.Empty, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SD05", Value = string.Empty, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SD06", Value = string.Empty, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SD07", Value = string.Empty, SqlDbType = SqlDbType.NVarChar });

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
           " delete from XysOption where  CODE = @CODE and SNO = @SNO  "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@CODE", Value = ViewPart.Field("CODE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SNO", Value = ViewPart.Field("SNO").value, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }
}
