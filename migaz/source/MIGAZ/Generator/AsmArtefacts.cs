using System.Collections.Generic;

namespace MIGAZ.Generator
{
    public class AsmArtefacts
    {
        public AsmArtefacts()
        {
            StorageAccounts = new List<string>();
            VirtualNetworks = new List<string>();
            VirtualMachines = new List<CloudServiceVM>();
        }

        public List<string> StorageAccounts { get; private set; }
        public List<string> VirtualNetworks { get; private set; }
        public List<CloudServiceVM> VirtualMachines { get; private set; }
    }
}