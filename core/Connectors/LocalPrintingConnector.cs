using System;
using System.Management;
using core.Logging;


namespace core.Connectors
{
    public class LocalPrintingConnector
    {
        public string LastMessage { get; set; }

        public bool MapPrinter(string server, string queue)
        {
            string fullname = "\\\\" + server + "\\" + queue;
            LoggerFacade.Info("Mappinging printer: " + fullname);

            try
            {
                ManagementScope scope = new ManagementScope(ManagementPath.DefaultPath);
                scope.Connect();

                ManagementClass wmiprintclass = new ManagementClass(new ManagementPath("Win32_Printer"), null);
                ManagementBaseObject oInputParameters = wmiprintclass.GetMethodParameters("AddPrinterConnection");

                oInputParameters.SetPropertyValue("Name", fullname);

                wmiprintclass.InvokeMethod("AddPrinterConnection", oInputParameters, null);
                return true;
            }
            catch (Exception e)
            {
                this.LastMessage = "Failed to map printer " + fullname + ". Error message: " + e.Message;
                LoggerFacade.Error(this.LastMessage);
                return false;
            }
        }

        public bool UnmapPrinter(string server, string queue)
        {
            string fullname = "\\\\" + server + "\\" + queue;
            LoggerFacade.Info("Mappinging printer: " + fullname);

            try
            {
                ManagementScope scope = new ManagementScope(ManagementPath.DefaultPath);
                scope.Connect();

                ManagementClass wmiprintclass = new ManagementClass(new ManagementPath("Win32_Printer"), null);
                ManagementBaseObject oInputParameters = wmiprintclass.GetMethodParameters("RemovePrinterConnection");

                oInputParameters.SetPropertyValue("Name", fullname);

                wmiprintclass.InvokeMethod("RemovePrinterConnection", oInputParameters, null);
                return true;
            }
            catch (Exception e)
            {
                this.LastMessage = "Failed to map printer " + fullname + ". Error message: " + e.Message;
                LoggerFacade.Error(this.LastMessage);
                return false;
            }
        }

        public bool IsPrinterConnected(string server, string queue)
        {
            return true;
        }
    }
}
