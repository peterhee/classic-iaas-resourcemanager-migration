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

        private const string sampleAsmStorageInfo = @"<StorageService>
  <Url>storage-account-request-uri</Url>
  <ServiceName>mystorage</ServiceName>
  <StorageServiceProperties>
    <AccountType>Standard_LRS</AccountType>
    <Description>description</Description>
    <AffinityGroup>affinity-group</AffinityGroup>
    <Location>Antarctica</Location>
    <Label>base64-encoded-label</Label>  
    <Status>status</Status>
    <Endpoints>
      <Endpoint>https://mystorage.blob.core.windows.net</Endpoint>
      <Endpoint>https://mystorage.queue.core.windows.net</Endpoint>
      <Endpoint>https://mystorage.table.core.windows.net</Endpoint>
      <Endpoint>https://mystorage.file.core.windows.net</Endpoint>
    </Endpoints>
    <GeoReplicationEnabled>geo-replication-indicator</GeoReplicationEnabled>
    <GeoPrimaryRegion>primary-region</GeoPrimaryRegion> 
    <StatusOfPrimary>primary-status</StatusOfPrimary>
    <LastGeoFailoverTime>DateTime</LastGeoFailoverTime>  
    <GeoSecondaryRegion>secondary-region</GeoSecondaryRegion>  
    <StatusOfSecondary>secondary-status</StatusOfSecondary>
    <CreationTime>time-of-creation</CreationTime>
    <CustomDomains>
      <CustomDomain>
        <Name>name-of-custom-domain</Name>
      </CustomDomain>
    </CustomDomains>
    <SecondaryReadEnabled>secondary-read-indicator</SecondaryReadEnabled>
    <SecondaryEndpoints>
      <Endpoint>storage-secondary-service-blob-endpoint</Endpoint>
      <Endpoint>storage-secondary-service-queue-endpoint</Endpoint>
      <Endpoint>storage-secondary-service-table-endpoint</Endpoint>
    </SecondaryEndpoints>
    <AccountType>type-of-storage-account</AccountType>
  </StorageServiceProperties>
  <ExtendedProperties>
    <ExtendedProperty>
      <Name>property-name</Name>
      <Value>property-value</Value>
    </ExtendedProperty>
  </ExtendedProperties>
  <Capabilities>
    <Capability>storage-account-capability</Capability>
  </Capabilities>
</StorageService>";

        [TestMethod]
        public void ValidateSingleStorageAccount()
        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);

            var asmStorageAccountXml = new XmlDocument();
            asmStorageAccountXml.LoadXml(sampleAsmStorageInfo);
            var info = new Hashtable();
            info["name"] = "mystorage";
            fakeAsmRetriever.SetResponse("StorageAccount", info, asmStorageAccountXml);
            
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
