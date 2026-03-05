using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_DEPTEV : WebGridEV
    {
        public HR_DEPTEV()
        {
            MyPageType = "HR_DEPT";
        }

        public override void InitialViewData()
        {
            string InitValue = "SQL@select SITEID, SITENAME from HR_SITE a order by SITEORDER ";
            string InitValue2 = "SQL@select SNO, SD01 + ' (' + SD02 + ')' from XysOption where CODE = 'DEPTSTATUS' order by convert(int, SNO) ";
            string InitValue1 = "SQL@ if exists(select * from VHR_DEPT where TOPDEPT = '')" +
                                       " begin " +
                                       "    select DEPTID, DEPTNAME from VHR_DEPT a order by DEPTID " +
                                       " End " +
                                       " else " +
                                       " begin " +
                                       "    select '' DEPTID,'' DEPTNAME " +
                                       "    union all " +
                                       "    select DEPTID, DEPTNAME from VHR_DEPT a order by DEPTID " +
                                       " End ";
            string InitValue3 = "{1|DEPTFLAG|*}";

            ViewPart.UIControl = new UIControl { UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit };
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

            string SSQL = " Select DEPTNO,DEPTID,DEPTBGN,SITEID,DEPTNAME,TOPDEPT,DEPTSTATUS,DEPTDESC,DEPTORDER,DEPTFLAG " +
                          " from VHR_DEPT  where DEPTID = N'" + PartialData + "' ";
            ViewPart.BindData(SSQL);

            if (!string.IsNullOrEmpty(ViewPart.Field("DEPTBGN").value))
            {
                ViewPart.Field("DEPTBGN").value = DateTime.Parse(ViewPart.Field("DEPTBGN").value).ToString(StdDateFormat);
            }
        }

        protected override string VerifyDelete()
        {
            string rtnvlu = string.Empty;
            string DEPTID = ViewPart.Field("DEPTID").value;

            string LowerDEPT = SQLData.SQLFieldValue("select count(*) from VHR_DEPT where DEPTFLAG = 1 and TOPDEPT = N'" + DEPTID + "'");
            if (Common.Val(LowerDEPT) > 0)
            {
                rtnvlu = "msg_lowerdept";
            }

            string ExistInOrg = SQLData.SQLFieldValue("select count(*) from HR_Org where DEPTID = N'" + DEPTID + "'");
            if (Common.Val(ExistInOrg) > 0)
            {
                rtnvlu = "msg_existinorg";
            }

            return rtnvlu;
        }

        protected override string PutDeleteData()
        {
            List<string> SQL = new List<string> {
            " delete from HR_DEPTHS WHERE  DEPTID = @DEPTID  " +
            " delete from HR_DEPT WHERE  DEPTID = @DEPTID  "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTID", Value = ViewPart.Field("DEPTID").value, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }


}
