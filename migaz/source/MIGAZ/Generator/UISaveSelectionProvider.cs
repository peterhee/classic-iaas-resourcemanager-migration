using Newtonsoft.Json;
using System.Windows.Forms;
using MIGAZ.Models;
using System.Collections.Generic;
using System.IO;
using System;

namespace MIGAZ.Generator
{
    class UISaveSelectionProvider : ISaveSelectionProvider
    {
        string filePath;
        List<SaveSelection> saveSelections;

        public UISaveSelectionProvider()
        {
            string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MIGAZ";
            if (!Directory.Exists(filedir)) { Directory.CreateDirectory(filedir); }

            filePath = filedir + "\\MIGAZ-SaveSelection.json";
        }

        public void Save(Guid subscriptionId, ListView lvwVirtualNetworks, ListView lvwStorageAccounts, ListView lvwVirtualMachines)
        {
            string jsontext = "";

            try
            {
                StreamReader saveSelectionReader = new StreamReader(filePath);
                jsontext = saveSelectionReader.ReadToEnd();
                saveSelectionReader.Close();

                saveSelections = JsonConvert.DeserializeObject<List<SaveSelection>>(jsontext);

                // If save selection for this subscription alredy exists, remove it
                if (saveSelections.Exists(x => x.SubscriptionId == subscriptionId))
                {
                    saveSelections.Remove(saveSelections.Find(x => x.SubscriptionId == subscriptionId));
                }
            }
            catch
            {
                // If file does not exist, or invalid, starts a new object
                saveSelections = new List<SaveSelection>();
            }

            SaveSelection saveSelection = new SaveSelection();
            saveSelection.SubscriptionId = subscriptionId;

            saveSelection.VirtualNetworks = new List<string>();
            foreach (ListViewItem virtualNetwork in lvwVirtualNetworks.CheckedItems)
            {
                saveSelection.VirtualNetworks.Add(virtualNetwork.Text);
            }

            saveSelection.StorageAccounts = new List<string>();
            foreach (ListViewItem storageAccount in lvwStorageAccounts.CheckedItems)
            {
                saveSelection.StorageAccounts.Add(storageAccount.Text);
            }

            saveSelection.VirtualMachines = new List<SaveSelectioVirtualMachine>();
            foreach (ListViewItem virtualMachine in lvwVirtualMachines.CheckedItems)
            {
                SaveSelectioVirtualMachine saveSelectionVirtualMachine = new SaveSelectioVirtualMachine();
                saveSelectionVirtualMachine.CloudService = virtualMachine.SubItems[0].Text;
                saveSelectionVirtualMachine.VirtualMachine = virtualMachine.SubItems[1].Text;

                saveSelection.VirtualMachines.Add(saveSelectionVirtualMachine);
            }

            saveSelections.Add(saveSelection);

            // save blob copy details file
            jsontext = JsonConvert.SerializeObject(saveSelections, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            StreamWriter saveSelectionWriter = new StreamWriter(filePath);
            saveSelectionWriter.Write(jsontext);
            saveSelectionWriter.Close();
        }

        public void Read(Guid subscriptionId, ref ListView lvwVirtualNetworks, ref ListView lvwStorageAccounts, ref ListView lvwVirtualMachines)
        {
            try
            {
                StreamReader saveSelectionReader = new StreamReader(filePath);
                string jsontext = saveSelectionReader.ReadToEnd();
                saveSelectionReader.Close();

                saveSelections = JsonConvert.DeserializeObject<List<SaveSelection>>(jsontext);
            }
            catch
            {
                // If file does not exist, or invalid, starts a new object
                saveSelections = new List<SaveSelection>();
            }

            if (saveSelections.Exists(x => x.SubscriptionId == subscriptionId))
            {
                SaveSelection saveSelection = saveSelections.Find(x => x.SubscriptionId == subscriptionId);

                foreach (string virtualNetwork in saveSelection.VirtualNetworks)
                {
                    lvwVirtualNetworks.FindItemWithText(virtualNetwork).Checked = true;
                }

                foreach (string storageAccount in saveSelection.StorageAccounts)
                {
                    lvwStorageAccounts.FindItemWithText(storageAccount).Checked = true;
                }

                foreach (SaveSelectioVirtualMachine saveSelectionVirtualMachine in saveSelection.VirtualMachines)
                {
                    int startAt = lvwVirtualMachines.FindItemWithText(saveSelectionVirtualMachine.CloudService).Index;
                    lvwVirtualMachines.FindItemWithText(saveSelectionVirtualMachine.VirtualMachine, true, startAt).Checked = true;
                }
            }
        }
    }
}
