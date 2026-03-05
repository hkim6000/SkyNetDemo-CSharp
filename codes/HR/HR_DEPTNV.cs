using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_DEPTNV : WebGridEV
    {
        public HR_DEPTNV()
        {
            MyPageType = "HR_DEPT";
        }

        public override void InitialViewData()
        {
            string InitValue = "SQL@select SITEID, SITENAME from HR_SITE a order by SITEORDER ";
            string InitValue2 = "SQL@select SNO, SD01 + ' (' + SD02 + ')' from XysOption where CODE = 'DEPTSTATUS' order by convert(int, SNO) ";
            string InitValue3 = "{1|DEPTFLAG|*}";

            ViewPart.UIControl = new UIControl
            {
                UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit
            };

            ViewPart.UIControl.Set(new UIControl.Item[] {
                new UIControl.Item { Name = "DEPTNO", Label = "DEPTNO", IsVisible = false, IsRequired = true, ValueType = ValueTypes.Defalt },
                new UIControl.Item { Name = "DEPTBGN", Label = "DEPTBGN", Styles = "width:130px;", Attributes = "onblur:ReloadTopDept('HR_DEPTNV/ReloadTopDept','DEPTBGN')", IsRequired = true, UIType = UITypes.Date, LineSpacing = 6 },
                new UIControl.Item { Name = "SITEID", Label = "SITEID", Styles = "width:380px;", IsRequired = true, UIType = UITypes.Dropdown, InitialValues = InitValue, LineSpacing = 6 },
                new UIControl.Item { Name = "DEPTID", Label = "DEPTID", Styles = "width:120px;", Attributes = "maxlength:16;", IsRequired = true, ValueType = ValueTypes.Defalt, LineSpacing = 1 },
                new UIControl.Item { Name = "DEPTNAME", Label = "DEPTNAME", Styles = "width:368px;", Attributes = "maxlength:100;", IsRequired = true, ValueType = ValueTypes.Defalt, LineSpacing = 6 },
                new UIControl.Item { Name = "TOPDEPT", Label = "TOPDEPT", Styles = "width:380px;", Attributes = "placeholder:Select...", UIType = UITypes.Dropdown, IsRequired = true, LineSpacing = 6 },
                new UIControl.Item { Name = "DEPTSTATUS", Label = "DEPTSTATUS", Styles = "width:380px;", UIType = UITypes.Dropdown, InitialValues = InitValue2, LineSpacing = 6 },
                new UIControl.Item { Name = "DEPTDESC", Label = "DEPTDESC", Styles = "width:368px;", Attributes = "maxlength:100;", LineSpacing = 20 },
                new UIControl.Item { Name = "DEPTORDER", Label = "DEPTORDER", Styles = "width:80px;padding-left:4px;", Attributes = "maxlength:16;", UIType = UITypes.Number, ValueType = ValueTypes.Defalt, InitialValues = "1" },
                new UIControl.Item { Name = "DEPTFLAG", Label = "DEPTFLAG", WrapStyles = "margin-left:20px;", IsReadOnly = true, IsVisible = false, UIType = UITypes.Checkbox, InitialValues = InitValue3, LineSpacing = 20 }
            });

            string SSQL = " Select DEPTNO,DEPTID,DEPTBGN,SITEID,DEPTNAME,TOPDEPT,DEPTSTATUS,DEPTDESC,DEPTORDER,DEPTFLAG from VHR_DEPTHS  where DEPTNO = " + PartialData;

            ViewPart.BindData(SSQL);
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string DEPTBGN = ViewPart.Field("DEPTBGN").value;
            string SITEID = ViewPart.Field("SITEID").value;
            string DEPTID = ViewPart.Field("DEPTID").value;
            string DEPTNAME = ViewPart.Field("DEPTNAME").value;
            string TOPDEPT = ViewPart.Field("TOPDEPT").value;

            DateTime tempDate;
            if (!DateTime.TryParse(DEPTBGN, out tempDate) || string.IsNullOrEmpty(DEPTBGN) || string.IsNullOrEmpty(SITEID) || string.IsNullOrEmpty(DEPTID) || string.IsNullOrEmpty(DEPTNAME) || string.IsNullOrEmpty(TOPDEPT))
            {
                rtnvlu = "msg_required";
            }
            else
            {
                string ExistAlready = SQLData.SQLFieldValue("select count(*) from HR_DEPT where DEPTID = N'" + DEPTID + "'");
                if (Convert.ToDouble(string.IsNullOrEmpty(ExistAlready) ? "0" : ExistAlready) > 0)
                {
                    rtnvlu = "msg_existalready";
                }
                if (TOPDEPT == DEPTID)
                {
                    rtnvlu = "msg_sameastopdept";
                }
            }
            return rtnvlu;
        }

        protected override string PutSaveData()
        {
            List<string> SQL = new List<string>();

            SQL.Add(" Insert into HR_DEPT(DEPTID,DEPTORDER,DEPTFLAG) " +
                    " values (@DEPTID, @DEPTORDER, @DEPTFLAG)" +
                    " declare @seq int " +
                    " exec dbo.XP_NextSeq N'HR_DEPTHS',N'DEPTNO',@seq out " +
                    " Insert into HR_DEPTHS( DEPTNO,DEPTID,SITEID,DEPTNAME,TOPDEPT,DEPTBGN,DEPTEnd,DEPTSTATUS,DEPTDESC,SYSDTE,SYSUSR) " +
                    " values ( @seq, @DEPTID, @SITEID, @DEPTNAME, @TOPDEPT, @DEPTBGN, @DEPTEnd, @DEPTSTATUS, @DEPTDESC, getdate(), @SYSUSR)" +
                    " UPDATE HR_DEPTHS SET DEPTEnd = dbo.FHR_DEPTEDT(DEPTNO) WHERE  DEPTID = @DEPTID  ");

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTNO", Value = Common.Val(ViewPart.Field("DEPTNO").value ?? "0"), SqlDbType = SqlDbType.BigInt });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTBGN", Value = DateTime.Parse(ViewPart.Field("DEPTBGN").value).ToString("yyyy-MM-dd"), SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTEnd", Value = "2999-12-31", SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEID", Value = ViewPart.Field("SITEID").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTID", Value = ViewPart.Field("DEPTID").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTNAME", Value = ViewPart.Field("DEPTNAME").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@TOPDEPT", Value = (ViewPart.Field("TOPDEPT").value ?? "").Replace("XXXXX", ""), SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTSTATUS", Value = ViewPart.Field("DEPTSTATUS").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTDESC", Value = ViewPart.Field("DEPTDESC").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTORDER", Value = Common.Val(ViewPart.Field("DEPTORDER").value ?? "0"), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTFLAG", Value = Common.Val(ViewPart.Field("DEPTFLAG").value ?? "0"), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSUSR", Value = AppKey.UserId, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }

        public ApiResponse ReloadTopDept()
        {
            string dte = GetDataValue("d");
            string top = GetDataValue("t");

            ApiResponse _ApiResponse = new ApiResponse();
            DateTime tempDate;
            if (DateTime.TryParse(dte, out tempDate))
            {
                string formattedDate = tempDate.ToString("yyyy-MM-dd");
                string Values = " SQL@ if exists(select * from VHR_DEPT where TOPDEPT = '')" +
                                " begin " +
                                "    select DEPTID, DEPTNAME from VHR_DEPTHS WHERE '" + formattedDate + "' BETWEEN DEPTBGN AND DEPTEND order by DEPTID " +
                                " End " +
                                " else " +
                                " begin " +
                                "    select 'XXXXX' DEPTID,'' DEPTNAME " +
                                "    union all " +
                                "    select DEPTID, DEPTNAME from VHR_DEPTHS WHERE '" + formattedDate + "' BETWEEN DEPTBGN AND DEPTEND order by DEPTID " +
                                " End ";

                OptionValues Opts = new OptionValues(Values);
                string OptHtml = Opts.OptionsHtml();

                _ApiResponse.SetElementContents("TOPDEPT", OptHtml);
            }

            return _ApiResponse;
        }
    }

}
