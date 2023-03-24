using DomainLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Presentation_Layer
{
    public class Utils
    {
        public TreeViewItem ListToTree(List<StructuralObject> list)
        {
            Stack<TreeViewItem> stack = new Stack<TreeViewItem>();

            foreach (StructuralObject obj in list)
            {
                int levelOffset = stack.Count - (obj.HierarchyId.Count(x => x == '/') - 2);
                for (int i = 0; i < levelOffset; i++)
                    stack.Pop();
                TreeViewItem item = new TreeViewItem();
                item.Name = "TreeItem" + obj.Id.ToString();
                item.Header = obj.Name;
                if (stack.Count > 0)
                    stack.Peek().Items.Add(item);
                stack.Push(item);
            }

            TreeViewItem first = null;
            while(stack.Count > 0)
                first = stack.Pop();
            return first;
        }
    }
}
