using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /// <summary>
    /// Class represening graph's vertex
    /// </summary>
    public class Vertex
    {
        public List<int> adjances;//List of adjacent vertices
        public int id;
        public int ipower;
        public int opower;
        public int power;
        public bool isIsolated;
        public bool isHanging;
        public bool isLooped;

        public Vertex(int id)
        {
            power = 0;
            isIsolated = false;
            isLooped = false;
            isHanging = false;
            adjances = new List<int>();
            this.id = id; 

        }
    }
}
