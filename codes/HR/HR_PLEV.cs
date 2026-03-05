using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{
    public class HR_PLEV : WebGridEV
    {
        public HR_PLEV()
        {
            MyPageType = "HR_PL";
        }

        public override void InitialViewData()
        {
            string MyDataName = "HR_PL";

            UIModes UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit;
            ViewPart.UIControl = UIControlFromXDataElements(MyDataName, XDataElementsFromDataName(MyDataName), UIMode);

            UIGrid UIGrid = UIGridFromXDataElements(XDataElementsFromDataName(MyDataName));
            string SqlColumns = string.Join(",", UIGrid.Columns().ToArray());
            SqlColumns = SqlColumns.Replace("EMPNM", "dbo.FHR_EMPNAMEF(EMPID) + ' [' + EMPID + ']' EMPNM");

            string SSQL = " Select " + SqlColumns + " from HR_PL  where PLNO = " + (string.IsNullOrEmpty(PartialData) ? "0" : PartialData) + " ";

            ViewPart.BindData(SSQL);

            ViewPart.Control("EMPNM").Attributes += "onchange:EMPSelected(this,'EMPID');";
            if (UIMode == UIModes.Edit)
            {
                ViewPart.Control("EMPNM").IsReadOnly = true;
                ViewPart.Control("EMPID").IsReadOnly = true;
            }
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string EMPID = ViewPart.Field("EMPID").value;
            string PLCD = ViewPart.Field("PLCD").value;
            string PLBDT = ViewPart.Field("PLBDT").value;
            string PLWTP = ViewPart.Field("PLWTP").value;
            string OGNO = ViewPart.Field("OGNO").value;

            DateTime tempDate;
            if (string.IsNullOrEmpty(EMPID) ||
               string.IsNullOrEmpty(PLCD) ||
               string.IsNullOrEmpty(PLBDT) ||
               !DateTime.TryParse(PLBDT, out tempDate) ||
               string.IsNullOrEmpty(PLWTP) ||
               Convert.ToDouble(string.IsNullOrEmpty(OGNO) ? "0" : OGNO) == 0)
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
                        " exec dbo.XP_NextSeq N'HR_PL',N'PLNO',@seq out " +
                        " Insert into HR_PL( PLNO,EMPID,PLCD,PLDESC,PLBDT,PLEDT,PLWTP,PLWRAT,OGNO,SYSDTE,SYSUSR) " +
                        " values ( @seq, @EMPID, @PLCD, @PLDESC, @PLBDT, @PLEDT, @PLWTP, @PLWRAT, @OGNO, getdate(), @SYSUSR) " +
                        " UPDATE HR_PL SET PLEDT = dbo.FHR_PLEDT(PLNO) WHERE  EMPID = @EMPID  ");
            }
            else
            {
                SQL.Add(" Update HR_PL set " +
                        " EMPID = @EMPID, " +
                        " PLCD = @PLCD, " +
                        " PLDESC = @PLDESC, " +
                        " PLBDT = @PLBDT, " +
                        " PLEDT = @PLEDT, " +
                        " PLWTP = @PLWTP, " +
                        " PLWRAT = @PLWRAT, " +
                        " OGNO = @OGNO, " +
                        " SYSDTE = getdate(), SYSUSR = @SYSUSR " +
                        " WHERE  PLNO = @PLNO " +
                        " UPDATE HR_PL SET PLEDT = dbo.FHR_PLEDT(PLNO) WHERE  EMPID = @EMPID  ");
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@PLNO", Value = ViewPart.Field("PLNO").value, SqlDbType = SqlDbType.BigInt });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPID", Value = ViewPart.Field("EMPID").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@PLCD", Value = ViewPart.Field("PLCD").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@PLDESC", Value = ViewPart.Field("PLDESC").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@PLBDT", Value = DateTime.Parse(ViewPart.Field("PLBDT").value).ToString(StdDateFormat), SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@PLEDT", Value = "2999-12-31", SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@PLWTP", Value = ViewPart.Field("PLWTP").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@PLWRAT", Value = Convert.ToDouble(string.IsNullOrEmpty(ViewPart.Field("PLWRAT").value) ? "0" : ViewPart.Field("PLWRAT").value), SqlDbType = SqlDbType.Float });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGNO", Value = Common.Val(ViewPart.Field("OGNO").value), SqlDbType = SqlDbType.BigInt });
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
            " delete from HR_PL WHERE PLNO = @PLNO "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@PLNO", Value = ViewPart.Field("PLNO").value, SqlDbType = SqlDbType.BigInt });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }
}
