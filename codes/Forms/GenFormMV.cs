using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;

namespace ASPNETCoreWeb.codes.Forms
{

    public class GenFormMV : WebBase
    {
        private string BaseFilter = "";
        private SQLGridSection.SQLGridInfo SQLGridInfo;

        public GenFormMV()
        {
            SQLGridInfo = new SQLGridSection.SQLGridInfo
            {
                Id = "DataGrid",
                Name = "DataGrid",
                CurrentPageNo = (int)(Common.Val(ParamValue("DataGrid_PageNo")) == 0 ? 1 : Common.Val(ParamValue("DataGrid_PageNo"))),
                LinesPerPage = 10,
                ExcludeDownloadColumns = new int[] { 0 },
                TDictionary = this.HtmlTranslator.TDictionary,
                Query = new SQLGridSection.SQLQuery
                {
                    Tables = "GenForm",
                    OrderBy = new string[] { "SYSDTE desc" },
                    Columns = new string[] {
                        "FormId",
                        "FormTitle",
                        "FormDesc",
                        "FormFlag",
                        "dbo.XF_FileList(FileRefId) FileList",
                        "dbo.XF_OffetTime(SYSDTE," + CSTimeOffset.ToString() + ") SYSDTE",
                        "dbo.XF_UserName(SYSUSR) SYSUSR"
                    },
                    ColumnAlias = new string[] {
                        Translator.Format("formid"),
                        Translator.Format("formtitle"),
                        Translator.Format("formdesc"),
                        Translator.Format("locked"),
                        Translator.Format("files"),
                        Translator.Format("sysdte"),
                        Translator.Format("sysusr")
                    },
                    Filters = string.IsNullOrEmpty(ParamValue("DataGrid_Filter")) ? BaseFilter : ParamValue("DataGrid_Filter")
                }
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
            // Note: Kept "XysBulletinMV/SearchClicked" as per original VB code provided
            filterBtn.SetAttribute(HtmlEvents.onclick, ByPassCall("XysBulletinMV/SearchClicked"));

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
            elmBox.Wrap.SetStyle(HtmlStyles.margin, "20px 10px 20px 10px");
            elmBox.Wrap.SetStyle(HtmlStyles.overflow, "auto");

            elmBox.AddItem(SQLGrid);

            return filter.HtmlText + elmBox.HtmlText;
        }

        private void SetGridStyle(SQLGridSection SQLGrid)
        {
            SQLGrid.Wrap.SetStyle(HtmlStyles.margin, string.Empty);
            SQLGrid.Wrap.SetStyle(HtmlStyles.marginLeft, "8px");
            SQLGrid.Wrap.SetStyle(HtmlStyles.display, "inline-block");

            if (SQLGrid.GridData != null)
            {
                ViewMethod editMethod = GetViewMethod("edit");

                SQLGrid.Grid.TableColumns[0].SetHeaderStyle(HtmlStyles.display, "none");
                SQLGrid.Grid.TableColumns[0].SetColumnStyle(HtmlStyles.display, "none");

                SQLGrid.Grid.TableColumns[1].SetColumnAttribute(HtmlEvents.onclick, ByPassCall(editMethod.Method, editMethod.Params));
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.textDecoration, "underline");
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.cursor, "pointer");
                SQLGrid.Grid.TableColumns[1].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");

                SQLGrid.Grid.TableColumns[3].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");
                SQLGrid.Grid.TableColumns[5].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");

                SQLGrid.Grid.TableColumns[3].SetColumnFormat("@R {3} | 0. , 1.✓");
                SQLGrid.Grid.TableColumns[6].SetColumnFormat("@D {6} |" + LocalDateFormat);

                for (int i = 0; i <= 6; i++)
                {
                    SQLGrid.Grid.TableColumns[i].SetColumnStyle(HtmlStyles.verticalAlign, "top");
                }

                for (int i = 0; i < SQLGrid.Grid.TableColumns.Count; i++)
                {
                    switch (i)
                    {
                        case 3:
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

        public ApiResponse SearchClicked()
        {
            string FilterBoxValue = ParamValue("FilterBox");

            var info = SQLGridInfo;
            info.Query.Filters = BaseFilter + "FormTitle+FormDesc+dbo.XF_UserName(SYSUSR) like N'%" + FilterBoxValue + "%' ";

            SQLGridSection SQLGrid = new SQLGridSection(info);
            if (SQLGrid.Grid != null) SQLGrid.Grid.Table.SetAttribute(HtmlAttributes.@class, "tableX");
            SetGridStyle(SQLGrid);

            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.ReplaceSQLGridSection("DataGrid", SQLGrid);
            _ApiResponse.StoreLocalValue("FilterBoxValue", FilterBoxValue);
            return _ApiResponse;
        }
    }

}
