using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MIGAZ.Generator;
using MIGAZ.Tests.Fakes;
using System.IO;
using MIGAZ.Models;
using System.Xml;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace MIGAZ.Tests
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void ValidateSingleStorageAccount()
        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);
            fakeAsmRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\Storage1"));
            
            var templateStream = new MemoryStream();
            var blobDetailStream = new MemoryStream();
            var artefacts = new AsmArtefacts();
            artefacts.StorageAccounts.Add("mystorage");

            templateGenerator.GenerateTemplate(TestHelper.TenantId, TestHelper.SubscriptionId, artefacts, new StreamWriter(templateStream), new StreamWriter(blobDetailStream));

            JObject templateJson = TestHelper.GetJsonData(templateStream);
            Assert.AreEqual(1, templateJson["resources"].Children().Count());
            var resource = templateJson["resources"].Single();
            Assert.AreEqual("Microsoft.Storage/storageAccounts", resource["type"].Value<string>());
            Assert.AreEqual("mystoragev2", resource["name"].Value<string>());
            Assert.AreEqual("Antarctica", resource["location"].Value<string>());
            Assert.AreEqual("Standard_LRS", resource["properties"]["accountType"].Value<string>());

        }
    }
}
