using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    //internal class AllowWithoutAuthAttribute
    //{
    //}
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllowWithoutAuthAttribute : Attribute
    {
    }

}
