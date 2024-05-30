using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{

    public abstract class BaseProduct
    {
        public abstract string Name { get; }
        public abstract List<string> GetAttributes();
    }
    public class Product
    {
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
    }

}
