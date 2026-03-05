using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{
    public class HR_ORGEV : WebGridEV
    {
        public HR_ORGEV()
        {
            MyPageType = "HR_ORG";
        }

        public override void InitialViewData()
        {
            string MyDataName = "HR_ORG";

            UIModes UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit;
            ViewPart.UIControl = UIControlFromXDataElements(MyDataName, XDataElementsFromDataName(MyDataName), UIMode);

            string SSQL = " Select OGNO,OGNM,DEPTID,OGA,OGB,OGC,OGG,OGH,OGBGT,OGTO,OGLEAD,OGFLAG from HR_ORG  where OGNO = " + (string.IsNullOrEmpty(PartialData) ? "0" : PartialData) + " ";
            ViewPart.BindData(SSQL);
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string DEPTID = ViewPart.Field("DEPTID").value;
            string OGA = ViewPart.Field("OGA").value;
            string OGB = ViewPart.Field("OGB").value;
            string OGC = ViewPart.Field("OGC").value;
            string OGG = ViewPart.Field("OGG").value;
            string OGH = ViewPart.Field("OGH").value;

            if (string.IsNullOrEmpty(DEPTID) ||
               string.IsNullOrEmpty(OGA) ||
               string.IsNullOrEmpty(OGB) ||
               string.IsNullOrEmpty(OGC) ||
               string.IsNullOrEmpty(OGG) ||
               string.IsNullOrEmpty(OGH))
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
                        " exec dbo.XP_NextSeq N'HR_ORG',N'OGNO',@seq out " +
                        " Insert into HR_ORG( OGNO,OGNM,DEPTID,OGA,OGB,OGC,OGG,OGH,OGBGT,OGTO,OGLEAD,OGFLAG,SYSDTE,SYSUSR) " +
                        " values ( @seq, dbo.FHR_OGNM(@DEPTID,@OGA), @DEPTID, @OGA, @OGB, @OGC, @OGG, @OGH, @OGBGT, @OGTO, @OGLEAD,@OGFLAG,getdate(), @SYSUSR) ");
            }
            else
            {
                SQL.Add(" Update HR_ORG set " +
                        " OGNM = dbo.FHR_OGNM(@DEPTID,@OGA), " +
                        " DEPTID = @DEPTID, " +
                        " OGA = @OGA, " +
                        " OGB = @OGB, " +
                        " OGC = @OGC, " +
                        " OGG = @OGG, " +
                        " OGH = @OGH, " +
                        " OGBGT = @OGBGT, " +
                        " OGTO = @OGTO, " +
                        " OGLEAD = @OGLEAD, " +
                        " OGFLAG = @OGFLAG, " +
                        " SYSDTE = getdate(), SYSUSR = @SYSUSR " +
                        " WHERE  OGNO = @OGNO ");
            }

            if (Common.Val(ViewPart.Field("OGFLAG").value ?? "0") == 0)
            {
                ViewPart.Field("OGLEAD").value = "0";
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@OGNO", Value = Common.Val(ViewPart.Field("OGNO").value ?? "0"), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGNM", Value = ViewPart.Field("OGNM").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTID", Value = ViewPart.Field("DEPTID").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGA", Value = ViewPart.Field("OGA").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGB", Value = ViewPart.Field("OGB").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGC", Value = ViewPart.Field("OGC").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGG", Value = ViewPart.Field("OGG").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGH", Value = ViewPart.Field("OGH").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGBGT", Value = Common.Val(ViewPart.Field("OGBGT").value ?? "0"), SqlDbType = SqlDbType.Decimal });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGTO", Value = Common.Val(ViewPart.Field("OGTO").value ?? "0"), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGLEAD", Value = Common.Val(ViewPart.Field("OGLEAD").value ?? "0"), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@OGFLAG", Value = Common.Val(ViewPart.Field("OGFLAG").value ?? "0"), SqlDbType = SqlDbType.Int });
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
            " delete from HR_ORG WHERE OGNO = @OGNO "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@OGNO", Value = ViewPart.Field("OGNO").value, SqlDbType = SqlDbType.Int });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }
}
