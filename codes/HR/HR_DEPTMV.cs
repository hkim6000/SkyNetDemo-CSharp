using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_DEPTMV : WebGridMV
    {
        public HR_DEPTMV()
        {
            SQLGridPage = this.GetType().Name;
            SQLGridFilter = " a.DEPTFLAG = 1 and convert(nvarchar(10),getdate(),121) between b.DEPTBGN and b.DEPTEnd  and a.DEPTID + b.DEPTNAME + dbo.FHR_SITENAME(b.SITEID) ";
            SQLGridOrderBy = new string[] { "a.DEPTORDER", "b.DEPTNAME", "a.DEPTID" };

            string GridName = Translator.Format("title");
            string GridTables = "HR_DEPT a inner join HR_DEPTHS b on a.DEPTID = b.DEPTID ";

            UIGrid UIGrid = new UIGrid();
            UIGrid.Items.AddRange(new UIGrid.Item[] {
                new UIGrid.Item { Name = "Edit", Value = "'&#10148;'", Label = string.Empty },
                new UIGrid.Item { Name = "DEPTNO", Value = "b.DEPTNO", Label = "DEPTNO", IsKey = true },
                new UIGrid.Item { Name = "DEPTID", Value = "a.DEPTID", Label = "DEPTID", IsKey = true },
                new UIGrid.Item { Name = "DEPTNAME", Value = "b.DEPTNAME", Label = "DEPTNAME" },
                new UIGrid.Item { Name = "SITENAME", Value = "dbo.FHR_SITENAME(b.SITEID)", Label = "SITENAME" },
                new UIGrid.Item { Name = "TOPDEPT", Value = "b.TOPDEPT", Label = "TOPDEPT" },
                new UIGrid.Item { Name = "DEPTBGN", Value = "b.DEPTBGN", Label = "DEPTBGN" },
                new UIGrid.Item { Name = "DEPTORDER", Value = "a.DEPTORDER", Label = "DEPTORDER" },
                new UIGrid.Item { Name = "DEPTFLAG", Value = "a.DEPTFLAG", Label = "DEPTFLAG" },
                new UIGrid.Item { Name = "SYSDTE", Label = "SYSDTE" },
                new UIGrid.Item { Name = "SYSUSR", Value = "dbo.XF_UserName(SYSUSR)", Label = "SYSUSR" }
            });

            SQLGridInfo.Name = GridName;
            SQLGridInfo.Id = this.GetType().Name;

            string pageNoStr = ParamValue(SQLGridInfo.Id + "_PageNo");
            double pageNoVal = Convert.ToDouble(string.IsNullOrEmpty(pageNoStr) ? "0" : pageNoStr);
            SQLGridInfo.CurrentPageNo = pageNoVal == 0 ? 1 : (int)pageNoVal;

            SQLGridInfo.LinesPerPage = 30;
            SQLGridInfo.ExcludeDownloadColumns = new int[] { 0, 1 };
            SQLGridInfo.DisplayCount = SQLGridSection.DisplayCounts.FilteredOnly;
            SQLGridInfo.TitleEnabled = true;
            SQLGridInfo.Query = new SQLGridSection.SQLQuery
            {
                Tables = GridTables,
                OrderBy = SQLGridOrderBy,
                Columns = UIGrid.Columns().ToArray(),
                ColumnAlias = UIGrid.Labels().ToArray(),
                Filters = string.IsNullOrEmpty(ParamValue(SQLGridInfo.Id + "_Filter"))
                    ? SQLGridFilter + " like '%%' "
                    : ParamValue(SQLGridInfo.Id + "_Filter")
            };
        }

        public override string InitialViewHtml()
        {
            MenuList ViewMenuItems = GetViewMenuItems();
            Wrap ViewButtons = GetViewButtons(new string[] { });

            Texts filterText = new Texts(TextTypes.text);
            filterText.Wrap.SetStyle(HtmlStyles.margin, "2px");
            filterText.Wrap.SetStyle(HtmlStyles.marginTop, "4px");
            filterText.Wrap.SetStyle(HtmlStyles.marginLeft, "8px");
            filterText.Text.SetStyle(HtmlStyles.fontSize, "16px");
            filterText.Text.SetStyle(HtmlStyles.height, "24px");
            filterText.Text.SetAttribute(HtmlAttributes.placeholder, Translator.Format("searchterm"));
            filterText.Text.SetAttribute(HtmlAttributes.id, "FilterBox");
            filterText.Text.SetAttribute(HtmlAttributes.value, ParamValue("FilterBoxValue"));

            CheckBox filterCheck = new CheckBox(Translator.Format("allstatus"));
            bool isChecked = Convert.ToDouble(string.IsNullOrEmpty(ParamValue("FilterPre")) ? "0" : ParamValue("FilterPre")) == 1;
            filterCheck.Checks.AddItem("filterCheck", "1", isChecked);
            filterCheck.Alignment = Alignments.Horizontal;
            filterCheck.LabelWrap.SetStyle(HtmlStyles.marginRight, "8px");
            filterCheck.Wrap.SetStyle(HtmlStyles.marginRight, "8px");

            Button filterBtn = new Button();
            filterBtn.SetStyle(HtmlStyles.backgroundImage, "url('" + ImagePath + "search.jpg')");
            filterBtn.SetStyle(HtmlStyles.backgroundRepeat, "no-repeat");
            filterBtn.SetStyle(HtmlStyles.backgroundSize, "24px 24px");
            filterBtn.SetStyle(HtmlStyles.borderRadius, "50%");
            filterBtn.SetStyle(HtmlStyles.border, "1px solid #ddd");
            filterBtn.SetStyle(HtmlStyles.padding, "6px");
            filterBtn.SetStyle(HtmlStyles.height, "30px");
            filterBtn.SetStyle(HtmlStyles.width, "30px");
            filterBtn.SetStyle(HtmlStyles.boxShadow, "1px 2px 2px 1px rgba(0, 0, 0, 0.15)");
            filterBtn.SetAttribute(HtmlEvents.onclick, ByPassCall(SQLGridPage + "/SearchClicked"));

            FilterSection filter = new();
            filter.ModalWrap = true;
            filter.Wrap.SetStyle(HtmlStyles.marginTop, "110px");
            filter.Wrap.SetStyle(HtmlStyles.width, "90%");
            filter.Menu = ViewMenuItems;
            filter.FilterHtml = filterCheck.HtmlText + filterText.HtmlText + filterBtn.HtmlText;

            SQLGridSection SQLGrid = new SQLGridSection(SQLGridInfo);
            if (SQLGrid.Grid != null) SQLGrid.Grid.Table.SetAttribute(HtmlAttributes.@class, "tableX");
            SetGridStyle(SQLGrid);

            HtmlElementBox elmBox = new HtmlElementBox();
            elmBox.SetAttribute(HtmlAttributes.id, References.Elements.ElmBox);
            elmBox.SetStyle(HtmlStyles.width, "90%");
            elmBox.SetStyle(HtmlStyles.margin, "auto");
            elmBox.SetStyle(HtmlStyles.marginTop, "8px");
            elmBox.SetStyle(HtmlStyles.marginBottom, "80px");
            elmBox.SetStyle(HtmlStyles.overflow, "auto");
            elmBox.Wrap.SetStyle(HtmlStyles.margin, "20px 10px 20px 10px");

            elmBox.AddItem(SQLGrid);

            return filter.HtmlText + elmBox.HtmlText;
        }

        protected override void SetGridStyle(SQLGridSection SQLGrid)
        {
            SQLGrid.Wrap.SetStyle(HtmlStyles.margin, string.Empty);
            SQLGrid.Wrap.SetStyle(HtmlStyles.marginLeft, "8px");
            SQLGrid.Wrap.SetStyle(HtmlStyles.display, "inline-block");
            SQLGrid.Wrap.SetStyle(HtmlStyles.width, "100%");

            if (SQLGrid.GridData != null)
            {
                SQLGrid.Grid.TableColumns[1].SetHeaderStyle(HtmlStyles.display, "none");
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.display, "none");

                for (int i = 2; i <= 8; i++)
                {
                    SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                }

                SQLGrid.Grid.TableColumns[8].SetColumnFormat("@R {8} | 0. , 1.✓");
                SQLGrid.Grid.TableColumns[6].SetColumnFormat("@D {6} |" + LocalDateFormat);
                SQLGrid.Grid.TableColumns[9].SetColumnFormat("@D {9} |" + LocalDateFormat);

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.cursor, "pointer");
                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[0].SetColumnAttribute(HtmlEvents.onclick, "DEPTDetail('HR_DEPTMV/DEPTDetail', '{2}')");
                SQLGrid.Grid.TableColumns[0].SetColumnAttribute(HtmlAttributes.id, "R_{2}");

                for (int i = 0; i < SQLGrid.Grid.TableColumns.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "center");
                            break;
                        default:
                            SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "left");
                            break;
                    }
                }
            }
        }

        public override ApiResponse SearchClicked()
        {
            string FilterBoxValue = ParamValue("FilterBox");
            string filterCheckVal = ParamValue("filterCheck");

            SQLGridFilter = "  convert(nvarchar(10),getdate(),121) between b.DEPTBGN and b.DEPTEnd  and a.DEPTID + b.DEPTNAME + dbo.FHR_SITENAME(b.SITEID) ";

            double chkVal = Convert.ToDouble(string.IsNullOrEmpty(filterCheckVal) ? "0" : filterCheckVal);
            if (chkVal != 1)
            {
                SQLGridFilter = " a.DEPTFLAG = 1 and " + SQLGridFilter;
            }

            SQLGridInfo.Query.Filters = SQLGridFilter + "  like N'%" + FilterBoxValue + "%' ";
            SQLGridSection SQLGrid = new SQLGridSection(SQLGridInfo);
            if (SQLGrid.Grid != null) SQLGrid.Grid.Table.SetAttribute(HtmlAttributes.@class, "tableX");
            SetGridStyle(SQLGrid);

            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.ReplaceSQLGridSection(SQLGridInfo.Id, SQLGrid);
            _ApiResponse.StoreLocalValue("FilterBoxValue", FilterBoxValue);
            _ApiResponse.StoreLocalValue("FilterPre", chkVal.ToString());
            return _ApiResponse;
        }

        public ApiResponse PopEditRecord()
        {
            string t = GetDataValue("t");
            HtmlDocument pophtml = PartialDocument("HR_DEPTPE", t);
            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.PopUpWindow(pophtml, References.Elements.PageContents);
            return _ApiResponse;
        }

        public ApiResponse PopAddRecord()
        {
            string t = GetDataValue("t");
            HtmlDocument pophtml = PartialDocument("HR_DEPTPA", t);
            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.PopUpWindow(pophtml, References.Elements.PageContents);
            return _ApiResponse;
        }

        public ApiResponse DEPTDetail()
        {
            string t = GetDataValue("t");
            string rid = "R_" + t;

            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.TableToggleRow(rid, DEPTDetailGrid(t));
            _ApiResponse.ExecuteScript("RefreshTableRow('" + rid + "','" + DEPTRefresh(t) + "')");

            return _ApiResponse;
        }

        private string DEPTRefresh(string deptid)
        {
            string ssql = " select top 1 '',DEPTNO, DEPTID,DEPTNAME,dbo.FHR_SITENAME(SITEID),TOPDEPT,DEPTBGN,DEPTORDER,convert(varchar(18),DEPTFLAG)" +
                          " from VHR_DEPT where DEPTid = N'" + deptid + "' order by DEPTBGN desc ";
            DataTable dt = SQLData.SQLDataTable(ssql);

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Rows[0][6] = WebCore.FormattedValue("@D " + dt.Rows[0][6].ToString() + " |" + LocalDateFormat);
                dt.Rows[0][8] = WebCore.FormattedValue("@R " + dt.Rows[0][8].ToString() + " | 0. , 1.✓");
                return string.Join("|", dt.Rows[0].ItemArray);
            }
            return string.Empty;
        }

        private string DEPTDetailGrid(string deptid)
        {
            string ssql = " select '&#10149;' [E],DEPTNO,DEPTID,DEPTBGN,dbo.FHR_SITENAME(SITEID) SITENAME,DEPTNAME,TOPDEPT, " +
                          "        dbo.FHR_DEPTSTATUSName(DEPTSTATUS) DEPTSTATUS,DEPTDESC,'&#10008;' as [D] " +
                          " from VHR_DEPThs where DEPTid = N'" + deptid + "' order by DEPTBGN desc";
            DataTable dt = SQLData.SQLDataTable(ssql);

            if (dt != null)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ColumnName = Translator.Format(dt.Columns[i].ColumnName);
                }
            }

            DataGrid Grid = new DataGrid(dt);
            Grid.Table.SetAttribute(HtmlAttributes.@class, "tableQ");
            GridStyle(Grid);

            Button btn = new Button();
            btn.SetAttribute(HtmlAttributes.value, Translator.Format("addrecord"));
            btn.SetStyles("border-radius:4px; border:1px solid #aaa; background-color:#e8e8e8; color:#222; padding:4px; padding-left:6px; padding-right:6px; display:inline-block; font-size:12px;");
            btn.SetAttribute(HtmlEvents.onclick, "PopAddRecord('HR_DEPTMV/PopAddRecord','" + deptid + "')");

            Button btn1 = new Button();
            btn1.SetAttribute(HtmlAttributes.value, Translator.Format("removeall"));
            btn1.SetStyles("border-radius:4px; border:1px solid #aaa; background-color:#e8e8e8; color:#ff6600; padding:4px;padding-left:6px; padding-right:6px; display:inline-block; font-size:12px;");
            btn1.SetAttribute(HtmlEvents.onclick, "$Call('PartialView', 'm=HR_DEPTEV&t=" + deptid + "')");

            Wrap WrapObj = new Wrap();
            WrapObj.InnerText = btn.HtmlText + btn1.HtmlText + Grid.HtmlText;
            WrapObj.SetStyle(HtmlStyles.marginLeft, "24px");
            WrapObj.SetStyle(HtmlStyles.marginBottom, "20px");
            WrapObj.SetStyle(HtmlStyles.overflowX, "auto");
            WrapObj.SetStyle(HtmlStyles.border, "1px solid #666");
            WrapObj.SetStyle(HtmlStyles.textAlign, "left");
            return WrapObj.HtmlText;
        }

        private void GridStyle(DataGrid DTGrid)
        {
            DTGrid.TableColumns[0].Alias = " ";
            DTGrid.TableColumns[9].Alias = " ";

            DTGrid.TableColumns[0].SetColumnAttribute(HtmlAttributes.title, Translator.Format("edit"));
            DTGrid.TableColumns[0].SetColumnStyle(HtmlStyles.cursor, "pointer");
            DTGrid.TableColumns[0].SetColumnAttribute(HtmlEvents.onclick, "PopEditRecord('HR_DEPTMV/PopEditRecord','{1}')");

            DTGrid.TableColumns[9].SetColumnAttribute(HtmlAttributes.title, Translator.Format("delete"));
            DTGrid.TableColumns[9].SetColumnStyle(HtmlStyles.cursor, "pointer");
            DTGrid.TableColumns[9].SetColumnAttribute(HtmlEvents.onclick, "$Call('HR_DEPTMV/DeleteData','p={1}&q={2}')");

            DTGrid.TableColumns[1].SetHeaderStyle(HtmlStyles.display, "none");
            DTGrid.TableColumns[1].SetColumnStyle(HtmlStyles.display, "none");
            DTGrid.TableColumns[2].SetHeaderStyle(HtmlStyles.display, "none");
            DTGrid.TableColumns[2].SetColumnStyle(HtmlStyles.display, "none");

            DTGrid.TableColumns[3].SetColumnFormat("@D {3} |" + LocalDateFormat);

            for (int i = 0; i < DTGrid.TableColumns.Count; i++)
            {
                switch (i)
                {
                    case 0:
                    case 3:
                    case 6:
                    case 9:
                        DTGrid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "center");
                        break;
                    default:
                        DTGrid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "left");
                        break;
                }
            }
        }

        public ApiResponse DeleteData()
        {
            string p = GetDataValue("p");
            string q = GetDataValue("q");

            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.PopUpWindow(DialogQstDelete(this.GetType().Name + "/ConfirmDeleteData", "p=" + p + "&q=" + q), References.Elements.PageContents);
            return _ApiResponse;
        }

        public ApiResponse ConfirmDeleteData()
        {
            string p = GetDataValue("p");
            string q = GetDataValue("q");

            ApiResponse _ApiResponse = new ApiResponse();
            string rlt = VerifyDelete(q);
            if (rlt != string.Empty)
            {
                _ApiResponse.PopUpWindow(DialogMsgRequred(rlt), References.Elements.PageContents);
            }
            else
            {
                rlt = PutDeleteData(p);
                if (rlt == string.Empty)
                {
                    _ApiResponse.PopUpWindow(DialogMsgDeleted(string.Empty, string.Empty), References.Elements.PageContents);
                    _ApiResponse.ExecuteScript("DEPTDetail('HR_DEPTMV/DEPTDetail','" + q + "')");
                    _ApiResponse.ExecuteScript("DEPTDetail('HR_DEPTMV/DEPTDetail','" + q + "')");
                }
                else
                {
                    _ApiResponse.PopUpWindow(DialogMsg(Translator.Format(rlt)), References.Elements.PageContents);
                }
            }

            return _ApiResponse;
        }

        protected string VerifyDelete(string DEPTID)
        {
            string rtnvlu = string.Empty;

            if (string.IsNullOrEmpty(DEPTID))
            {
                rtnvlu = "msg_required";
            }
            else
            {
                string ExistOne = SQLData.SQLFieldValue("select count(*) from HR_DEPTHS where DEPTID = N'" + DEPTID + "'");
                if (Common.Val(string.IsNullOrEmpty(ExistOne) ? "0" : ExistOne) == 1)
                {
                    rtnvlu = "msg_cantdelete";
                }
            }

            return rtnvlu;
        }

        private string PutDeleteData(string p)
        {
            List<string> SQL = new List<string>
        {
            "   declare @DEPTID nvarchar(16)  " +
            "   set @DEPTID = (select top 1 DEPTID from HR_DEPTHS WHERE  DEPTNO = @DEPTNO)  " +
            "   delete from HR_DEPTHS WHERE  DEPTNO = @DEPTNO  " +
            "   UPDATE HR_DEPTHS SET DEPTEnd = dbo.FHR_DEPTEDT(DEPTNO) WHERE  DEPTID = @DEPTID "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@DEPTNO", Value = p, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }

}
