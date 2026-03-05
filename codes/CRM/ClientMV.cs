using ASPNETCoreWeb.codes.XysBases;

namespace ASPNETCoreWeb.codes.CRM
{

    public class ClientMV : WebBase
    {
        /*
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
            filterBtn.SetAttribute(HtmlEvents.onclick, ByPassCall("ClientMV/SearchClicked"));

            ToolKit.FilterSection filter = new ToolKit.FilterSection();
            filter.ModalWrap = true;
            filter.Wrap.SetStyle(HtmlStyles.marginTop, "110px");
            filter.Wrap.SetStyle(HtmlStyles.width, "90%");
            filter.Menu = ViewMenuItems;
            filter.FilterHtml = filterText.HtmlText + filterBtn.HtmlText;

            string sSql = "Select ClientId,ClientName,ClientData,sysdte,dbo.XF_UserName(SYSUSR) sysusr from Client Order by ClientName ";
            DataGrid DataGrid = new DataGrid(GetDataTable(sSql));
            SetGridStyle(DataGrid);

            HtmlElementBox elmBox = new HtmlElementBox();
            elmBox.SetAttribute(HtmlAttributes.id, References.Elements.ElmBox);
            elmBox.SetStyle(HtmlStyles.width, "90%");
            elmBox.SetStyle(HtmlStyles.margin, "auto");
            elmBox.SetStyle(HtmlStyles.marginTop, "8px");
            elmBox.SetStyle(HtmlStyles.marginBottom, "80px");
            elmBox.Wrap.SetStyle(HtmlStyles.margin, "20px 10px 20px 10px");

            elmBox.AddItem(DataGrid);

            string ViewHtml = filter.HtmlText + elmBox.HtmlText;

            return ViewHtml;
        }

        private DataTable GetDataTable(string ssql)
        {
            DataTable srcDt = SQLData.SQLDataTable(ssql);

            DataTable trgDt = new DataTable();
            trgDt.Columns.Add(srcDt.Columns[0].ColumnName);
            trgDt.Columns.Add(srcDt.Columns[1].ColumnName);

            // DynamicColumns returns DataColumn array
            DataColumn[] dynamicCols = DynamicColumns("client");
            if (dynamicCols != null)
            {
                foreach (var col in dynamicCols)
                {
                    trgDt.Columns.Add(new DataColumn(col.ColumnName, col.DataType));
                }
            }

            trgDt.Columns.Add(srcDt.Columns[3].ColumnName);
            trgDt.Columns.Add(srcDt.Columns[4].ColumnName);

            for (int i = 0; i < trgDt.Columns.Count; i++)
            {
                trgDt.Columns[i].Caption = Translator.Format(trgDt.Columns[i].ColumnName);
            }

            for (int i = 0; i < srcDt.Rows.Count; i++)
            {
                List<object> RowData = new List<object>();
                RowData.Add(srcDt.Rows[i][0].ToString());
                RowData.Add(srcDt.Rows[i][1].ToString());

                string DataField = srcDt.Rows[i][2].ToString();
                List<NameValue> NameVlus = (List<NameValue>)DeserializeObject(DataField, typeof(List<NameValue>));

                // Fill dynamic columns logic
                int dynamicStartIdx = 2;
                int dynamicEndIdx = trgDt.Columns.Count - 3; // Index before sysdte and sysusr

                for (int j = dynamicStartIdx; j <= dynamicEndIdx; j++)
                {
                    string colname = trgDt.Columns[j].ColumnName;
                    NameValue nv = NameVlus?.Find(x => x.name == colname);
                    RowData.Add(nv != null ? nv.value : string.Empty);
                }

                RowData.Add(srcDt.Rows[i][3].ToString());
                RowData.Add(srcDt.Rows[i][4].ToString());

                trgDt.Rows.Add(RowData.ToArray());
            }
            return trgDt;
        }

        private void SetGridStyle(DataGrid Grid)
        {
            Grid.Table.SetAttribute(HtmlAttributes.id, "DataGrid");
            Grid.Table.SetAttribute(HtmlAttributes.@class, "tableX");
            Grid.Table.SetStyle(HtmlStyles.margin, string.Empty);
            Grid.Table.SetStyle(HtmlStyles.marginLeft, "8px");
            Grid.Table.SetStyle(HtmlStyles.display, "inline-block");

            Grid.TableColumns[0].SetHeaderStyle(HtmlStyles.display, "none");
            Grid.TableColumns[0].SetColumnStyle(HtmlStyles.display, "none");

            Grid.TableColumns[1].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");

            Grid.TableColumns[1].SetColumnAttribute(HtmlEvents.onclick, ByPassCall("PartialView", "m=ClientEV&t={0}"));
            Grid.TableColumns[1].SetColumnStyle(HtmlStyles.textDecoration, "underline");
            Grid.TableColumns[1].SetColumnStyle(HtmlStyles.cursor, "pointer");
            Grid.TableColumns[1].SetColumnStyle(HtmlStyles.whiteSpace, "nowrap");

            Grid.TableColumns[4].SetColumnFormat("@D {4} |" + LocalDateFormat);

            for (int i = 0; i < Grid.TableColumns.Count; i++)
            {
                switch (i)
                {
                    case 2:
                    case 3:
                    case 4:
                        Grid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "center");
                        break;
                    default:
                        Grid.TableColumns[i].SetColumnStyle(HtmlStyles.textAlign, "left");
                        break;
                }
            }
        }

        public ApiResponse SearchClicked()
        {
            string FilterBoxValue = ParamValue("FilterBox");

            string sSql = " Select ClientId,ClientName,ClientData,SysDte,SysUsr from Client " +
                          " where ClientName + ClientData like N'%" + FilterBoxValue + "%' Order by ClientName ";

            DataGrid DataGrid = new DataGrid(GetDataTable(sSql));
            SetGridStyle(DataGrid);

            ApiResponse _ApiResponse = new ApiResponse();
            _ApiResponse.ReplaceElement("DataGrid", DataGrid.HtmlText);
            _ApiResponse.StoreLocalValue("FilterBoxValue", FilterBoxValue);
            return _ApiResponse;
        }
        */
    }

}
