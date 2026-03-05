using Microsoft.Data.SqlClient;
using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_WKMSTMV : WebGridMV
    {
        public HR_WKMSTMV()
        {
            SQLGridPage = this.GetType().Name;
            SQLGridFilter = " a.WKFLAG = 1 and convert(nvarchar(10),getdate(),121) between b.WKBDT and b.WKEDT  and a.EMPID + dbo.FHR_EMPNAME(b.EMPID) ";
            SQLGridOrderBy = new string[] { "a.EMPID" };

            string GridName = Translator.Format("title");
            string GridTables = "HR_WKMST a inner join HR_WKMSTHS b on a.EMPID = b.EMPID ";

            UIGrid UIGrid = new UIGrid();
            UIGrid.Items.AddRange(new UIGrid.Item[] {
                new UIGrid.Item { Name = "Edit", Value = "'&#10148;'", Label = string.Empty },
                new UIGrid.Item { Name = "WKNO", Value = "b.WKNO", Label = "WKNO", IsKey = true },
                new UIGrid.Item { Name = "EMPID", Value = "a.EMPID", Label = "EMPID", IsKey = true }, // Corrected WKID to EMPID based on join logic
                new UIGrid.Item { Name = "EMPNAME", Value = " dbo.FHR_EMPNAME(b.EMPID)", Label = "EMPNAME" },
                new UIGrid.Item { Name = "WKBDT", Value = "b.WKBDT", Label = "WKBDT" },
                new UIGrid.Item { Name = "WKCD1", Value = "b.WKCD1", Label = "WKCD1" },
                new UIGrid.Item { Name = "WKCD2", Value = "b.WKCD2", Label = "WKCD2" },
                new UIGrid.Item { Name = "WKCD3", Value = "b.WKCD3", Label = "WKCD3" },
                new UIGrid.Item { Name = "WKCD4", Value = "b.WKCD4", Label = "WKCD4" },
                new UIGrid.Item { Name = "WKCD5", Value = "b.WKCD5", Label = "WKCD5" },
                new UIGrid.Item { Name = "WKFLAG", Value = "a.WKFLAG", Label = "WKFLAG" },
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
            filterCheck.Checks.AddItem("FilterCheck", "1", isChecked);
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

                SQLGrid.Grid.TableColumns[10].SetColumnFormat("@R {10} | 0. , 1.✓");
                SQLGrid.Grid.TableColumns[11].SetColumnFormat("@D {11} |" + LocalDateFormat);

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.cursor, "pointer");
                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[0].SetColumnAttribute(HtmlEvents.onclick, "WKDetail('HR_WKMSTMV/WKDetail', '{2}')");
                SQLGrid.Grid.TableColumns[0].SetColumnAttribute(HtmlAttributes.id, "R_{2}");

                for (int i = 0; i < SQLGrid.Grid.TableColumns.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
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

            SQLGridFilter = "  convert(nvarchar(10),getdate(),121) between b.WKBDT and b.WKEDT  and a.EMPID + dbo.FHR_EMPNAME(b.EMPID) ";
            double chkVal = Convert.ToDouble(string.IsNullOrEmpty(filterCheckVal) ? "0" : filterCheckVal);

            if (chkVal != 1)
            {
                SQLGridFilter = " a.WKFLAG = 1 and " + SQLGridFilter;
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
            HtmlDocument pophtml = PartialDocument("HR_WKPE", t);
            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.PopUpWindow(pophtml, References.Elements.PageContents);
            return _ApiResponse;
        }

        public ApiResponse PopAddRecord()
        {
            string t = GetDataValue("t");
            HtmlDocument pophtml = PartialDocument("HR_WKPA", t);
            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.PopUpWindow(pophtml, References.Elements.PageContents);
            return _ApiResponse;
        }

        public ApiResponse WKDetail()
        {
            string t = GetDataValue("t");
            string rid = "R_" + t;

            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.TableToggleRow(rid, WKDetailGrid(t));
            _ApiResponse.ExecuteScript("RefreshTableRow('" + rid + "','" + WKRefresh(t) + "')");

            return _ApiResponse;
        }

        private string WKRefresh(string WKid)
        {
            string ssql = " select top 1 '',WKNO, WKID,WKNAME,dbo.FHR_EMPNAME(EMPID),TOPWK,WKBDT,WKORDER,convert(varchar(18),WKFLAG)" +
                          " from VHR_WK where WKid = N'" + WKid + "' order by WKBDT desc ";
            DataTable dt = SQLData.SQLDataTable(ssql);

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Rows[0][6] = WebCore.FormattedValue("@D " + dt.Rows[0][6].ToString() + " |" + LocalDateFormat);
                dt.Rows[0][8] = WebCore.FormattedValue("@R " + dt.Rows[0][8].ToString() + " | 0. , 1.✓");
                return string.Join("|", dt.Rows[0].ItemArray);
            }
            return string.Empty;
        }

        private string WKDetailGrid(string WKid)
        {
            string ssql = " select '&#10149;' [E],WKNO,WKID,WKBDT,dbo.FHR_EMPNAME(EMPID) EMPNAME,WKNAME,TOPWK, " +
                          "        dbo.FHR_WKSTATUSName(WKSTATUS) WKSTATUS,WKDESC,'&#10008;' as [D] " +
                          " from VHR_WKhs where WKid = N'" + WKid + "' order by WKBDT desc";
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
            btn.SetAttribute(HtmlEvents.onclick, "PopAddRecord('HR_WKMV/PopAddRecord','" + WKid + "')");

            Button btn1 = new Button();
            btn1.SetAttribute(HtmlAttributes.value, Translator.Format("removeall"));
            btn1.SetStyles("border-radius:4px; border:1px solid #aaa; background-color:#e8e8e8; color:#ff6600; padding:4px;padding-left:6px; padding-right:6px; display:inline-block; font-size:12px;");
            btn1.SetAttribute(HtmlEvents.onclick, "$Call('PartialView', 'm=HR_WKEV&t=" + WKid + "')");

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
            DTGrid.TableColumns[0].SetColumnAttribute(HtmlEvents.onclick, "PopEditRecord('HR_WKMV/PopEditRecord','{1}')");

            DTGrid.TableColumns[9].SetColumnAttribute(HtmlAttributes.title, Translator.Format("delete"));
            DTGrid.TableColumns[9].SetColumnStyle(HtmlStyles.cursor, "pointer");
            DTGrid.TableColumns[9].SetColumnAttribute(HtmlEvents.onclick, "$Call('HR_WKMV/DeleteData','p={1}&q={2}')");

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
                    _ApiResponse.ExecuteScript("WKDetail('HR_WKMV/WKDetail','" + q + "')");
                    _ApiResponse.ExecuteScript("WKDetail('HR_WKMV/WKDetail','" + q + "')");
                }
                else
                {
                    _ApiResponse.PopUpWindow(DialogMsg(Translator.Format(rlt)), References.Elements.PageContents);
                }
            }

            return _ApiResponse;
        }

        protected string VerifyDelete(string WKID)
        {
            if (string.IsNullOrEmpty(WKID))
            {
                return "msg_required";
            }

            string ExistOne = SQLData.SQLFieldValue("select count(*) from HR_WKHS where WKID = N'" + WKID + "'");
            if (Common.Val(ExistOne) == 1)
            {
                return "msg_cantdelete";
            }

            return string.Empty;
        }

        private string PutDeleteData(string p)
        {
            List<string> SQL = new List<string>
        {
            "   declare @WKID nvarchar(16)  " +
            "   set @WKID = (select top 1 WKID from HR_WKHS WHERE  WKNO = @WKNO)  " +
            "   delete from HR_WKHS WHERE  WKNO = @WKNO  " +
            "   UPDATE HR_WKHS SET WKEDT = dbo.FHR_WKEDT(WKNO) WHERE  WKID = @WKID "
        };

            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParams.Add(new SqlParameter { ParameterName = "@WKNO", Value = p, SqlDbType = SqlDbType.NVarChar });

            return PutData(SqlWithParams(SQL, SqlParams));
        }
    }

}
