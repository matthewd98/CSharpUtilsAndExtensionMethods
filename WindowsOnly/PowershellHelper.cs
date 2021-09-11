using System.Collections.ObjectModel;
using System.Management.Automation;

namespace CSharpUtilsAndExtensionMethods.WindowsOnly
{
    public static class PowershellHelper
    {
        public static string? ExecutePsScriptAndGetOutput(string psScriptToExecute)
        {
            // Reference: https://docs.microsoft.com/en-us/archive/blogs/kebab/executing-powershell-scripts-from-c

            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript(psScriptToExecute);

                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                foreach (PSObject outputItem in PSOutput)
                {
                    //If null object was dumped to the pipeline during the script then a null object may be present here. check for null to prevent potential NRE.
                    if (outputItem != null)
                    {
                        return outputItem.BaseObject as string;
                    }
                }

                return null;
            }
        }
    }
}
