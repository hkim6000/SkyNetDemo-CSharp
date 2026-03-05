using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using ASPNETCoreWeb.codes.XysPages;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_PLMV : WebGridMV
    {
        public HR_PLMV()
        {
            SQLGridPage = this.GetType().Name;
            SQLGridFilter = "dbo.FHR_EMPNAME(EMPID)";
            SQLGridOrderBy = new string[] { "PLBDT desc", "dbo.FHR_OGOGDEPTNAME(OGNO)", "EMPID" };

            string GridName = Translator.Format("title");
            string GridTables = "HR_PL";
            string GridData = "HR_PL";

            List<XDataElement> XDataElements = XDataElementsFromDataName(GridData);
            List<string> RemovedElements = new List<string> { "EMPNM", "PLEDT" };
            UIGrid UIGrid = UIGridFromXDataElements(XDataElements, RemovedElements);

            if (UIGrid.Items != null)
            {
                UIGrid.Items.Insert(0, new UIGrid.Item { Name = "Edit", Value = "'&#10148;'", Label = string.Empty });

                for (int i = 0; i < UIGrid.Items.Count; i++)
                {
                    switch (UIGrid.Items[i].Name.ToLower())
                    {
                        case "empid":
                            UIGrid.Items[i].Value = "dbo.FHR_EMPNAMEF(EMPID)";
                            break;
                        case "plcd":
                            UIGrid.Items[i].Value = "dbo.FHR_PLCDNM(PLCD)";
                            break;
                        case "sysusr":
                            UIGrid.Items[i].Value = "dbo.XF_UserName(SYSUSR)";
                            break;
                    }
                }

                SQLGridInfo.Name = GridName;
                SQLGridInfo.Id = this.GetType().Name;

                string pageNoParam = ParamValue(SQLGridInfo.Id + "_PageNo");
                double pageNoVal = Convert.ToDouble(string.IsNullOrEmpty(pageNoParam) ? "0" : pageNoParam);
                SQLGridInfo.CurrentPageNo = (pageNoVal == 0) ? 1 : (int)pageNoVal;

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
            filter.FilterHtml = filterText.HtmlText + filterBtn.HtmlText;

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

                SQLGrid.Grid.TableColumns[9].SetColumnFormat("@D {9} |" + LocalDateFormat);

                for (int i = 0; i < SQLGrid.Grid.TableColumns.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 5:
                        case 9:
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
    }

}
