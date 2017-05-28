using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /// <summary>
    /// Class representing graph's edge
    /// </summary>
    public class Edge
    {
        public int n1;//Source vertex
        public int n2;//Destination vertex
        public int w;

        public Edge(int n1,int n2,int w)
        {
            this.n1 = n1;
            this.n2 = n2;
            this.w = w;
        }

    }
}
