using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_EMPEV : WebGridEV
    {
        public HR_EMPEV()
        {
            MyPageType = "HR_EMP";
        }

        public override void InitialViewData()
        {
            string MyDataName = "HR_EMP";

            UIModes UIMode = string.IsNullOrEmpty(PartialData) ? UIModes.New : UIModes.Edit;
            ViewPart.UIControl = UIControlFromXDataElements(MyDataName, XDataElementsFromDataName(MyDataName), UIMode);

            UIGrid UIGrid = UIGridFromXDataElements(XDataElementsFromDataName(MyDataName));
            string SSQL = " Select " + string.Join(",", UIGrid.Columns().ToArray()) + " from HR_EMP  where EMPID = N'" + PartialData + "'";

            ViewPart.BindData(SSQL);

            if (!string.IsNullOrEmpty(ViewPart.Control("EMPDOB").Value))
            {
                ViewPart.Control("EMPDOB").Value = DateTime.Parse(ViewPart.Control("EMPDOB").Value).ToString(StdDateFormat);
            }
        }

        protected override string VerifySave()
        {
            string rtnvlu = string.Empty;

            string EMPID = ViewPart.Field("EMPID").value;
            string EMPNML = ViewPart.Field("EMPNML").value;
            string EMPNMF = ViewPart.Field("EMPNMF").value;
            string EMPDOB = ViewPart.Field("EMPDOB").value;

            if (string.IsNullOrEmpty(EMPID) ||
                string.IsNullOrEmpty(EMPNML) ||
                string.IsNullOrEmpty(EMPNMF) ||
                string.IsNullOrEmpty(EMPDOB))
            {
                rtnvlu = "msg_required";
            }
            else
            {
                DateTime tempDate;
                if (!DateTime.TryParse(EMPDOB, out tempDate))
                {
                    rtnvlu = "msg_required";
                }
            }
            return rtnvlu;
        }

        protected override string PutSaveData()
        {
            List<string> SQL = new List<string>();

            if (ViewPart.Data == null)
            {
                SQL.Add(" Insert into HR_EMP( EMPID,EMPNML,EMPNMF,EMPNMM,EMPDOB,EMPSSN,EMPSEX,EMPMAR,EMPAGE,EMPHOH,EMPBDT,EMPEDT,EMPRSN,EMPEMAIL,EMPTEL,EMPCELL,EMPZIP,EMPADR1,EMPADR2,EMPCITY,EMPSTATE,EMPNAT,EMPOGNO,EMPFLAG,SYSDTE,SYSUSR) " +
                        " values ( @EMPID, @EMPNML, @EMPNMF, @EMPNMM, @EMPDOB, @EMPSSN, @EMPSEX, @EMPMAR, @EMPAGE, @EMPHOH, @EMPBDT, @EMPEDT, @EMPRSN, @EMPEMAIL, @EMPTEL, @EMPCELL, @EMPZIP, @EMPADR1, @EMPADR2, @EMPCITY, @EMPSTATE, @EMPNAT, @EMPOGNO, @EMPFLAG, getdate(), @SYSUSR) ");
            }
            else
            {
                SQL.Add(" Update HR_EMP set " +
                        " EMPNML = @EMPNML, " +
                        " EMPNMF = @EMPNMF, " +
                        " EMPNMM = @EMPNMM, " +
                        " EMPDOB = @EMPDOB, " +
                        " EMPSSN = @EMPSSN, " +
                        " EMPSEX = @EMPSEX, " +
                        " EMPMAR = @EMPMAR, " +
                        " EMPAGE = @EMPAGE, " +
                        " EMPHOH = @EMPHOH, " +
                        " EMPBDT = @EMPBDT, " +
                        " EMPEDT = @EMPEDT, " +
                        " EMPRSN = @EMPRSN, " +
                        " EMPEMAIL = @EMPEMAIL, " +
                        " EMPTEL = @EMPTEL, " +
                        " EMPCELL = @EMPCELL, " +
                        " EMPZIP = @EMPZIP, " +
                        " EMPADR1 = @EMPADR1, " +
                        " EMPADR2 = @EMPADR2, " +
                        " EMPCITY = @EMPCITY, " +
                        " EMPSTATE = @EMPSTATE, " +
                        " EMPNAT = @EMPNAT, " +
                        " EMPOGNO = @EMPOGNO, " +
                        " EMPFLAG = @EMPFLAG, " +
                        " SYSDTE = getdate(), SYSUSR = @SYSUSR " +
                        " WHERE  EMPID = @EMPID ");
            }

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPID", Value = ViewPart.Field("EMPID").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPNML", Value = ViewPart.Field("EMPNML").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPNMF", Value = ViewPart.Field("EMPNMF").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPNMM", Value = ViewPart.Field("EMPNMM").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPDOB", Value = DateTime.Parse(ViewPart.Field("EMPDOB").value).ToString("yyyy-MM-dd"), SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPSSN", Value = ViewPart.Field("EMPSSN").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPSEX", Value = ViewPart.Field("EMPSEX").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPMAR", Value = ViewPart.Field("EMPMAR").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPAGE", Value = ViewPart.Field("EMPAGE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPHOH", Value = ViewPart.Field("EMPHOH").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPBDT", Value = ViewPart.Field("EMPBDT").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPEDT", Value = ViewPart.Field("EMPEDT").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPRSN", Value = ViewPart.Field("EMPRSN").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPEMAIL", Value = ViewPart.Field("EMPEMAIL").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPTEL", Value = ViewPart.Field("EMPTEL").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPCELL", Value = ViewPart.Field("EMPCELL").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPZIP", Value = ViewPart.Field("EMPZIP").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPADR1", Value = ViewPart.Field("EMPADR1").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPADR2", Value = ViewPart.Field("EMPADR2").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPCITY", Value = ViewPart.Field("EMPCITY").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPSTATE", Value = ViewPart.Field("EMPSTATE").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPNAT", Value = ViewPart.Field("EMPNAT").value, SqlDbType = SqlDbType.NVarChar });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPOGNO", Value = Common.Val(ViewPart.Field("EMPOGNO").value), SqlDbType = SqlDbType.Int });
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPFLAG", Value = ViewPart.Field("EMPFLAG").value, SqlDbType = SqlDbType.NVarChar });
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
            " delete from HR_EMP WHERE EMPID = @EMPID  "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@EMPID", Value = ViewPart.Field("EMPID").value, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }

}
