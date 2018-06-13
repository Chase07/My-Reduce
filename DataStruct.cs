using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class ConditionObj
    {
        public KeyValuePair<string, int> decisionTotal;// string: Decision, int: number of neighbors having same decision value
        public ConditionObj(string value, int amount)
        {
            decisionTotal = new KeyValuePair<string, int>(value, amount);
        }
    }
}
