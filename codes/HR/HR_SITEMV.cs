using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using ASPNETCoreWeb.codes.XysPages;
using static SkyNet.ToolKit.SQLGridSection;
using static SkyNet.WebPage;

namespace ASPNETCoreWeb.codes.HR
{
    public class HR_SITEMV : WebGridMV
    {
        public HR_SITEMV()
        {
            SQLGridPage = this.GetType().Name;
            SQLGridFilter = "SITENAME + SITEALIAS";
            SQLGridOrderBy = new string[] { };

            string GridName = Translator.Format("title");
            string GridTables = "HR_SITE";
            string GridData = "HR_SITE";

            List<XDataElement> XDataElements = XDataElementsFromDataName(GridData);
            List<string> RemovedElements = new List<string> {
                "SITEORDER", "SITEREGNO", "SITETAXNO", "SITEOWNER", "SITEBIZ2", "SITEADDR1", "SITEADDR2",
                "SITEZIP", "SITEFAX", "SITEMAIL", "SITEWEB", "SYSDTE", "SYSUSR"
            };
            UIGrid UIGrid = UIGridFromXDataElements(XDataElements, RemovedElements);

            if (UIGrid.Items != null)
            {
                UIGrid.Items.Insert(0, new UIGrid.Item { Name = "Edit", Value = "'&#10148;'", Label = string.Empty });

                SQLGridInfo.Name = GridName;
                SQLGridInfo.Id = this.GetType().Name;

                string pageNoParam = ParamValue(SQLGridInfo.Id + "_PageNo");
                double pageNoVal = Convert.ToDouble(string.IsNullOrEmpty(pageNoParam) ? "0" : pageNoParam);
                SQLGridInfo.CurrentPageNo = pageNoVal == 0 ? 1 : (int)pageNoVal;

                SQLGridInfo.LinesPerPage = 30;
                SQLGridInfo.ExcludeDownloadColumns = new int[] { 0 };
                SQLGridInfo.DisplayCount = SQLGridSection.DisplayCounts.FilteredOnly;
                SQLGridInfo.TitleEnabled = true;

                SQLGridInfo.Query = new SQLGridSection.SQLQuery
                {
                    Tables = GridTables,
                    OrderBy = UIGrid.Items.FindAll(x => x.IsKey == true).Select(x => x.Name).ToArray(),
                    Columns = UIGrid.Columns().ToArray(),
                    ColumnAlias = UIGrid.Labels().ToArray(),
                    Filters = string.IsNullOrEmpty(ParamValue(SQLGridInfo.Id + "_Filter"))
                        ? SQLGridFilter + " like '%%' "
                        : ParamValue(SQLGridInfo.Id + "_Filter")
                };
            }
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

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[2].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[10].SetHeaderStyle(HtmlStyles.whiteSpace, "nowrap");

                SQLGrid.Grid.TableColumns[10].SetColumnFormat("@R {10} | 0. , 1.✓");

                for (int i = 0; i < SQLGrid.Grid.TableColumns.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 10:
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
