using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace pet_shop.Classes
{
    public static class Navigation
    {
        public static Frame ActiveFrame { get; set; }
        public static Models.User CurrentUser { get; set; }
    }
}
