using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MIGAZ.Tests.Fakes;
using System.Xml;
using MIGAZ.Generator;
using System.Collections;
using System.IO;
using MIGAZ.Models;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace MIGAZ.Tests
{
    [TestClass]
    public class VirtualMachineTests
    {
        
        private async Task<JObject> GenerateSingleVMTemplate()

        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);
            fakeAsmRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VM1"));

            var templateStream = new MemoryStream();
            var blobDetailStream = new MemoryStream();
            var artefacts = new AsmArtefacts();
            artefacts.VirtualMachines.Add(new CloudServiceVM() { CloudService = "myservice", VirtualMachine = "myservice" });

            await templateGenerator.GenerateTemplate(TestHelper.TenantId, TestHelper.SubscriptionId, artefacts, new StreamWriter(templateStream), new StreamWriter(blobDetailStream));

            return TestHelper.GetJsonData(templateStream);
        }

        [TestMethod]
        public async Task VMDiskUrlsAreCorrectlyUpdated()
        {
            var templateJson = await GenerateSingleVMTemplate();
            var vmResource = templateJson["resources"].Where(j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual("myservice", vmResource["name"]);

            var osDisk = vmResource["properties"]["storageProfile"]["osDisk"];
            Assert.AreEqual("https://myservicev2.blob.core.windows.net/vhds/myservice-myservice-os-1445207070064.vhd", osDisk["vhd"]["uri"].Value<string>());
        }

        [TestMethod]
        public async Task AvailabilitySetNameIsBasedOnCloudServceName()
        {
            var templateJson = await GenerateSingleVMTemplate();

            string expectedASName = "myservice-defaultAS";
            string expectedASId = $"[concat(resourceGroup().id, '/providers/Microsoft.Compute/availabilitySets/{expectedASName}')]";

            var vmResource = templateJson["resources"].Where(j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual(expectedASId, vmResource["properties"]["availabilitySet"]["id"].Value<string>());
            Assert.AreEqual(expectedASId, vmResource["dependsOn"][1].Value<string>());

            var asResource = templateJson["resources"].Where(j => j["type"].Value<string>() == "Microsoft.Compute/availabilitySets").Single();
            Assert.AreEqual(expectedASName, asResource["name"].Value<string>());
        }

        [TestMethod]
        public async Task ValidateSingleVMWithDataDisksNotInVnet()
        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);
            fakeAsmRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VM2"));

            var templateStream = new MemoryStream();
            var blobDetailStream = new MemoryStream();
            var artefacts = new AsmArtefacts();
            artefacts.VirtualMachines.Add(new CloudServiceVM() { CloudService = "myasmvm", VirtualMachine = "myasmvm" });

            await templateGenerator.GenerateTemplate(TestHelper.TenantId, TestHelper.SubscriptionId, artefacts, new StreamWriter(templateStream), new StreamWriter(blobDetailStream));

            var templateJson = TestHelper.GetJsonData(templateStream);

            // Validate VNET
            var vnets = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworks");
            Assert.AreEqual(1, vnets.Count());
            Assert.AreEqual("myasmvm-VNET", vnets.First()["name"].Value<string>());

            // Validate VM
            var vmResource = templateJson["resources"].Where(
                j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual("myasmvm", vmResource["name"].Value<string>());

            // Validate disks
            var dataDisks = (JArray)vmResource["properties"]["storageProfile"]["dataDisks"];
            Assert.AreEqual(2, dataDisks.Count);
            Assert.AreEqual("Disk1", dataDisks[0]["name"].Value<string>());
            Assert.AreEqual("Disk2", dataDisks[1]["name"].Value<string>());
        }

        [TestMethod]
        public async Task ValidateVMInVnetButNotInSubnet()
        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);
            fakeAsmRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VM3"));

            var templateStream = new MemoryStream();
            var blobDetailStream = new MemoryStream();
            var artefacts = new AsmArtefacts();
            artefacts.VirtualMachines.Add(new CloudServiceVM() { CloudService = "CS3", VirtualMachine = "VM3" });

            await templateGenerator.GenerateTemplate(TestHelper.TenantId, TestHelper.SubscriptionId, artefacts, new StreamWriter(templateStream), new StreamWriter(blobDetailStream));

            var templateJson = TestHelper.GetJsonData(templateStream);

            // Validate VM
            var vmResource = templateJson["resources"].Where(
                j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual("VM3", vmResource["name"].Value<string>());
            StringAssert.Contains(vmResource["properties"]["networkProfile"]["networkInterfaces"][0]["id"].Value<string>(),
                "'/providers/Microsoft.Network/networkInterfaces/VM3'");

            // Validate NIC
            var nicResource = templateJson["resources"].Where(
                j => j["type"].Value<string>() == "Microsoft.Network/networkInterfaces").Single();
            Assert.AreEqual("VM3", nicResource["name"].Value<string>());
            StringAssert.Contains(nicResource["properties"]["ipConfigurations"][0]["properties"]["subnet"]["id"].Value<string>(),
                "'/providers/Microsoft.Network/virtualNetworks/POC-Vnet/subnets/Subnet1'");
        }
    }
}
