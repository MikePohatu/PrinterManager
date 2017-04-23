using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class PrintQueue: IComparable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public PrintServer Server { get; set; }

        public PrintQueue(string name, PrintServer server)
        {
            this.Name = name;
            this.Server = server;
        }

        public int CompareTo(object queue)
        {
            PrintQueue q = queue as PrintQueue;
            return string.Compare(q.Name, this.Name, true);
        }
    }
}
