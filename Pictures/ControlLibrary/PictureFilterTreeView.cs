using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Linq;

namespace msn2.net.Pictures.Controls
{
    public class PictureFilterTreeView : TreeView
    {
        private TreeNode categoryNode;
        private TreeNode dateTakenNode;
        private TreeNode dateAddedNode;

        private ContextMenu categoryContextMenu;

        public event FilterChangedHandler FilterChanged;
        private string selectPath = null;

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
                this.categoryContextMenu.MenuItems.Add(new MenuItem("&Move", new EventHandler(OnCategoryMove)));
                this.categoryContextMenu.MenuItems.Add(new MenuItem("&Delete", new EventHandler(OnCategoryDelete)));

                this.categoryNode.ContextMenu = new ContextMenu();
                this.categoryNode.ContextMenu.MenuItems.Add(new MenuItem("&Add", new EventHandler(OnCategoryAdd)));
                this.categoryNode.ContextMenu.MenuItems.Add(new MenuItem("&Refresh", new EventHandler(OnCategoryRefresh)));
            }
        }

        public void Load()
        {
            if (this.DesignMode == false)
            {
                this.ReloadCategories();

                this.dateTakenNode.Nodes.Clear();
                this.dateAddedNode.Nodes.Clear();

                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadDates), this.dateTakenNode);
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadDates), this.dateAddedNode);
            }
        }

        private void ReloadCategories()
        {
            this.categoryNode.Nodes.Clear();
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadCategories), null);
        }

        private delegate void SetImageIndexDelegate(TreeNode node, int index);

        private void SetImageIndex(TreeNode node, int index)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new SetImageIndexDelegate(SetImageIndex), new object[] { node, index });
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
                    bool noSelectPath = this.selectPath == null;
                    // load first node
                    Category rootCategory = PicContext.Current.CategoryManager.GetRootCategory();

                    // load first level
                    FillChildren(rootCategory, this.categoryNode, 2);

                    // Expand root node
                    if (noSelectPath == true)
                    {
                        this.BeginInvoke(new TreeNodeHandler(
                            delegate(TreeNode expandNode)
                            {
                                expandNode.Expand();
                                this.SelectedNode = expandNode;
                            }),
                            new object[] { this.categoryNode });
                    }

                    this.SetImageIndex(this.categoryNode, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading category tree: " + ex.Message);
            }
        }

        private void FillChildren(Category parentCategory, TreeNode n, int intLevelsToGo)
        {
            // clear out all child nodes
            this.Invoke(new TreeNodeHandler(
                delegate(TreeNode node)
                {
                    node.Nodes.Clear();
                }),
                new object[] { n });

            // load child nodes from dvCategory
            List<Category> categories = PicContext.Current.CategoryManager.GetChildrenCategories(
                parentCategory.Id);

            foreach (Category category in categories)
            {
                CategoryTreeNode childNode = new CategoryTreeNode(category);
                childNode.ContextMenu = this.categoryContextMenu;
                this.AddTreeNode(n, childNode);

                bool inSelectPath = this.selectPath != null && this.selectPath.StartsWith(category.Path);

                if (intLevelsToGo > 0 || inSelectPath)
                {
                    this.FillChildren(category, childNode, intLevelsToGo - 1);
                }
                else
                {
                    // add a fake node to be filled in
                    this.AddTreeNode(childNode, new LoadingTreeNode());
                }

                if (this.selectPath != null && category.Path == this.selectPath)
                {
                    this.selectPath = null;
                    this.SelectAndExpandToNode(childNode);
                }
            }
        }

        private void SelectAndExpandToNode(TreeNode node)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new TreeNodeHandler(this.SelectAndExpandToNode), node);
            }
            else
            {
                this.SelectedNode = node;

                TreeNode temp = node;
                while (temp != null)
                {
                    if (temp.IsExpanded == false)
                    {
                        temp.Expand();
                    }
                    temp = temp.Parent;
                }

                node.EnsureVisible();
            }
        }

        private delegate void AddTreeNodeDelegate(TreeNode a, TreeNode b);

        private void AddTreeNode(TreeNode parentNode, TreeNode childNode)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new AddTreeNodeDelegate(this.AddTreeNode), parentNode, childNode);
            }
            else
            {
                parentNode.Nodes.Add(childNode);
            }
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
            TreeNode selectedNode = this.SelectedNode;
            if (selectedNode != null)
            {
                CategoryTreeNode parentNode = this.SelectedNode as CategoryTreeNode;
                Category category = new Category();
                category.ParentId = parentNode == null ? PicContext.Current.CategoryManager.GetRootCategory().Id : parentNode.Category.Id;

                CategoryEditDialog ec = new CategoryEditDialog(PicContext.Current, category);
                if (ec.ShowDialog() == DialogResult.OK)
                {
                    // add new tree node
                    CategoryTreeNode newCategoryNode = new CategoryTreeNode(ec.Category);
                    newCategoryNode.ContextMenu = this.categoryContextMenu;
                    selectedNode.Nodes.Add(newCategoryNode);

                    // expand parent node and select new node
                    selectedNode.Expand();
                    this.SelectedNode = newCategoryNode;
                }
            }
        }

        private void OnCategoryEdit(object sender, EventArgs e)
        {
            CategoryTreeNode node = this.SelectedNode as CategoryTreeNode;
            if (node != null)
            {
                CategoryEditDialog ec = new CategoryEditDialog(PicContext.Current, node.Category);
                if (ec.ShowDialog() == DialogResult.OK)
                {
                    node.Update(ec.Category);
                }
            }
        }

        private void OnCategoryMove(object sender, EventArgs e)
        {
            CategoryTreeNode node = this.SelectedNode as CategoryTreeNode;
            if (node != null)
            {
                fSelectCategory dialog = new fSelectCategory();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        PicContext.Current.CategoryManager.MoveCategory(node.Category.Id, dialog.SelectedCategory.Id);
                        Category movedCategory = PicContext.Current.CategoryManager.GetCategory(node.Category.Id);

                        this.SelectCategory(movedCategory);
                    }
                    catch (ApplicationException ex)
                    {
                        MessageBox.Show(ex.Message, "Move Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        void OnCategoryRefresh(object sender, EventArgs e)
        {
            this.ReloadCategories();
        }

        private void OnCategoryDelete(object sender, EventArgs e)
        {
            CategoryTreeNode node = this.SelectedNode as CategoryTreeNode;
            if (node != null)
            {
                CategoryTree.DeleteCategoryNode(node);
            }
        }

        public void SelectCategory(Category category)
        {
            if (category != null)
            {
                this.selectPath = category.Path;
            }
            this.ReloadCategories();
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

        public IQueryable<Picture> GetPictureQuery()
        {
            IQueryable<Picture> query = null;

            if (this.SelectedNode is CategoryTreeNode)
            {
                Category category = ((CategoryTreeNode)this.SelectedNode).Category;

                query = from p in PicContext.Current.PictureManager.GetPictures()
                        where p.PictureCategories.Any(pc => pc.Category.Path.StartsWith(category.Path))
                        select p;
            }
            else
            {
                query = from p in PicContext.Current.PictureManager.GetPictures()
                        select p;
            }

            return query;
        }

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
                        categoryNode.Category.Id);
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
                CategoryTreeNode categoryNode = (CategoryTreeNode)e.Node;
                whereClause = string.Format(
                    "p.PictureID in (select PictureID from PictureCategory where CategoryID IN (select SubCategoryId from CategorySubCategory where CategoryId = {0}))",
                    categoryNode.Category.Id);
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

