using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MANUUFinance
{
    class imagesAvailable
    {
        internal bool imagesavailable(int imageId)
        {
            bool status = false;
            string curFile = @"C:\Users\Md Shoaib Alam\Desktop\attahment\" + imageId + ".jpg";
            if (File.Exists(curFile))
            {
                return status = true;
            }
            else
            {
                return status;
            }
        }
    }
}
