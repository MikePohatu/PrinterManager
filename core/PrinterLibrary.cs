
using System.Collections.Generic;
using core.Logging;


namespace core
{
    public class PrinterLibrary
    {
        private const string _localname = "*LOCAL*";
        private SortedDictionary<string, PrintServer> _servers = new SortedDictionary<string, PrintServer>();

        public PrintServer AddPrintServer(string name)
        {
            string newname = name;
            if (string.IsNullOrWhiteSpace(newname))
            { newname = _localname; }

            PrintServer svr = this.GetServer(newname);

            if (svr == null)
            {
                svr = new PrintServer();
                svr.Name = newname.ToUpper();
                this._servers.Add(svr.Name, svr);
            }
            else { LoggerFacade.Error("Server " + svr.Name + " already in PrinterLibrary"); }

            return svr;
        }


        public void AddPrintQueue(string server, string name)
        {
            string svrname = server;

            if (string.IsNullOrWhiteSpace(svrname))
            { svrname = _localname; }

            PrintServer svr = this.GetServer(svrname);
            if (svr == null) { svr = this.AddPrintServer(server); }

            svr.AddQueue(name);
        }


        public PrintServer GetServer(string name)
        {
            string formattedname = name.ToUpper();
            PrintServer svr;

            this._servers.TryGetValue(formattedname, out svr);
            return svr;
        }


        public List<PrintQueue> GetAllQueues()
        {
            List<PrintQueue> queues = new List<PrintQueue>();

            foreach (PrintServer p in this._servers.Values)
            { queues.AddRange(p.Queues); }

            queues.Sort();
            return queues;
        }
    }
}
