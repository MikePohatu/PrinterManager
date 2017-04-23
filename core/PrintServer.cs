using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using core.Logging;

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

        public void AddQueue(string queue)
        {
            if (string.IsNullOrWhiteSpace(queue)) { LoggerFacade.Error("Empty queue passed to AddQueue"); }
            PrintQueue newqueue = new PrintQueue(queue.ToUpper(), this);
            if (newqueue != null) { this.Queues.Add(newqueue); }
        }

        public void RemoveQueue(PrintQueue queue)
        {
            if (queue != null) { this.Queues.Remove(queue); }
        }
    }
}
