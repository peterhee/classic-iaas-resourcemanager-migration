using System;
using System.Collections.Generic;

namespace MIGAZ.Models
{
    public class SaveSelection
    {
        public Guid SubscriptionId;
        public List<string> VirtualNetworks;
        public List<string> StorageAccounts;
        public List<SaveSelectioVirtualMachine> VirtualMachines;
    }

    public class SaveSelectioVirtualMachine
    {
        public string CloudService;
        public string VirtualMachine;
    }
}
