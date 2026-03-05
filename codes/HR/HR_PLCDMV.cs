using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using static SkyNet.ToolKit.SQLGridSection;
using static SkyNet.WebPage;

namespace ASPNETCoreWeb.codes.HR
{
    public class HR_PLCDMV : WebGridMV
    {
        public HR_PLCDMV()
        {
            SQLGridPage = this.GetType().Name;
            SQLGridFilter = "CODE=N'PLCD' and CODE+SD01+SD02+SD03+SD04+SD05+SD06+SD07";
            SQLGridOrderBy = new string[] { "CODE", "SNO" };

            string GridName = Translator.Format("title");
            string GridTables = "XysOption";

            UIGrid UIGrid = new UIGrid();
            UIGrid.Items.AddRange(new UIGrid.Item[] {
            new UIGrid.Item { Name = "Edit", Value = "'&#10148;'", Label = string.Empty },
            new UIGrid.Item { Name = "CODE", Label = "id", IsKey = true },
            new UIGrid.Item { Name = "SNO", Label = "code", IsKey = true },
            new UIGrid.Item { Name = "SD01", Label = "name" },
            new UIGrid.Item { Name = "SD02", Label = "alias" },
            new UIGrid.Item { Name = "SD04", Label = "empsts", Value = "dbo.FHR_EmpStatusName(SD04)" },
            new UIGrid.Item { Name = "SD03", Label = "inuse" }
        });

            SQLGridInfo.Name = GridName;
            SQLGridInfo.Id = this.GetType().Name;

            string pageNoParam = ParamValue(SQLGridInfo.Id + "_PageNo");
            double pageNoVal = Convert.ToDouble(string.IsNullOrEmpty(pageNoParam) ? "0" : pageNoParam);
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

        protected override void SetGridStyle(SQLGridSection SQLGrid)
        {
            SQLGrid.Wrap.SetStyle(HtmlStyles.margin, string.Empty);
            SQLGrid.Wrap.SetStyle(HtmlStyles.marginLeft, "8px");
            SQLGrid.Wrap.SetStyle(HtmlStyles.display, "inline-block");
            SQLGrid.Wrap.SetStyle(HtmlStyles.minWidth, "60%");
            SQLGrid.Wrap.SetStyle(HtmlStyles.overflow, "auto");

            if (SQLGrid.GridData != null)
            {
                ViewMethod editMethod = GetViewMethod("edit");

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.cursor, "pointer");
                SQLGrid.Grid.TableColumns[0].SetColumnAttribute(HtmlEvents.onclick, ByPassCall(editMethod.Method, editMethod.Params));

                SQLGrid.Grid.TableColumns[1].SetHeaderStyle(HtmlStyles.display, "none");
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.display, "none");
                SQLGrid.Grid.TableColumns[2].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[3].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[4].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");

                SQLGrid.Grid.TableColumns[6].SetColumnFormat("@R {6} | 0. , 1.✓");

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
