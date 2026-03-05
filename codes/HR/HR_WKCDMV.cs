using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using static SkyNet.ToolKit.SQLGridSection;
using static SkyNet.WebPage;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_WKCDMV : WebGridMV
    {
        public HR_WKCDMV()
        {
            SQLGridPage = this.GetType().Name;
            SQLGridFilter = "WKCD + WKNM + WKDESC ";
            SQLGridOrderBy = new string[] { "WKCD" };

            string GridName = Translator.Format("title");
            string GridTables = "HR_WKCD";

            UIGrid UIGrid = new UIGrid();
            UIGrid.Items.AddRange(new UIGrid.Item[] {
            new UIGrid.Item { Name = "Edit", Value = "'&#10148;'", Label = string.Empty },
            new UIGrid.Item { Name = "WKCD", Label = "WKCD", IsKey = true },
            new UIGrid.Item { Name = "WKNM", Label = "WKNM" },
            new UIGrid.Item { Name = "WKDESC", Label = "WKDESC" },
            new UIGrid.Item { Name = "WKBGN", Label = "WKBGN" },
            new UIGrid.Item { Name = "WKEND", Label = "WKEND" },
            new UIGrid.Item { Name = "WKA", Value = "dbo.XF_MIN2HHMM(WKA)", Label = "WKA" },
            new UIGrid.Item { Name = "WKB", Value = "dbo.XF_MIN2HHMM(WKB)", Label = "WKB" },
            new UIGrid.Item { Name = "WKC", Value = "dbo.XF_MIN2HHMM(WKC)", Label = "WKC" },
            new UIGrid.Item { Name = "WKD", Value = "dbo.XF_MIN2HHMM(WKD)", Label = "WKD" },
            new UIGrid.Item { Name = "WKE", Value = "dbo.XF_MIN2HHMM(WKE)", Label = "WKE" },
            new UIGrid.Item { Name = "WKFLAG", Label = "WKFLAG" }
        });

            SQLGridInfo.Name = GridName;
            SQLGridInfo.Id = this.GetType().Name;

            string pageNoParam = ParamValue(SQLGridInfo.Id + "_PageNo");
            double pageNoVal = Convert.ToDouble(string.IsNullOrEmpty(pageNoParam) ? "0" : pageNoParam);
            SQLGridInfo.CurrentPageNo = pageNoVal == 0 ? 1 : (int)pageNoVal;

            SQLGridInfo.LinesPerPage = 60;
            SQLGridInfo.ExcludeDownloadColumns = new int[] { 0, 1 };
            SQLGridInfo.DisplayCount = SQLGridSection.DisplayCounts.None;
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

        protected override void SetGridStyle(SQLGridSection SQLGrid)
        {
            SQLGrid.Wrap.SetStyle(HtmlStyles.margin, string.Empty);
            SQLGrid.Wrap.SetStyle(HtmlStyles.marginLeft, "8px");
            SQLGrid.Wrap.SetStyle(HtmlStyles.display, "inline-block");
            SQLGrid.Wrap.SetStyle(HtmlStyles.width, "100%");
            SQLGrid.Wrap.SetStyle(HtmlStyles.overflow, "auto");

            if (SQLGrid.GridData != null)
            {
                ViewMethod editMethod = GetViewMethod("edit");

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.cursor, "pointer");
                SQLGrid.Grid.TableColumns[0].SetColumnAttribute(HtmlEvents.onclick, ByPassCall(editMethod.Method, editMethod.Params));

                SQLGrid.Grid.TableColumns[4].SetColumnStyle(HtmlStyles.fontWeight, "700");
                SQLGrid.Grid.TableColumns[5].SetColumnStyle(HtmlStyles.fontWeight, "700");
                SQLGrid.Grid.TableColumns[6].SetColumnStyle(HtmlStyles.fontWeight, "bold");
                SQLGrid.Grid.TableColumns[6].SetColumnStyle(HtmlStyles.color, "Green");

                SQLGrid.Grid.TableColumns[11].SetColumnFormat("@R {11} | 0. , 1.✓");

                for (int i = 0; i < SQLGrid.Grid.TableColumns.Count; i++)
                {
                    SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");

                    switch (i)
                    {
                        case 0:
                        case 1:
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
    }

}
