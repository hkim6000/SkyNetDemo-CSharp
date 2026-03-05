using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;

namespace ASPNETCoreWeb.codes.HR
{
    public class HR_WKDAYMV : WebGridMV
    {
        public HR_WKDAYMV()
        {
            SQLGridPage = this.GetType().Name;
            SQLGridFilter = "WKDAY between convert(varchar(10),dateadd(day,1,EOMONTH(GETDATE(),-1)) ,121) and convert(varchar(10),EOMONTH(GETDATE()) ,121) ";
            SQLGridOrderBy = new string[] { "WKDAY" };

            string GridName = Translator.Format("title");
            string GridTables = "HR_WKDAY";

            UIGrid UIGrid = new UIGrid();
            UIGrid.Items.AddRange(new UIGrid.Item[] {
                new UIGrid.Item { Name = "Edit", Value = "'&#10148;'", Label = string.Empty },
                new UIGrid.Item { Name = "WKDAY", Label = "WKDAY", IsKey = true },
                new UIGrid.Item { Name = "DAYTYPE", Value = "dbo.XF_DayType(DAYTYPE)", Label = "DAYTYPE" },
                new UIGrid.Item { Name = "DAYMEMO", Label = "DAYMEMO" }
            });

            SQLGridInfo.Name = GridName;
            SQLGridInfo.Id = this.GetType().Name;

            string pageNoParam = ParamValue(SQLGridInfo.Id + "_PageNo");
            double pageNoVal = Convert.ToDouble(string.IsNullOrEmpty(pageNoParam) ? "0" : pageNoParam);
            SQLGridInfo.CurrentPageNo = (pageNoVal == 0) ? 1 : (int)pageNoVal;

            SQLGridInfo.LinesPerPage = 60;
            SQLGridInfo.ExcludeDownloadColumns = new int[] { 0, 1 };
            SQLGridInfo.DisplayCount = SQLGridSection.DisplayCounts.None;
            SQLGridInfo.Query = new SQLGridSection.SQLQuery
            {
                Tables = GridTables,
                OrderBy = SQLGridOrderBy,
                Columns = UIGrid.Columns().ToArray(),
                ColumnAlias = UIGrid.Labels().ToArray(),
                Filters = SQLGridFilter
            };
        }

        public override ApiResponse SearchClicked()
        {
            string FilterBgn = ParamValue("FilterBgn");
            string FilterEnd = ParamValue("FilterEnd");

            ApiResponse _ApiResponse = new ApiResponse();

            DateTime dateBgn, dateEnd;
            if (DateTime.TryParse(FilterBgn, out dateBgn) && DateTime.TryParse(FilterEnd, out dateEnd))
            {
                SQLGridInfo.Query.Filters = " WKDAY between '" + dateBgn.ToString("yyyy-MM-dd") + "' and '" + dateEnd.ToString("yyyy-MM-dd") + "' ";
                SQLGridSection SQLGrid = new SQLGridSection(SQLGridInfo);
                if (SQLGrid.Grid != null) SQLGrid.Grid.Table.SetAttribute(HtmlAttributes.@class, "tableX");
                SetGridStyle(SQLGrid);

                _ApiResponse.ReplaceSQLGridSection(SQLGridInfo.Id, SQLGrid);
            }
            return _ApiResponse;
        }

        public override string InitialViewHtml()
        {
            MenuList ViewMenuItems = GetViewMenuItems(new string[] { "home" });
            Wrap ViewButtons = GetViewButtons(new string[] { });

            DateTime now = DateTime.Now;
            string FilterBgn = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            string FilterEnd = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).ToString("yyyy-MM-dd");

            Texts filterDate1 = new Texts(TextTypes.date);
            filterDate1.Label.InnerText = Translator.Format("from");
            filterDate1.Label.SetStyle(HtmlStyles.fontSize, "14px");
            filterDate1.Text.SetStyle(HtmlStyles.fontSize, "14px");
            filterDate1.Text.SetStyle(HtmlStyles.width, "120px");
            filterDate1.Text.SetAttribute(HtmlAttributes.id, "FilterBgn");
            filterDate1.Text.SetAttribute(HtmlAttributes.value, FilterBgn);

            Texts filterDate2 = new Texts(TextTypes.date);
            filterDate2.Label.InnerText = Translator.Format("to");
            filterDate2.Label.SetStyle(HtmlStyles.fontSize, "14px");
            filterDate2.Text.SetStyle(HtmlStyles.fontSize, "14px");
            filterDate2.Text.SetStyle(HtmlStyles.width, "120px");
            filterDate2.Text.SetAttribute(HtmlAttributes.id, "FilterEnd");
            filterDate2.Text.SetAttribute(HtmlAttributes.value, FilterEnd);

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
            filter.Menu.Wrap.SetStyle(HtmlStyles.paddingTop, "12px");
            filter.FilterHtml = filterDate1.HtmlText + filterDate2.HtmlText + filterBtn.HtmlText;

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
            SQLGrid.Wrap.SetStyle(HtmlStyles.minWidth, "60%");
            SQLGrid.Wrap.SetStyle(HtmlStyles.overflow, "auto");

            if (SQLGrid.GridData != null)
            {
                ViewMethod editMethod = GetViewMethod("edit");

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.cursor, "pointer");
                SQLGrid.Grid.TableColumns[0].SetColumnAttribute(HtmlEvents.onclick, ByPassCall(editMethod.Method, editMethod.Params));

                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.fontWeight, "bold");
                SQLGrid.Grid.TableColumns[2].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[3].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");

                SQLGrid.Grid.TableColumns[1].SetColumnFormat("@D {1} |" + LocalDateFormat + "& @S {2} | Weekend.color:#ff6600; ,Holiday.color:#070AB5;");
                SQLGrid.Grid.TableColumns[2].SetColumnFormat("@R {2} | Weekday. , Weekend.Weekend , Holiday.Holiday & @S {2} | Weekend.color:#ff6600; ,Holiday.color:#070AB5;");

                for (int i = 0; i < SQLGrid.Grid.TableColumns.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
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
