using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using ASPNETCoreWeb.codes.XysPages;

namespace ASPNETCoreWeb.codes.HR
{
    public class HR_ORGMV : WebGridMV
    {
        public HR_ORGMV()
        {
            SQLGridPage = typeof(HR_ORGMV).ToString();
            SQLGridFilter = "OGNM";
            SQLGridOrderBy = new string[] { "OGFLAG desc", "DEPTID", "OGB Desc", "OGA" };

            string GridName = Translator.Format("title");
            string GridTables = "HR_ORG";
            string GridData = "HR_ORG";

            List<XDataElement> XDataElements = XDataElementsFromDataName(GridData);
            List<string> RemovedElements = new List<string> { "OGNM" };
            UIGrid UIGrid = UIGridFromXDataElements(XDataElements, RemovedElements);

            if (UIGrid.Items != null)
            {
                UIGrid.Items.Insert(0, new UIGrid.Item { Name = "Edit", Value = "'&#10148;'", Label = string.Empty });

                for (int i = 0; i < UIGrid.Items.Count; i++)
                {
                    switch (UIGrid.Items[i].Name.ToLower())
                    {
                        case "deptid":
                            UIGrid.Items[i].Value = "dbo.FHR_DeptName(DEPTID)";
                            break;
                        case "oga":
                            UIGrid.Items[i].Value = "dbo.FHR_OGANM(OGA)";
                            break;
                        case "ogb":
                            UIGrid.Items[i].Value = "dbo.FHR_OGBNM(OGB)";
                            break;
                        case "ogc":
                            UIGrid.Items[i].Value = "dbo.FHR_OGCNM(OGC)";
                            break;
                        case "ogg":
                            UIGrid.Items[i].Value = "dbo.FHR_OGGNM(OGG)";
                            break;
                        case "ogh":
                            UIGrid.Items[i].Value = "dbo.FHR_OGHNM(OGH)";
                            break;
                        case "ogbgt":
                            UIGrid.Items[i].Value = "FORMAT(OGBGT/1000, N'#,##0') + 'K'";
                            break;
                        case "sysusr":
                            UIGrid.Items[i].Value = "dbo.XF_UserName(SYSUSR)";
                            break;
                    }
                }

                SQLGridInfo.Name = GridName;
                SQLGridInfo.Id = typeof(HR_ORGMV).ToString();

                string pageNoStr = ParamValue(SQLGridInfo.Id + "_PageNo");
                double pageNoVal = Convert.ToDouble(string.IsNullOrEmpty(pageNoStr) ? "0" : pageNoStr);
                SQLGridInfo.CurrentPageNo = pageNoVal == 0 ? 1 : (int)pageNoVal;

                SQLGridInfo.LinesPerPage = 30;
                SQLGridInfo.ExcludeDownloadColumns = new int[] { 0, 1 };
                SQLGridInfo.DisplayCount = SQLGridSection.DisplayCounts.FilteredNAll;
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

                SQLGrid.Grid.TableColumns[1].SetHeaderStyle(HtmlStyles.display, "none");
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.display, "none");

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.cursor, "pointer");
                SQLGrid.Grid.TableColumns[0].SetColumnAttribute(HtmlEvents.onclick, ByPassCall(editMethod.Method, editMethod.Params));

                for (int i = 0; i <= 8; i++)
                {
                    SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                }

                SQLGrid.Grid.TableColumns[10].SetColumnFormat("@R {10} | 0. , 1.✓");
                SQLGrid.Grid.TableColumns[11].SetColumnFormat("@R {11} | 0. , 1.✓");
                SQLGrid.Grid.TableColumns[12].SetColumnFormat("@D {12} |" + LocalDateFormat);

                for (int i = 0; i < SQLGrid.Grid.TableColumns.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "center");
                            break;
                        case 8:
                            SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "right");
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
