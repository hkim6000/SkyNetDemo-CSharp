using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_WKMSTEV : WebGridEV
    {
        public HR_WKMSTEV()
        {
            MyPageType = "HR_WKMST";
        }

        public override void InitialViewData()
        {
            string MyDataName = "HR_WKMST";

            UIModes UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit;
            ViewPart.UIControl = new UIControl { UIMode = UIMode };

            /*
            ViewPart.UIControl.Set(new UIControl.Item[] {
                new UIControl.Item { Name = "DEPTNO", Label = "DEPTNO", IsReadOnly = true, IsVisible = false, ValueType = ValueTypes.Defalt, LineSpacing = 1 },
                new UIControl.Item { Name = "DEPTID", Label = "DEPTID", Styles = "width:120px;", IsReadOnly = true, IsVisible = false, ValueType = ValueTypes.Defalt, LineSpacing = 1 },
                new UIControl.Item { Name = "DEPTBGN", Label = "DEPTBGN", Styles = "width:130px;", IsReadOnly = true, IsVisible = false, UIType = UITypes.Date, LineSpacing = 2 },
                new UIControl.Item { Name = "SITEID", Label = "SITEID", Styles = "width:380px;", IsReadOnly = true, UIType = UITypes.Dropdown, InitialValues = InitValue, LineSpacing = 2 },
                new UIControl.Item { Name = "DEPTNAME", Label = "DEPTNAME", Styles = "width:368px;", IsReadOnly = true, LineSpacing = 2 },
                new UIControl.Item { Name = "TOPDEPT", Label = "TOPDEPT", Styles = "width:380px;", IsReadOnly = true, UIType = UITypes.Dropdown, InitialValues = InitValue1, LineSpacing = 2 },
                new UIControl.Item { Name = "DEPTSTATUS", Label = "DEPTSTATUS", Styles = "width:380px;", IsReadOnly = true, UIType = UITypes.Dropdown, InitialValues = InitValue2, LineSpacing = 2 },
                new UIControl.Item { Name = "DEPTDESC", Label = "DEPTDESC", Styles = "width:368px;", IsReadOnly = true, LineSpacing = 2 },
                new UIControl.Item { Name = "DEPTORDER", Label = "DEPTORDER", Styles = "width:80px;padding-left:4px;", IsReadOnly = true, UIType = UITypes.Text },
                new UIControl.Item { Name = "DEPTFLAG", Label = "DEPTFLAG", WrapStyles = "margin-left:20px;", IsReadOnly = true, UIType = UITypes.Checkbox, InitialValues = InitValue3, LineSpacing = 20 }
            });
            */

            string SSQL = " Select WKNO,EMPID,WKBDT,WKCD1,WKCD2,WKCD3,WKCD4,WKCD5,WKFLAG " +
                          " from VHR_WKMST where EMPID = N'" + PartialData + "' ";

            ViewPart.BindData(SSQL);

            if (!string.IsNullOrEmpty(ViewPart.Field("WKBDT").value))
            {
                ViewPart.Field("WKBDT").value = DateTime.Parse(ViewPart.Field("WKBDT").value).ToString(StdDateFormat);
            }
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string EMPID = ViewPart.Field("EMPID").value;
            string WKBDT = ViewPart.Field("WKBDT").value;

            if (string.IsNullOrEmpty(EMPID) || string.IsNullOrEmpty(WKBDT))
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
                SQL.Add(" declare @seq int " +
                        " exec dbo.XP_NextSeq N'HR_WKMST',N'WKNO',@seq out " +
                        " Insert into HR_WKMSTHS( WKNO,EMPID,WKBDT,WKEDT,WKCD1,WKCD2,WKCD3,WKCD4,WKCD5,SYSDTE,SYSUSR) " +
                        " values ( @seq, @EMPID, @WKBDT, @WKEDT, @WKCD1, @WKCD2, @WKCD3, @WKCD4, @WKCD5, getdate(), @SYSUSR) ");
            }
            else
            {
                SQL.Add(" Update HR_WKMST set " +
                        " OGNM = dbo.FHR_OGNM(@DEPTID,@OGA), " +
                        " EMPID = @EMPID, " +
                        " WKBDT = @WKBDT, " +
                        " WKEDT = @WKEDT, " +
                        " WKCD1 = @WKCD1, " +
                        " WKCD2 = @WKCD2, " +
                        " WKCD3 = @WKCD3, " +
                        " WKCD4 = @WKCD4, " +
                        " WKCD5 = @WKCD5, " +
                        " SYSDTE = getdate(), SYSUSR = @SYSUSR " +
                        " WHERE OGNO = @OGNO ");
            }

            // Maintaining original logic: setting LEADER to 0 if FLAG is 0 (adapted from original var names)
            if (Common.Val(ViewPart.Field("EMPFLAG").value) == 0)
            {
                ViewPart.Field("EMPFLAG").value = "0";
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@WKNO", Value = ViewPart.Field("WKNO").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPID", Value = ViewPart.Field("EMPID").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKBDT", Value = ViewPart.Field("WKBDT").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKEDT", Value = ViewPart.Field("WKEDT").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKCD1", Value = ViewPart.Field("WKCD1").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKCD2", Value = ViewPart.Field("WKCD2").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKCD3", Value = ViewPart.Field("WKCD3").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKCD4", Value = ViewPart.Field("WKCD4").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@WKCD5", Value = ViewPart.Field("WKCD5").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSDTE", Value = ViewPart.Field("SYSDTE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSUSR", Value = ViewPart.Field("SYSUSR").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGNO", Value = Common.Val(ViewPart.Field("OGNO").value), SqlDbType = SqlDbType.Int });

            return PutData(SqlWithParams(SQL, SqlParams));
        }

        protected override string VerifyDelete()
        {
            return string.Empty;
        }

        protected override string PutDeleteData()
        {
            List<string> SQL = new List<string> {
            " delete from HR_WKMST WHERE OGNO = @OGNO "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@OGNO", Value = Common.Val(ViewPart.Field("OGNO").value), SqlDbType = SqlDbType.Int });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }

}
