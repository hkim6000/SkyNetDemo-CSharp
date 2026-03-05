using SkyNet;
using SkyNet.ToolKit;
using ASPNETCoreWeb.codes.XysBases;
using System.Data;

namespace ASPNETCoreWeb.codes.HR
{

    public class HR_DEPTVW : WebBase
    {
        public class DEPTTree
        {
            public string TDate = DateTime.Now.ToString("yyyy-MM-dd");
            public DataTable TDT = null;
            public string TID = "DEPTVIEW";
        }

        private DEPTTree DpTr;

        public override void InitialViewData()
        {
            InitialTreeView(DateTime.Now.ToString("yyyy-MM-dd"));
        }

        public override string InitialViewHtml()
        {
            MenuList ViewMenuItems = GetViewMenuItems();
            Wrap ViewButtons = GetViewButtons();

            Label label = new Label();
            label.Wrap.SetStyles("font-weight:700;font-size:22px;margin:12px;");
            label.Wrap.InnerText = Translator.Format("graphicview");

            FilterSection filter = new FilterSection();
            filter.ModalWrap = true;
            filter.Wrap.SetStyle(HtmlStyles.marginTop, "110px");
            filter.Wrap.SetStyle(HtmlStyles.width, "92%");
            filter.Menu = ViewMenuItems;
            filter.FilterHtml = label.HtmlText;

            Hidden DpTrHdn = new Hidden("DEPTTree");
            DpTrHdn.SetAttribute(HtmlAttributes.value, SerializeObjectEnc(DpTr, typeof(DEPTTree)));

            HtmlElementBox elmBtns = new HtmlElementBox();
            elmBtns.DefaultStyle = false;
            elmBtns.AddItem(ViewButtons, 15);

            HtmlElementBox elmBox = new HtmlElementBox();
            elmBox.SetAttribute(HtmlAttributes.id, References.Elements.ElmBox);
            elmBox.SetStyle(HtmlStyles.width, "92%");
            elmBox.SetStyle(HtmlStyles.margin, "auto");
            elmBox.SetStyle(HtmlStyles.marginTop, "8px");
            elmBox.SetStyle(HtmlStyles.marginBottom, "80px");
            elmBox.Wrap.SetStyle(HtmlStyles.margin, "30px 10px 30px 20px");

            elmBox.AddItem(elmBtns, 4);
            elmBox.AddItem(DpTrHdn, 20);

            object tObj = TreeViewObj();
            if (tObj != null)
            {
                elmBox.AddItem(tObj, 20);
            }

            string ViewHtml = filter.HtmlText + elmBox.HtmlText;

            HtmlDoc.AddJsFile(WebEnv.HeaderScripts.TreeScript);
            return ViewHtml;
        }

        private void InitialTreeView(string BaseDate)
        {
            DpTr = new DEPTTree { TDate = BaseDate };
            string ssql = "  exec dbo.PHR_DEPTTREEVIEW N'" + DpTr.TDate + "' ";
            DpTr.TDT = SQLData.SQLDataTable(ssql);
        }

        private TreeView2 TreeViewObj()
        {
            if (DpTr == null || DpTr.TDT == null) return null;

            List<TreeView2.TreeItem> TreeItems = new List<TreeView2.TreeItem>();
            for (int i = 0; i < DpTr.TDT.Rows.Count; i++)
            {
                TreeView2.TreeItem TreeItem = new TreeView2.TreeItem()
                {
                    Id = DpTr.TDT.Rows[i][0].ToString(),
                    ParentId = DpTr.TDT.Rows[i][2].ToString()
                };
                TreeItem.Item.InnerText = DpTr.TDT.Rows[i][1].ToString();
                TreeItem.Item.SetAttribute(HtmlAttributes.data("tag"), DpTr.TDT.Rows[i][3].ToString());
                TreeItem.Item.SetStyle(HtmlStyles.border, "2px solid #1C698C");
                TreeItem.Item.SetStyle(HtmlStyles.minWidth, "120px");

                TreeItems.Add(TreeItem);
            }

            TreeView2 _TreeView = new TreeView2(DpTr.TID)
            {
                TreeItems = TreeItems,
                TreeItemGap = 10,
                TreeItemClick = string.Empty,
                TreeItemRightClick = "TreeSelected('" + this.GetType().Name + "',args)",
                TreeSubItemClick = string.Empty,
                TreeSubItemRightClick = string.Empty
            };

            _TreeView.SetStyle(HtmlStyles.marginTop, "0px");
            _TreeView.SetStyle(HtmlStyles.marginBottom, "40px");

            return _TreeView;
        }

        public ApiResponse TreeSelected()
        {
            ApiResponse _ApiResponse = new ApiResponse();
            return _ApiResponse;
        }

        public ApiResponse TreeClicked()
        {
            ApiResponse _ApiResponse = new ApiResponse();
            return _ApiResponse;
        }
    }

}
