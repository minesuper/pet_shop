using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pet_shop.Classes
{
    public class ImageToByte
    {
        public static void GetImageBytes()
        {
            var list = Models.pets_shopEntities.GetContext().Product.ToList();
            foreach (var item in list)
            {
                string path = Directory.GetCurrentDirectory() + @"\Images\" + item.NameOfImage;
                if (File.Exists(path))
                {
                    item.ProductImage = File.ReadAllBytes(path);
                }
                Models.pets_shopEntities.GetContext().SaveChanges();
            }
        }
    }
}
