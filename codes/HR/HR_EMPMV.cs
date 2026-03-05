using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;

namespace ASPNETCoreWeb.codes.HR
{
    public class HR_EMPMV : WebGridMV
    {
        public HR_EMPMV()
        {
            SQLGridPage = this.GetType().Name;
            SQLGridFilter = "EMPNML + EMPNMF";
            SQLGridOrderBy = new string[] { };

            string GridName = Translator.Format("title");
            string GridTables = "HR_EMP";

            List<string> GridColumns = new List<string> {
            "'&#10148;' Edit",
            "EMPID",
            "dbo.FHR_EMPNAMEF(EMPId) EMPNM",
            "EMPDOB",
            "EMPSEX",
            "dbo.FHR_EMPSTATUSNAME(EMPFLAG) EMPFlag",
            "EMPEMAIL",
            "EMPCELL",
            "EMPCITY",
            "EMPSTATE",
            "EMPNAT",
            "EMPOGNO"
        };

            List<string> GridAlias = new List<string> {
            " ",
            "EMPID",
            "EMPNM",
            "EMPDOB",
            "EMPSEX",
            "EMPFLAG",
            "EMPEMAIL",
            "EMPCELL",
            "EMPCITY",
            "EMPSTATE",
            "EMPNAT",
            "EMPOGNO"
        };

            TranslatorFormat(GridAlias);

            SQLGridInfo.Name = GridName;
            SQLGridInfo.Id = this.GetType().Name;

            string pageNoStr = ParamValue(SQLGridInfo.Id + "_PageNo");
            double pageNoVal = Convert.ToDouble(string.IsNullOrEmpty(pageNoStr) ? "0" : pageNoStr);
            SQLGridInfo.CurrentPageNo = pageNoVal == 0 ? 1 : (int)pageNoVal;

            SQLGridInfo.LinesPerPage = 30;
            SQLGridInfo.ExcludeDownloadColumns = new int[] { 0 };
            SQLGridInfo.DisplayCount = SQLGridSection.DisplayCounts.FilteredNAll;
            SQLGridInfo.TitleEnabled = true;

            SQLGridInfo.Query = new SQLGridSection.SQLQuery
            {
                Tables = GridTables,
                OrderBy = new string[] { "dbo.FHR_EMPNAMEF(EMPID)" },
                Columns = GridColumns.ToArray(),
                ColumnAlias = GridAlias.ToArray(),
                Filters = string.IsNullOrEmpty(ParamValue(SQLGridInfo.Id + "_Filter"))
                    ? SQLGridFilter + " like '%%' "
                    : ParamValue(SQLGridInfo.Id + "_Filter")
            };
        }

        private void TranslatorFormat(List<string> cols)
        {
            if (cols != null)
            {
                for (int i = 0; i < cols.Count; i++)
                {
                    cols[i] = Translator.Format(cols[i]);
                }
            }
        }

        protected override void SetGridStyle(SQLGridSection SQLGrid)
        {
            SQLGrid.Wrap.SetStyle(HtmlStyles.margin, string.Empty);
            SQLGrid.Wrap.SetStyle(HtmlStyles.marginLeft, "8px");
            SQLGrid.Wrap.SetStyle(HtmlStyles.display, "inline-block");
            SQLGrid.Wrap.SetStyle(HtmlStyles.width, "100%");
            SQLGrid.Wrap.SetStyle(HtmlStyles.overflow, "auto");
            SQLGrid.Grid.Table.SetStyle(HtmlStyles.marginBottom, "20px");

            if (SQLGrid.GridData != null)
            {
                ViewMethod editMethod = GetViewMethod("edit");

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.cursor, "pointer");
                SQLGrid.Grid.TableColumns[0].SetColumnAttribute(HtmlEvents.onclick, ByPassCall(editMethod.Method, editMethod.Params));

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[2].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[3].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[4].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[5].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[6].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");

                SQLGrid.Grid.TableColumns[3].SetColumnFormat("@D {3} |" + LocalDateFormat);

                for (int i = 0; i < SQLGrid.Grid.TableColumns.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 3:
                        case 4:
                        case 5:
                            SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "center");
                            break;
                        default:
                            SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "left");
                            break;
                    }
                }
            }
        }
    }

}
