/* This Libarary File is written by Steve Houldey.
 * Its use is subject to MIT Licsence.
 * 
 * Thank you for using....
 */


// Define Using Statements.
using System;
using System.Management;
using System.Net;


//Create the namespace Network_Config

namespace Network_Config
{
    /// <summary>
    /// Network Configuration Class to Change the ip address, Gateway, Subnet and DNS settings of the
    /// network controller.....
    /// </summary>
    /// <remarks>Taken this code from multiple sources of the internet, readme.md contains links.</remarks>
    public class Network_Config
    {

        /// <summary>
        /// This method will change the network IPv4 address of the adapter, note this one uses the default adapter
        /// </summary>
        /// <param name="ipddress">Requested IPAddress of the adapater</param>
        /// <param name="subnetmask">Requested Subnet mask of the network adapter</param>
        /// <param name="gateway">Gateway address for the adapter to use</param>
        public void ChangeNetworkIP_Configuration(string ipddress, string subnetmask, string gateway)
        {
            // This method / function will change the ipaddress of the adpater along with the
            // subnet mask and gateway.  IPv4 Addressing only! - Dns will be done in another function


            
            #region Using Management Class - Create the object from the Win32 Config
            //First off we need to call in the Win32 Lib to access the network adapter.  While deprecated, 
            //this still works.  This will create an object objMC (object Management Class) so we can 
            //assignt the required information into it!

            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            #endregion

            #region loop through the object and find the adpater we want to use!

            foreach (ManagementObject objMO in objMOC)
            {
                //Check and see if the adpater found has been enabled.
                if ((bool)objMO["IPEnabled"])
                {
                    
                    //Create a setIP obeject from the management base object
                    ManagementBaseObject setIP;

                    //Set the object newIP to allow static assignment of IP addresses.
                    ManagementBaseObject newIP = objMO.GetMethodParameters("EnableStatic");

                    ManagementBaseObject setGateway;
                    //Create an object to allow static assignment of the gateway ip address
                    ManagementBaseObject newGateway = objMO.GetMethodParameters("SetGateways");

                    //Create 2 array variables and assign the pased values to them.
                    newIP["IPAddress"] = new string[] { ipddress };
                    newIP["SubnetMask"] = new string[] { subnetmask };

                    //Create 2 more arrays for the getway IPv4 address
                    newGateway["DefaultIPGateway"] = new string[] { gateway };
                    newGateway["GatewayCostMetric"] = new int[] { 1 };


                    //Set the IPv4 Address to the adapter
                    setIP = objMO.InvokeMethod("EnableStatic", newIP, null);
                    //Set the Gatway address to the adapter.
                    setGateway = objMO.InvokeMethod("SetGateways", newGateway, null);

                }
            }

            #endregion

        }
    }
}
