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

namespace MIGAZ.Tests
{
    [TestClass]
    public class VirtualMachineTests
    {
        
        private JObject GenerateSingleVMTemplate()

        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);
            fakeAsmRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VM1"));

            var templateStream = new MemoryStream();
            var blobDetailStream = new MemoryStream();
            var artefacts = new AsmArtefacts();
            artefacts.VirtualMachines.Add(new CloudServiceVM() { CloudService = "myservice", VirtualMachine = "myservice" });

            templateGenerator.GenerateTemplate(TestHelper.TenantId, TestHelper.SubscriptionId, artefacts, new StreamWriter(templateStream), new StreamWriter(blobDetailStream));

            return TestHelper.GetJsonData(templateStream);
        }

        [TestMethod]
        public void VMDiskUrlsAreCorrectlyUpdated()
        {
            var templateJson = GenerateSingleVMTemplate();
            var vmResource = templateJson["resources"].Where(j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual("myservice", vmResource["name"]);

            var osDisk = vmResource["properties"]["storageProfile"]["osDisk"];
            Assert.AreEqual("https://myservicev2.blob.core.windows.net/vhds/myservice-myservice-os-1445207070064.vhd", osDisk["vhd"]["uri"].Value<string>());
        }

        [TestMethod]
        public void AvailabilitySetNameIsBasedOnCloudServceName()
        {
            var templateJson = GenerateSingleVMTemplate();

            string expectedASName = "myservice-defaultAS";
            string expectedASId = $"[concat(resourceGroup().id, '/providers/Microsoft.Compute/availabilitySets/{expectedASName}')]";

            var vmResource = templateJson["resources"].Where(j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual(expectedASId, vmResource["properties"]["availabilitySet"]["id"].Value<string>());
            Assert.AreEqual(expectedASId, vmResource["dependsOn"][1].Value<string>());

            var asResource = templateJson["resources"].Where(j => j["type"].Value<string>() == "Microsoft.Compute/availabilitySets").Single();
            Assert.AreEqual(expectedASName, asResource["name"].Value<string>());
        }
    }
}
