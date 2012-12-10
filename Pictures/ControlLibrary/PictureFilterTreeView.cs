using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace msn2.net.Pictures.Controls
{
    public class PictureFilterTreeView : TreeView
    {
        private TreeNode categoryNode;
        private TreeNode dateTakenNode;
        private TreeNode dateAddedNode;

        private ContextMenu categoryContextMenu;

        public event FilterChangedHandler FilterChanged;

        public PictureFilterTreeView()
        {
            base.HideSelection = false;

            if (this.DesignMode == false)
            {
                this.categoryNode = new TreeNode("Categories") { ImageIndex = 2, SelectedImageIndex = 2 };
                this.dateTakenNode = new TreeNode("Date Taken") { ImageIndex = 2, SelectedImageIndex = 2 };
                this.dateAddedNode = new TreeNode("Date Added") { ImageIndex = 2, SelectedImageIndex = 2 };

                base.Nodes.Add(this.categoryNode);
                base.Nodes.Add(this.dateTakenNode);
                base.Nodes.Add(this.dateAddedNode);

                this.ImageList = new ImageList();
                this.ImageList.Images.Add(CommonImages.Folder);
                this.ImageList.Images.Add(CommonImages.Calendar);
                this.ImageList.Images.Add(CommonImages.Refresh);

                this.categoryContextMenu = new ContextMenu();
                this.categoryContextMenu.MenuItems.Add(new MenuItem("&Add", new EventHandler(OnCategoryAdd)));
                this.categoryContextMenu.MenuItems.Add(new MenuItem("&Edit", new EventHandler(OnCategoryEdit)));
                this.categoryContextMenu.MenuItems.Add(new MenuItem("&Delete", new EventHandler(OnCategoryDelete)));

            }
        }

        public void Load()
        {
            if (this.DesignMode == false)
            {
                this.categoryNode.Nodes.Clear();
                this.dateTakenNode.Nodes.Clear();
                this.dateAddedNode.Nodes.Clear();

                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadCategories), null);
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadDates), this.dateTakenNode);
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadDates), this.dateAddedNode);
            }
        }

        private delegate void SetImageIndexDelegate(TreeNode node, int index);

        private void SetImageIndex(TreeNode node, int index)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new SetImageIndexDelegate(SetImageIndex), new object[] { node, index });
            }
            else
            {
                node.ImageIndex = index;
                node.SelectedImageIndex = index;
            }
        }

        #region Category nodes

        private void LoadCategories(object state)
        {
            try
            {
                // clear tree
                if (PicContext.Current != null)
                {
                    // load first node
                    CategoryInfo rootCategory = PicContext.Current.CategoryManager.GetRootCategory();

                    // load first level
                    FillChildren(rootCategory, this.categoryNode, 2);

                    // Expand root node
                    this.BeginInvoke(new TreeNodeHandler(
                        delegate(TreeNode expandNode)
                        {
                            expandNode.Expand();
                            this.SelectedNode = expandNode;
                        }), 
                        new object[] { this.categoryNode });

                    this.SetImageIndex(this.categoryNode, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading category tree: " + ex.Message);
            }
        }
        
        private void FillChildren(CategoryInfo parentCategory, TreeNode n, int intLevelsToGo)
        {
            // clear out all child nodes
            this.Invoke(new TreeNodeHandler(
                delegate(TreeNode node)
                {
                    node.Nodes.Clear();
                }),
                new object[] { n });

            // load child nodes from dvCategory
            List<CategoryInfo> categories = PicContext.Current.CategoryManager.GetCategories(
                parentCategory.CategoryId);

            foreach (CategoryInfo category in categories)
            {
                CategoryTreeNode childNode = new CategoryTreeNode(category);
                childNode.ContextMenu = this.categoryContextMenu;
                this.AddTreeNode(n, childNode);

                if (intLevelsToGo > 0)
                {
                    this.FillChildren(category, childNode, intLevelsToGo - 1);
                }
                else
                {   
                    // add a fake node to be filled in
                    this.AddTreeNode(childNode, new LoadingTreeNode());
                }
            }
        }

        private delegate void AddTreeNodeDelegate(TreeNode a, TreeNode b);

        private void AddTreeNode(TreeNode parentNode, TreeNode childNode)
        {
            this.Invoke(new AddTreeNodeDelegate(
                delegate(TreeNode parent, TreeNode child)
                {
                    parent.Nodes.Add(child);
                }),
                new object[] { parentNode, childNode });
        }

        private void ValidateChildrenLoaded(CategoryTreeNode node)
        {
            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "<to load>")
            {
                FillChildren(node.Category, node, 2);
            }
        }

        private void OnCategoryAdd(object sender, EventArgs e)
        {
            CategoryTreeNode parentNode = this.SelectedNode as CategoryTreeNode;
            if (parentNode != null)
            {
                fEditCategory ec = new fEditCategory();

                ec.NewCategory(parentNode.Category.CategoryId);

                ec.ShowDialog();

                if (!ec.Cancel)
                {
                    // add new tree node
                    CategoryTreeNode newCategoryNode = new CategoryTreeNode(ec.SelectedCategory);
                    newCategoryNode.ContextMenu = this.categoryContextMenu;
                    parentNode.Nodes.Add(newCategoryNode);

                    // expand parent node and select new node
                    parentNode.Expand();
                    this.SelectedNode = newCategoryNode;
                }
            }
        }

        private void OnCategoryEdit(object sender, EventArgs e)
        {
            CategoryTreeNode node = this.SelectedNode as CategoryTreeNode;
            if (node != null)
            {
                fEditCategory ec = new fEditCategory();
                ec.CategoryID = node.Category.CategoryId;
                ec.ShowDialog();

                if (!ec.Cancel)
                {
                    node.Update(ec.SelectedCategory);
                }
            }
        }

        private void OnCategoryDelete(object sender, EventArgs e)
        {
            CategoryTreeNode node = this.SelectedNode as CategoryTreeNode;
            if (node != null)
            {
                CategoryTree.DeleteCategoryNode(node);
            }
        }

        #endregion

        #region Date nodes

        private void LoadDates(object parentNodeObject)
        {
            TreeNode parentNode = (TreeNode)parentNodeObject;

            DateCollection dates = null;
            string fieldName = null;
            if (parentNode.Text.Contains("Taken") == true)
            {
                dates = PicContext.Current.PictureManager.GetPictureDates();
                fieldName = "PictureDate";
            }
            else
            {
                dates = PicContext.Current.PictureManager.GetPictureAddedDates();
                fieldName = "PictureAddDate";
            }

            this.Invoke(
                new DateLoadDelegate(DateSelector.LoadTreeView), 
                new object[] { dates, fieldName, parentNode.Nodes, 1 });

            this.SetImageIndex(parentNode, 1);
        }

        private delegate void DateLoadDelegate(DateCollection dates, string fieldName, TreeNodeCollection nodes, int imageIndex);

        #endregion

        public string WhereClause
        {
            get
            {
                string whereClause = null;

                if (this.SelectedNode is CategoryTreeNode)
                {
                    CategoryTreeNode categoryNode = (CategoryTreeNode)this.SelectedNode;
                    whereClause = string.Format(
                        "p.PictureID in (select PictureID from PictureCategory where CategoryID IN (select SubCategoryId from CategorySubCategory where CategoryId = {0}))",
                        categoryNode.Category.CategoryId);
                }
                else if (this.SelectedNode is DateFilterTreeNode)
                {
                    whereClause = this.SelectedNode.Tag.ToString();
                }

                return whereClause;
            }
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            CategoryTreeNode node = e.Node as CategoryTreeNode;
            if (node != null)
            {
                ValidateChildrenLoaded(node);
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);

            string whereClause = null;

            if (e.Node is CategoryTreeNode)
            {
                CategoryTreeNode categoryNode = (CategoryTreeNode) e.Node;
                whereClause = string.Format(
                    "p.PictureID in (select PictureID from PictureCategory where CategoryID IN (select SubCategoryId from CategorySubCategory where CategoryId = {0}))", 
                    categoryNode.Category.CategoryId);
            }
            else if (e.Node is DateFilterTreeNode && e.Node.Tag != null)
            {
                whereClause = e.Node.Tag.ToString();
            }

            if (this.FilterChanged != null && whereClause != null)
            {
                this.FilterChanged(whereClause);
            }
        }

        private bool IsNodeAParent(TreeNode currentNode, TreeNode possibleParent)
        {
            if (currentNode == possibleParent || currentNode.Parent == possibleParent)
            {
                return true;
            }
            else
            {
                return IsNodeAParent(currentNode.Parent, possibleParent);
            }
        }



        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }
    }

    public delegate void TreeNodeHandler(TreeNode node);
    public delegate void FilterChangedHandler(string whereClause);

}

