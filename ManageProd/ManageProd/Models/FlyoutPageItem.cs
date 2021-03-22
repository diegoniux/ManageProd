using System;
using System.Collections.Generic;
using System.Text;

namespace ManageProd.Models
{
    public class FlyoutPageItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }
}
