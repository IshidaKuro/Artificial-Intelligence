using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra_Algorithm
{
    class Point
    {
        public int x { get; set; }
        public int y { get; set; }
        public int point { get; set; }
        public List<Point> connections = new List<Point>();
        public bool visited = false;
        public double tentativeDistance = double.MaxValue;
        public Point previousPoint;


    }
}
