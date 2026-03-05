using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_SITEEV : WebGridEV
    {
        public HR_SITEEV()
        {
            MyPageType = "HR_SITE";
        }

        public override void InitialViewData()
        {
            string MyDataName = "HR_SITE";

            UIModes UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit;
            ViewPart.UIControl = UIControlFromXDataElements(MyDataName, XDataElementsFromDataName(MyDataName), UIMode);

            string SSQL = " Select SITEID,SITENAME,SITEALIAS,SITETYPE,SITEREGNO,SITETAXNO,SITEOWNER,SITEBIZ1,SITEBIZ2, " +
                          "        SITEADDR1,SITEADDR2,SITECITY,SITESTATE,SITEZIP,SITECOUNTRY,SITEPHONE,SITEFAX,SITEMAIL, " +
                          "        SITEWEB,SITEORDER,SITEFLAG,SYSDTE,SYSUSR from HR_SITE  where SITEID = N'" + PartialData + "'";
            ViewPart.BindData(SSQL);
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string SITEID = ViewPart.Field("SITEID").value;
            string SITENAME = ViewPart.Field("SITENAME").value;

            if (string.IsNullOrEmpty(SITEID) || string.IsNullOrEmpty(SITENAME))
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
                SQL.Add(" Insert into HR_SITE(SITEID,SITENAME,SITEALIAS,SITETYPE,SITEREGNO,SITETAXNO,SITEOWNER,SITEBIZ1,SITEBIZ2,SITEADDR1,SITEADDR2,SITECITY,SITESTATE,SITEZIP,SITECOUNTRY,SITEPHONE,SITEFAX,SITEMAIL,SITEWEB,SITEORDER,SITEFLAG,SYSDTE,SYSUSR ) " +
                        " values ( @SITEID, @SITENAME, @SITEALIAS, @SITETYPE, @SITEREGNO, @SITETAXNO, @SITEOWNER, @SITEBIZ1, @SITEBIZ2, @SITEADDR1, @SITEADDR2, @SITECITY, @SITESTATE, @SITEZIP, @SITECOUNTRY, @SITEPHONE, @SITEFAX, @SITEMAIL, @SITEWEB, @SITEORDER, @SITEFLAG, @SYSDTE, @SYSUSR)  ");
            }
            else
            {
                SQL.Add(" Update HR_SITE set " +
                        " SITENAME = @SITENAME, " +
                        " SITEALIAS = @SITEALIAS, " +
                        " SITETYPE = @SITETYPE, " +
                        " SITEREGNO = @SITEREGNO, " +
                        " SITETAXNO = @SITETAXNO, " +
                        " SITEOWNER = @SITEOWNER, " +
                        " SITEBIZ1 = @SITEBIZ1, " +
                        " SITEBIZ2 = @SITEBIZ2, " +
                        " SITEADDR1 = @SITEADDR1, " +
                        " SITEADDR2 = @SITEADDR2, " +
                        " SITECITY = @SITECITY, " +
                        " SITESTATE = @SITESTATE, " +
                        " SITEZIP = @SITEZIP, " +
                        " SITECOUNTRY = @SITECOUNTRY, " +
                        " SITEPHONE = @SITEPHONE, " +
                        " SITEFAX = @SITEFAX, " +
                        " SITEMAIL = @SITEMAIL, " +
                        " SITEWEB = @SITEWEB, " +
                        " SITEORDER = @SITEORDER, " +
                        " SITEFLAG = @SITEFLAG, " +
                        " SYSDTE = getdate(), SYSUSR = @SYSUSR " +
                        " WHERE  SITEID = @SITEID ");
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEID", Value = ViewPart.Field("SITEID").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITENAME", Value = ViewPart.Field("SITENAME").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEALIAS", Value = ViewPart.Field("SITEALIAS").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITETYPE", Value = ViewPart.Field("SITETYPE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEREGNO", Value = ViewPart.Field("SITEREGNO").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITETAXNO", Value = ViewPart.Field("SITETAXNO").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEOWNER", Value = ViewPart.Field("SITEOWNER").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEBIZ1", Value = ViewPart.Field("SITEBIZ1").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEBIZ2", Value = ViewPart.Field("SITEBIZ2").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEADDR1", Value = ViewPart.Field("SITEADDR1").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEADDR2", Value = ViewPart.Field("SITEADDR2").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITECITY", Value = ViewPart.Field("SITECITY").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITESTATE", Value = ViewPart.Field("SITESTATE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEZIP", Value = ViewPart.Field("SITEZIP").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITECOUNTRY", Value = ViewPart.Field("SITECOUNTRY").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEPHONE", Value = ViewPart.Field("SITEPHONE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEFAX", Value = ViewPart.Field("SITEFAX").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEMAIL", Value = ViewPart.Field("SITEMAIL").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEWEB", Value = ViewPart.Field("SITEWEB").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEORDER", Value = Common.Val(ViewPart.Field("SITEORDER").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEFLAG", Value = Common.Val(ViewPart.Field("SITEFLAG").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSDTE", Value = ViewPart.Field("SYSDTE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSUSR", Value = AppKey.UserId, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }

        protected override string VerifyDelete()
        {
            string rtnvlu = string.Empty;

            string SITEID = ViewPart.Field("SITEID").value;

            string ExistInOrg = SQLData.SQLFieldValue("select count(*) from HR_DeptHS where SiteId = N'" + SITEID + "'");
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
            " delete from HR_SITE WHERE SITEID = @SITEID "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEID", Value = ViewPart.Field("SITEID").value, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }

}
