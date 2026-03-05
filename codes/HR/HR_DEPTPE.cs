using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_DEPTPE : WebBase
    {
        public override void InitialViewData()
        {
            string InitValue = "SQL@select SITEID, SITENAME from HR_SITE a order by SITEORDER ";
            string InitValue2 = "SQL@select SNO, SD01 + ' (' + SD02 + ')' from XysOption where CODE = 'DEPTSTATUS' order by convert(int, SNO) ";
            string InitValue3 = "{1|DEPTFLAG|*}";
            string InitValue1 = " SQL@ " +
                                " declare @dte nvarchar(10), @top nvarchar(16), @mydept nvarchar(16) " +
                                " set @dte = convert(varchar(10),getdate(),121) " +
                                " set @top = (select TOPDEPT from VHR_DEPTHS  where DEPTNO = " + PartialData + ") " +
                                " set @mydept = (select DEPTID from VHR_DEPTHS  where DEPTNO = " + PartialData + ") " +
                                " if exists(select * from VHR_DEPT where TOPDEPT = '')" +
                                " begin " +
                                "    select DEPTID, DEPTNAME, case when DEPTID = @top then 1 else 0 end TopDept  " +
                                "    from VHR_DEPTHS WHERE @dte BETWEEN DEPTBGN AND DEPTEND and DEPTID <> @mydept order by DEPTID " +
                                " End " +
                                " else " +
                                " begin " +
                                "    select 'XXXXX' DEPTID,'' DEPTNAME, case when 'XXXXX' = @top then 1 else 0 end TopDept " +
                                "    union all " +
                                "    select DEPTID, DEPTNAME, case when DEPTID = @top then 1 else 0 end TopDept " +
                                "    from VHR_DEPTHS WHERE @dte BETWEEN DEPTBGN AND DEPTEND and DEPTID <> @mydept order by DEPTID " +
                                " End ";

            ViewPart.UIControl = new UIControl { UIGroup = this.GetType().Name, UIMode = UIModes.Edit };
            ViewPart.UIControl.Set(new UIControl.Item[] {
                new UIControl.Item { Name = "DEPTNO", Label = "DEPTNO", IsReadOnly = true, IsVisible = false, IsKey = true, IsRequired = true, ValueType = ValueTypes.Defalt, LineSpacing = 1 },
                new UIControl.Item { Name = "DEPTID", Label = "DEPTID", Styles = "width:120px;", IsReadOnly = true, IsVisible = false, IsRequired = true, ValueType = ValueTypes.Defalt, LineSpacing = 1 },
                new UIControl.Item { Name = "DEPTBGN", Label = "DEPTBGN", Styles = "width:130px;", IsReadOnly = true, IsRequired = true, UIType = UITypes.Date, LineSpacing = 1 },
                new UIControl.Item { Name = "SITEID", Label = "SITEID", Styles = "width:380px;", IsRequired = true, UIType = UITypes.Dropdown, InitialValues = InitValue, LineSpacing = 1 },
                new UIControl.Item { Name = "DEPTNAME", Label = "DEPTNAME", Styles = "width:368px;", Attributes = "maxlength:100;", IsRequired = true, ValueType = ValueTypes.Defalt, LineSpacing = 1 },
                new UIControl.Item { Name = "TOPDEPT", Label = "TOPDEPT", Styles = "width:380px;", Attributes = "placeholder:Select...", UIType = UITypes.Dropdown, InitialValues = InitValue1, IsRequired = true, LineSpacing = 6 },
                new UIControl.Item { Name = "DEPTSTATUS", Label = "DEPTSTATUS", Styles = "width:380px;", UIType = UITypes.Dropdown, InitialValues = InitValue2, LineSpacing = 1 },
                new UIControl.Item { Name = "DEPTDESC", Label = "DEPTDESC", Styles = "width:368px;", Attributes = "maxlength:100;", LineSpacing = 1 },
                new UIControl.Item { Name = "DEPTORDER", Label = "DEPTORDER", Styles = "width:80px;padding-left:4px;", Attributes = "maxlength:16;", UIType = UITypes.Number, ValueType = ValueTypes.Defalt, InitialValues = "1" },
                new UIControl.Item { Name = "DEPTFLAG", Label = "DEPTFLAG", WrapStyles = "margin-left:20px;", UIType = UITypes.Checkbox, InitialValues = InitValue3, LineSpacing = 20 }
            });

            string SSQL = " Select DEPTNO,DEPTID,DEPTBGN,SITEID,DEPTNAME,TOPDEPT,DEPTSTATUS,DEPTDESC,DEPTORDER,DEPTFLAG from VHR_DEPTHS  where DEPTNO = " + PartialData;
            ViewPart.BindData(SSQL);

            ViewPart.Control("DEPTBGN").Value = DateTime.Parse(ViewPart.Control("DEPTBGN").Value).ToString(StdDateFormat);
        }

        public override string InitialViewHtml()
        {
            MenuList ViewMenuItems = GetViewMenuItems();
            Wrap ViewButtons = GetViewButtons();

            Label label = new Label();
            label.Wrap.SetStyles("font-weight:700;font-size:22px;margin:12px;");
            label.Wrap.InnerText = Translator.Format("edit");

            HtmlElementBox elmBox = new HtmlElementBox();
            elmBox.DefaultStyle = false;
            elmBox.SetAttribute(HtmlAttributes.id, References.Elements.ElmBox);
            elmBox.SetStyle(HtmlStyles.margin, "auto");
            elmBox.SetStyle(HtmlStyles.margin, "20px");
            elmBox.SetStyle(HtmlStyles.boxSizing, "border-box");

            elmBox.AddItem(label, 10);
            elmBox.AddItem(ViewPart.UIControl, 10);
            elmBox.AddItem(ViewButtons, 10);

            return elmBox.HtmlText;
        }

        public ApiResponse Cancel()
        {
            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.PopOff();
            return _ApiResponse;
        }

        public ApiResponse SaveData()
        {
            ApiResponse _ApiResponse = new ApiResponse();

            string rlt = VerifySave();
            if (rlt != string.Empty)
            {
                _ApiResponse.PopUpWindow(DialogMsgRequred(rlt), References.Elements.PageContents);
            }
            else
            {
                rlt = PutSaveData();
                if (string.IsNullOrEmpty(rlt))
                {
                    _ApiResponse.ExecuteScript("DEPTDetail('HR_DEPTMV/DEPTDetail','" + ViewPart.Field("DEPTID").value + "')");
                    _ApiResponse.ExecuteScript("DEPTDetail('HR_DEPTMV/DEPTDetail','" + ViewPart.Field("DEPTID").value + "')");
                    _ApiResponse.PopOff();
                }
                else
                {
                    _ApiResponse.PopUpWindow(DialogMsg(rlt), References.Elements.PageContents);
                }
            }

            return _ApiResponse;
        }

        protected string VerifySave()
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
                if (Common.Val(ViewPart.Field("DEPTFLAG").value) == 0)
                {
                    string LowerDEPT = SQLData.SQLFieldValue("select count(*) from VHR_DEPT where DEPTFLAG = 1 and TOPDEPT = N'" + DEPTID + "'");
                    if (Common.Val(LowerDEPT) > 0)
                    {
                        rtnvlu = "msg_lowerdept";
                    }
                    string ExistInOrg = SQLData.SQLFieldValue("select count(*) from HR_Org where OGFlag = 1 and DEPTID = N'" + DEPTID + "'");
                    if (Common.Val(ExistInOrg) > 0)
                    {
                        rtnvlu = "msg_existorg";
                    }
                }
                else
                {
                    string TopExist = SQLData.SQLFieldValue("select count(*) from VHR_DEPT where DEPTFLAG = 1 and DEPTID = N'" + TOPDEPT + "'");
                    if (Common.Val(TopExist) == 0)
                    {
                        rtnvlu = "msg_noTOPDEPT";
                    }
                    if (TOPDEPT == DEPTID)
                    {
                        rtnvlu = "msg_sameastopdept";
                    }
                }
            }

            return rtnvlu;
        }

        private string PutSaveData()
        {
            List<string> SQL = new List<string>();

            SQL.Add(" if not exists(select * from VHR_DEPTHS where DEPTID = @DEPTID and DEPTBGN > @DEPTBGN) " +
                    " begin " +
                    "   Update HR_DEPT set DEPTORDER = @DEPTORDER, DEPTFLAG = @DEPTFLAG where DEPTID = @DEPTID " +
                    " end " +
                    " " +
                    " Update HR_DEPTHS set " +
                    " SITEID = @SITEID, " +
                    " DEPTNAME = @DEPTNAME, " +
                    " TOPDEPT = @TOPDEPT, " +
                    " DEPTBGN = @DEPTBGN, " +
                    " DEPTEnd = @DEPTEnd, " +
                    " DEPTSTATUS = @DEPTSTATUS, " +
                    " DEPTDESC = @DEPTDESC, " +
                    " SYSDTE = getdate(), " +
                    " SYSUSR = @SYSUSR " +
                    " Where DEPTNO = @DEPTNO " +
                    " UPDATE HR_DEPTHS SET DEPTEnd = dbo.FHR_DEPTEDT(DEPTNO) WHERE  DEPTID = @DEPTID  ");

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTNO", Value = Common.Val(ViewPart.Field("DEPTNO").value ?? "0"), SqlDbType = SqlDbType.BigInt });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTBGN", Value = DateTime.Parse(ViewPart.Field("DEPTBGN").value).ToString(StdDateFormat), SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTEnd", Value = "2999-12-31", SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@SITEID", Value = ViewPart.Field("SITEID").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTID", Value = ViewPart.Field("DEPTID").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTNAME", Value = ViewPart.Field("DEPTNAME").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@TOPDEPT", Value = ViewPart.Field("TOPDEPT").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTSTATUS", Value = ViewPart.Field("DEPTSTATUS").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTDESC", Value = ViewPart.Field("DEPTDESC").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTORDER", Value = ViewPart.Field("DEPTORDER").value, SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTFLAG", Value = Common.Val(ViewPart.Field("DEPTFLAG").value ?? "0").ToString(), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@SYSUSR", Value = AppKey.UserId, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }

        public ApiResponse ReloadTopDept()
        {
            string DEPTID = ViewPart.Field("DEPTID").value;
            string dte = GetDataValue("d");
            string top = GetDataValue("t");
            string Values = " SQL@ " +
                            " declare @dte nvarchar(10), @top nvarchar(16), @mydept nvarchar(16) " +
                            " set @dte = '" + DateTime.Parse(dte).ToString("yyyy-MM-dd") + "' " +
                            " set @top = '" + top + "' " +
                            " set @mydept = '" + DEPTID + "' " +
                            " if exists(select * from VHR_DEPT where TOPDEPT = '')" +
                            " begin " +
                            "    select DEPTID, DEPTNAME, case when DEPTID = @top then 1 else 0 end TopDept  " +
                            "    from VHR_DEPTHS WHERE @dte BETWEEN DEPTBGN AND DEPTEND and DEPTID <> @mydept order by DEPTID " +
                            " End " +
                            " else " +
                            " begin " +
                            "    select 'XXXXX' DEPTID,'' DEPTNAME, case when 'XXXXX' = @top then 1 else 0 end TopDept " +
                            "    union all " +
                            "    select DEPTID, DEPTNAME, case when DEPTID = @top then 1 else 0 end TopDept " +
                            "    from VHR_DEPTHS WHERE @dte BETWEEN DEPTBGN AND DEPTEND and DEPTID <> @mydept order by DEPTID " +
                            " End ";

            OptionValues Opts = new OptionValues(Values);
            string OptHtml = Opts.OptionsHtml();
            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.SetElementContents("TOPDEPT", OptHtml);
            return _ApiResponse;
        }
    }

}
