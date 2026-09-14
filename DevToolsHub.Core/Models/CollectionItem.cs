using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Models
{
    public class CollectionItem
    {
        public int CollectionId { get; set; }

        public int ToolId { get; set; }


        public Collection Collection { get; set; } = null!;

        public Tool Tool { get; set; } = null!;
    }
}
