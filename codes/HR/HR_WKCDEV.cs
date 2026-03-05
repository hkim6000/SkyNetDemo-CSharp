using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_WKCDEV : WebGridEV
    {
        public HR_WKCDEV()
        {
            MyPageType = "HR_WKCD";
        }

        public override void InitialViewData()
        {
            string InitValue1 = "{1|WKFLAG|*}";
            string InitValue2 = "sql@ select mins,title, case when mins = 480 then 1 else 0 end from dbo.FHR_IntMins(15) ";
            string InitValue3 = "sql@ select mins,title from dbo.FHR_IntMins(15) ";

            ViewPart.UIControl = new UIControl
            {
                KeyLockOnEdit = true,
                UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit
            };

            ViewPart.UIControl.Set(new UIControl.Item[]
            {
            new UIControl.Item { Name = "WKCD", Label = "WKCD", Styles = "width:60px;", Attributes = "maxlength:4;", IsKey = true, IsRequired = true, LineSpacing = 1 },
            new UIControl.Item { Name = "WKNM", Label = "WKNM", Styles = "width:300px;", Attributes = "maxlength:30;", IsKey = true, IsRequired = true, LineSpacing = 1 },
            new UIControl.Item { Name = "WKDESC", Label = "WKDESC", Styles = "width:300px;", Attributes = "maxlength:200;", LineSpacing = 20 },
            new UIControl.Item { Name = "WKBGN", Label = "WKBGN", UIType = UITypes.Time, Styles = "width:120px;", IsRequired = true, ValueType = ValueTypes.Defalt, InitialValues = "08:00", LineSpacing = 0 },
            new UIControl.Item { Name = "WKEND", Label = "WKEND", UIType = UITypes.Time, Styles = "width:120px;", IsRequired = true, ValueType = ValueTypes.Defalt, InitialValues = "17:00", LineSpacing = 20 },
            new UIControl.Item { Name = "WKA", Label = "WKA", UIType = UITypes.Dropdown, Styles = "width:90px;", InitialValues = InitValue2, IsRequired = true, LineSpacing = 0 },
            new UIControl.Item { Name = "WKB", Label = "WKB", UIType = UITypes.Dropdown, Styles = "width:90px;", InitialValues = InitValue3, LineSpacing = 5 },
            new UIControl.Item { Name = "WKC", Label = "WKC", UIType = UITypes.Dropdown, Styles = "width:90px;", InitialValues = InitValue3, LineSpacing = 0 },
            new UIControl.Item { Name = "WKD", Label = "WKD", UIType = UITypes.Dropdown, Styles = "width:90px;", InitialValues = InitValue3, LineSpacing = 0 },
            new UIControl.Item { Name = "WKE", Label = "WKE", UIType = UITypes.Dropdown, Styles = "width:90px;", InitialValues = InitValue3, LineSpacing = 20 },
            new UIControl.Item { Name = "WKFLAG", Label = "WKFLAG", UIType = UITypes.Checkbox, InitialValues = InitValue1, LineSpacing = 20 }
            });

            string SSQL = " Select WKCD,WKNM,WKDESC,WKBGN,WKEND,WKA,WKB,WKC,WKD,WKE,WKFLAG,SYSDTE,SYSUSR From HR_WKCD where WKCD = N'" + PartialData + "' ";

            ViewPart.BindData(SSQL);
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string WKCD = ViewPart.Field("WKCD").value;
            string WKNM = ViewPart.Field("WKNM").value;
            string WKBGN = ViewPart.Field("WKBGN").value;
            string WKEND = ViewPart.Field("WKEND").value;

            if (string.IsNullOrEmpty(WKCD) || string.IsNullOrEmpty(WKNM) || string.IsNullOrEmpty(WKBGN) || string.IsNullOrEmpty(WKEND))
            {
                rtnvlu = "msg_required";
            }
            else
            {
                if (WKCD.Length != 4)
                {
                    rtnvlu = "msg_wkcd4";
                }
            }

            return rtnvlu;
        }

        protected override string PutSaveData()
        {
            List<string> SQL = new List<string>();

            if (ViewPart.Data == null)
            {
                SQL.Add(" Insert into HR_WKCD( WKCD,WKNM,WKDESC,WKBGN,WKEND,WKA,WKB,WKC,WKD,WKE,WKFLAG,SYSDTE,SYSUSR ) " +
                        " values ( @WKCD, @WKNM, @WKDESC, @WKBGN, @WKEND, @WKA, @WKB, @WKC, @WKD, @WKE, @WKFLAG,getdate(), @SYSUSR) ");
            }
            else
            {
                SQL.Add(" Update HR_WKCD set " +
                        " WKNM = @WKNM, " +
                        " WKDESC = @WKDESC, " +
                        " WKBGN = @WKBGN, " +
                        " WKEND = @WKEND, " +
                        " WKA = @WKA, " +
                        " WKB = @WKB, " +
                        " WKC = @WKC, " +
                        " WKD = @WKD, " +
                        " WKE = @WKE, " +
                        " WKFLAG = @WKFLAG, " +
                        " SYSDTE = getdate(), SYSUSR = @SYSUSR " +
                        " WHERE  WKCD = @WKCD ");
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@WKCD", Value = ViewPart.Field("WKCD").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKNM", Value = ViewPart.Field("WKNM").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKDESC", Value = ViewPart.Field("WKDESC").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKBGN", Value = ViewPart.Field("WKBGN").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKEND", Value = ViewPart.Field("WKEND").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKA", Value = Common.Val(ViewPart.Field("WKA").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKB", Value = Common.Val(ViewPart.Field("WKB").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKC", Value = Common.Val(ViewPart.Field("WKC").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKD", Value = Common.Val(ViewPart.Field("WKD").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKE", Value = Common.Val(ViewPart.Field("WKE").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKFLAG", Value = Common.Val(ViewPart.Field("WKFLAG").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSUSR", Value = AppKey.UserId, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }

        protected override string VerifyDelete()
        {
            string rtnvlu = string.Empty;

            // Maintaining logic from original source despite variable name discrepancies
            string SiteId = ViewPart.Field("SiteId").value;

            string ExistInOrg = SQLData.SQLFieldValue("select count(*) from HR_DeptHS where SiteId = N'" + SiteId + "'");
            if (Common.Val(ExistInOrg) > 0)
            {
                rtnvlu = "msg_existindept";
            }

            return rtnvlu;
        }

        protected override string PutDeleteData()
        {
            List<string> SQL = new List<string>
        {
            " delete from HR_WKCD WHERE WKCD = @WKCD "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@WKCD", Value = ViewPart.Field("WKCD").value, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }

}
