using System;
using System.Management;
using core.Logging;
using System.Collections.Generic;

namespace core.Connectors
{
    public class LocalPrintingWmiConnector
    {
        private ManagementClass wmiprintclass = new ManagementClass(new ManagementPath("Win32_Printer"), null);
        //private ManagementScope _scope = new ManagementScope(ManagementPath.DefaultPath);

        public string LastShortMessage { get; set; }
        public string LastMessage { get; set; }

        public bool MapPrinter(string server, string queue)
        {
            string fullname = "\\\\" + server + "\\" + queue;
            LoggerFacade.Info("Mappinging printer: " + fullname);
            this.ClearMessages();

            try
            {
                //_scope.Connect();

                ManagementBaseObject inputParameters = wmiprintclass.GetMethodParameters("AddPrinterConnection");

                inputParameters.SetPropertyValue("Name", fullname);

                wmiprintclass.InvokeMethod("AddPrinterConnection", inputParameters, null);
                return true;
            }
            catch (Exception e)
            {
                this.LastMessage = "Failed to map printer " + fullname + ". Error message: " + e.Message;
                this.LastShortMessage = "Failed to map printer " + queue;
                LoggerFacade.Error(this.LastMessage);
                return false;
            }
        }

        public bool UnmapPrinter(string server, string queue)
        {
            string fullname = "\\\\" + server + "\\" + queue;
            LoggerFacade.Info("Unmappinging printer: " + fullname);
            this.ClearMessages();

            try
            {
                ManagementObject printer = this.GetPrinterObject(server, queue);
                if (printer != null)
                {
                    wmiprintclass.InvokeMethod("RemovePrinterConnection", printer, null);
                    return true;
                }
                else
                {
                    this.LastMessage = "Failed to unmap printer " + fullname + ". Error message: Printer not connected";
                    this.LastShortMessage = "Printer " + queue + " not connected";
                    LoggerFacade.Warn(this.LastMessage);
                    return false;
                }
            }
            catch (Exception e)
            {
                this.LastMessage = "Failed to unmap printer " + fullname + ". Error message: " + e.Message;
                this.LastShortMessage = "Failed to unmap printer " + queue;
                LoggerFacade.Error(this.LastMessage);
                return false;
            }
        }

        public bool IsPrinterConnected(string server, string queue)
        {
            //_scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = 
                new ManagementObjectSearcher("SELECT * FROM Win32_Printer WHERE ServerName=\\\\" + server + " AND ShareName=" + queue);

            ManagementObjectCollection col = searcher.Get();
            if (col.Count > 0) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Search for a queue name and return the ManagementObjectCollection
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public ManagementObjectCollection GetQueuesOfName(string queue)
        {
            //_scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_Printer WHERE ShareName=" + queue);

            return searcher.Get();
        }

        public PrinterLibrary GetConnectedPrinters()
        {
            PrinterLibrary lib = new PrinterLibrary();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            foreach (ManagementObject printer in searcher.Get())
            {
                string svr = printer["ServerName"] as string;
                string name = printer["Name"] as string;
                lib.AddPrintQueue(svr,name);
            }

            return lib;
        }

        /// <summary>
        /// Return the first ManagementObject that comes back from the query of server and queue. If not found, returns null
        /// </summary>
        /// <param name="server"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        private ManagementObject GetPrinterObject(string server, string queue)
        {
            //_scope.Connect();

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_Printer WHERE ServerName=\\\\" + server + " AND ShareName=" + queue);

            foreach (ManagementObject printer in searcher.Get())
            {
                return printer;
            }

            return null;
        }

        private void ClearMessages()
        {
            this.LastMessage = string.Empty;
            this.LastShortMessage = string.Empty;
        }
    }
}
