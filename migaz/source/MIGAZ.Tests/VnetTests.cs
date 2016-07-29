using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MIGAZ.Tests.Fakes;
using MIGAZ.Generator;
using System.IO;
using MIGAZ.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace MIGAZ.Tests
{
    /// <summary>
    /// Summary description for VnetTests
    /// </summary>
    [TestClass]
    public class VnetTests
    {
        [TestMethod]
        public void ValidateComplexSingleVnet()
        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);
            fakeAsmRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VNET1"));

            var templateStream = new MemoryStream();
            var blobDetailStream = new MemoryStream();
            var artefacts = new AsmArtefacts();
            artefacts.VirtualNetworks.Add("10.2.0.0");

            templateGenerator.GenerateTemplate(TestHelper.TenantId, TestHelper.SubscriptionId, artefacts, new StreamWriter(templateStream), new StreamWriter(blobDetailStream));

            JObject templateJson = TestHelper.GetJsonData(templateStream);

            // Validate VNETs
            var vnets = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworks");
            Assert.AreEqual(1, vnets.Count());
            Assert.AreEqual("10.2.0.0", vnets.First()["name"].Value<string>());

            // Validate subnets
            var subnets = vnets.First()["properties"]["subnets"];
            Assert.AreEqual(8, subnets.Count());

            // Validate gateway
            var gw = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworkGateways");
            Assert.AreEqual(1, gw.Count());
            Assert.AreEqual("10.2.0.0-VPNGateway", gw.First()["name"].Value<string>());

            var localGw = templateJson["resources"].Children().Where(
               r => r["type"].Value<string>() == "Microsoft.Network/localNetworkGateways");
            Assert.AreEqual(2, localGw.Count());
            Assert.AreEqual("MOBILEDATACENTER-LocalGateway", localGw.First()["name"].Value<string>());
            Assert.AreEqual("EastUSNet-LocalGateway", localGw.Last()["name"].Value<string>());

            var pips = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/publicIPAddresses");
            Assert.AreEqual(1, pips.Count());
            Assert.AreEqual("10.2.0.0-VPNGateway-PIP", pips.First()["name"].Value<string>());
            Assert.AreEqual("Dynamic", pips.First()["properties"]["publicIPAllocationMethod"].Value<string>());
        }

    }
}
