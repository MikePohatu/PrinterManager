using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class PrintServer
    {
        public string Name { get; set; }
        public List<PrintQueue> Queues { get; set; }

        public PrintServer()
        {
            this.Queues = new List<PrintQueue>();
        }

        public void AddQueue(PrintQueue queue)
        {
            if (queue != null) { this.Queues.Add(queue); }
        }

        public void RemoveQueue(PrintQueue queue)
        {
            if (queue != null) { this.Queues.Remove(queue); }
        }
    }
}
