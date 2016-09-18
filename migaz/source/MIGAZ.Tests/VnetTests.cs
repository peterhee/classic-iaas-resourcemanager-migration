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
            Assert.AreEqual("10.2.0.0-Gateway", gw.First()["name"].Value<string>());

            var localGw = templateJson["resources"].Children().Where(
               r => r["type"].Value<string>() == "Microsoft.Network/localNetworkGateways");
            Assert.AreEqual(2, localGw.Count());
            Assert.AreEqual("MOBILEDATACENTER-LocalGateway", localGw.First()["name"].Value<string>());
            Assert.AreEqual("EastUSNet-LocalGateway", localGw.Last()["name"].Value<string>());

            var pips = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/publicIPAddresses");
            Assert.AreEqual(1, pips.Count());
            Assert.AreEqual("10.2.0.0-Gateway-PIP", pips.First()["name"].Value<string>());
            Assert.AreEqual("Dynamic", pips.First()["properties"]["publicIPAllocationMethod"].Value<string>());
        }

        [TestMethod]
        public void ValidateSingleVnetWithNsgAndRT()
        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);
            fakeAsmRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VNET2"));

            var templateStream = new MemoryStream();
            var blobDetailStream = new MemoryStream();
            var artefacts = new AsmArtefacts();
            artefacts.VirtualNetworks.Add("asmtest");

            templateGenerator.GenerateTemplate(TestHelper.TenantId, TestHelper.SubscriptionId, artefacts, new StreamWriter(templateStream), new StreamWriter(blobDetailStream));

            JObject templateJson = TestHelper.GetJsonData(templateStream);

            // Validate NSG
            var nsgs = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/networkSecurityGroups");
            Assert.AreEqual(1, nsgs.Count());
            Assert.AreEqual("asmnsg", nsgs.First()["name"].Value<string>());

            // Validate NSG rules
            JArray rules = (JArray) nsgs.First()["properties"]["securityRules"];
            Assert.AreEqual(2, rules.Count());
            Assert.AreEqual("Enable-Internal-DNS", rules[0]["name"].Value<string>());
            Assert.AreEqual("Port-7777-rule", rules[1]["name"].Value<string>());

            // Validate RouteTable
            var rt = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/routeTables");
            Assert.AreEqual(1, rt.Count());
            Assert.AreEqual("asmrt", rt.First()["name"].Value<string>());

            // Validate Routes
            JArray routes = (JArray)rt.First()["properties"]["routes"];
            Assert.AreEqual(1, routes.Count());
            Assert.AreEqual("all-traffic-to-fw", routes[0]["name"].Value<string>());

            // Validate VNETs
            var vnets = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworks");
            Assert.AreEqual(1, vnets.Count());
            Assert.AreEqual("asmtest", vnets.First()["name"].Value<string>());

            // Validate subnets
            var subnets = vnets.First()["properties"]["subnets"];
            Assert.AreEqual(1, subnets.Count());
            Assert.AreEqual("Subnet-1", subnets.First()["name"].Value<string>());
            StringAssert.Contains(subnets.First()["properties"]["networkSecurityGroup"]["id"].Value<string>(), "networkSecurityGroups/asmnsg");
            StringAssert.Contains(subnets.First()["properties"]["routeTable"]["id"].Value<string>(), "routeTables/asmrt");
        }

        [TestMethod]
        public void ValidateSingleVnetWithExpressRouteGateway()
        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);
            fakeAsmRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VNET3"));

            var templateStream = new MemoryStream();
            var blobDetailStream = new MemoryStream();
            var artefacts = new AsmArtefacts();
            artefacts.VirtualNetworks.Add("vnet3");

            var messages = templateGenerator.GenerateTemplate(TestHelper.TenantId, TestHelper.SubscriptionId, artefacts, new StreamWriter(templateStream), new StreamWriter(blobDetailStream));

            JObject templateJson = TestHelper.GetJsonData(templateStream);

            // Validate VNETs
            var vnets = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworks");
            Assert.AreEqual(1, vnets.Count());
            Assert.AreEqual("vnet3", vnets.First()["name"].Value<string>());

            // Validate subnets
            var subnets = vnets.First()["properties"]["subnets"];
            Assert.AreEqual(2, subnets.Count());

            // Validate gateway
            var gw = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworkGateways");
            Assert.AreEqual(1, gw.Count());
            Assert.AreEqual("vnet3-Gateway", gw.First()["name"].Value<string>());
            Assert.AreEqual("ExpressRoute", gw.First()["properties"]["gatewayType"].Value<string>());

            // Validate no local network
            var localGw = templateJson["resources"].Children().Where(
               r => r["type"].Value<string>() == "Microsoft.Network/localNetworkGateways");
            Assert.AreEqual(0, localGw.Count());

            // Validate connection
            var conn = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/connections");
            Assert.AreEqual(1, conn.Count());
            Assert.AreEqual("vnet3-Gateway-localsite-connection", conn.First()["name"].Value<string>());
            Assert.AreEqual("ExpressRoute", conn.First()["properties"]["connectionType"].Value<string>());
            Assert.IsNotNull(conn.First()["properties"]["peer"]["id"].Value<string>());

            // Validate message
            Assert.AreEqual(1, messages.Count);
            StringAssert.Contains(messages[0], "ExpressRoute");
        }
    }
}
