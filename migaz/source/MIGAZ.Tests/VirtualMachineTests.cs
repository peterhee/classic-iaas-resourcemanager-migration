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
        private const string sampleCloudServiceInfo = @"<HostedService>
    <Url>https://management.core.windows.net/f2175d43-ac02-49b4-9262-47db12ec799b/services/hostedservices/myservice</Url>
    <ServiceName>myservice</ServiceName>
    <HostedServiceProperties>
        <Description>myservice</Description>
        <Location>Australia East</Location>
        <Label>dG9tc2NvbnRhaW5lcnM=</Label>
        <Status>Created</Status>
        <DateCreated>2015-10-18T22:24:43Z</DateCreated>
        <DateLastModified>2015-12-06T22:12:24Z</DateLastModified>
        <ExtendedProperties>
            <ExtendedProperty>
                <Name>ResourceGroup</Name>
                <Value>Group-2</Value>
            </ExtendedProperty>
            <ExtendedProperty>
                <Name>ResourceLocation</Name>
                <Value>australiaeast</Value>
            </ExtendedProperty>
            <ExtendedProperty>
                <Name>ProvisioningSource</Name>
                <Value>AzureResourceManager</Value>
            </ExtendedProperty>
        </ExtendedProperties>
    </HostedServiceProperties>
    <Deployments>
        <Deployment>
            <Name>myservice</Name>
            <DeploymentSlot>Production</DeploymentSlot>
            <PrivateID>901de0aeaec54df38f7a9d421f991e0b</PrivateID>
            <Status>Suspended</Status>
            <Label>dG9tc2NvbnRhaW5lcnM=</Label>
            <Url>http://myservice.cloudapp.net/</Url>
            <Configuration>PFNlcnZpY2VDb25maWd1cmF0aW9uIHhtbG5zOnhzZD0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIHhtbG5zPSJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL1NlcnZpY2VIb3N0aW5nLzIwMDgvMTAvU2VydmljZUNvbmZpZ3VyYXRpb24iPg0KICA8Um9sZSBuYW1lPSJ0b21zY29udGFpbmVycyI+DQogICAgPEluc3RhbmNlcyBjb3VudD0iMSIgLz4NCiAgPC9Sb2xlPg0KPC9TZXJ2aWNlQ29uZmlndXJhdGlvbj4=</Configuration>
            <RoleInstanceList>
                <RoleInstance>
                    <RoleName>myservice</RoleName>
                    <InstanceName>myservice</InstanceName>
                    <InstanceStatus>StoppedDeallocated</InstanceStatus>
                    <PowerState>Stopped</PowerState>
                    <RemoteAccessCertificateThumbprint>3ADEC63A54E7D197E95FD3A914E8A9987CC490BA</RemoteAccessCertificateThumbprint>
                </RoleInstance>
            </RoleInstanceList>
            <UpgradeDomainCount>1</UpgradeDomainCount>
            <RoleList>
                <Role>
                    <RoleName>myservice</RoleName>
                    <OsVersion/>
                    <RoleType>PersistentVMRole</RoleType>
                    <ConfigurationSets>
                        <ConfigurationSet>
                            <ConfigurationSetType>NetworkConfiguration</ConfigurationSetType>
                            <InputEndpoints>
                                <InputEndpoint>
                                    <LocalPort>80</LocalPort>
                                    <Name>HTTP</Name>
                                    <Port>80</Port>
                                    <Protocol>tcp</Protocol>
                                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                                </InputEndpoint>
                                <InputEndpoint>
                                    <LocalPort>5004</LocalPort>
                                    <Name>HTTP 5004</Name>
                                    <Port>5004</Port>
                                    <Protocol>tcp</Protocol>
                                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                                </InputEndpoint>
                                <InputEndpoint>
                                    <LocalPort>8080</LocalPort>
                                    <Name>Raven</Name>
                                    <Port>8080</Port>
                                    <Protocol>tcp</Protocol>
                                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                                </InputEndpoint>
                                <InputEndpoint>
                                    <LocalPort>3389</LocalPort>
                                    <Name>Remote Desktop</Name>
                                    <Port>3389</Port>
                                    <Protocol>tcp</Protocol>
                                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                                </InputEndpoint>
                                <InputEndpoint>
                                    <LocalPort>5986</LocalPort>
                                    <Name>WinRM</Name>
                                    <Port>5986</Port>
                                    <Protocol>tcp</Protocol>
                                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                                </InputEndpoint>
                            </InputEndpoints>
                            <SubnetNames>
                                <SubnetName>Subnet-1</SubnetName>
                            </SubnetNames>
                        </ConfigurationSet>
                    </ConfigurationSets>
                    <ResourceExtensionReferences>
                        <ResourceExtensionReference>
                            <ReferenceName>Microsoft.EnterpriseCloud.Monitoring.MicrosoftMonitoringAgent</ReferenceName>
                            <Publisher>Microsoft.EnterpriseCloud.Monitoring</Publisher>
                            <Name>MicrosoftMonitoringAgent</Name>
                            <Version>1.*</Version>
                            <ResourceExtensionParameterValues>
                                <ResourceExtensionParameterValue>
                                    <Key>PublicConfiguration</Key>
                                    <Value>eyJXb3Jrc3BhY2VJZCI6ImVkMjg4N2Y4LWY2MDktNDliNC05NzU0LWE3NzUyYmZhYzg2ZiJ9</Value>
                                    <Type>Public</Type>
                                </ResourceExtensionParameterValue>
                            </ResourceExtensionParameterValues>
                            <State>Enable</State>
                        </ResourceExtensionReference>
                        <ResourceExtensionReference>
                            <ReferenceName>Microsoft.Compute.BGInfo</ReferenceName>
                            <Publisher>Microsoft.Compute</Publisher>
                            <Name>BGInfo</Name>
                            <Version>1.*</Version>
                            <State>Enable</State>
                        </ResourceExtensionReference>
                    </ResourceExtensionReferences>
                    <DataVirtualHardDisks/>
                    <OSVirtualHardDisk>
                        <HostCaching>ReadWrite</HostCaching>
                        <DiskName>myservice-myservice-os-1445207070064</DiskName>
                        <MediaLink>https://myservice.blob.core.windows.net/vhds/myservice-myservice-os-1445207070064.vhd</MediaLink>
                        <SourceImageName>a699494373c04fc0bc8f2bb1389d6106__WindowsServer_en-us_TP3_Container_VHD_Azure-20150901.vhd</SourceImageName>
                        <OS>Windows</OS>
                        <IOType>Provisioned</IOType>
                    </OSVirtualHardDisk>
                    <RoleSize>Standard_DS2</RoleSize>
                    <DefaultWinRmCertificateThumbprint>64484D1BC98BB4689CE49437009ADBB175B9F335</DefaultWinRmCertificateThumbprint>
                    <ProvisionGuestAgent>true</ProvisionGuestAgent>
                </Role>
            </RoleList>
            <SdkVersion/>
            <Locked>false</Locked>
            <RollbackAllowed>false</RollbackAllowed>
            <VirtualNetworkName>Group Group-2 myservice</VirtualNetworkName>
            <CreatedTime>2015-10-18T22:25:33Z</CreatedTime>
            <LastModifiedTime>2016-07-11T05:01:15Z</LastModifiedTime>
            <ExtendedProperties/>
            <PersistentVMDowntime>
                <StartTime>2016-07-06T10:25:28Z</StartTime>
                <EndTime>2017-07-06T10:25:28Z</EndTime>
                <Status>PersistentVMUpdateScheduled</Status>
            </PersistentVMDowntime>
            <VirtualIPs/>
            <InternalDnsSuffix>myservice.p8.internal.cloudapp.net</InternalDnsSuffix>
            <LoadBalancers/>
            <VirtualNetworkId>356c28e0-d474-4ea0-8441-94c3746a309d</VirtualNetworkId>
        </Deployment>
    </Deployments>
    <DefaultWinRmCertificateThumbprint>64484D1BC98BB4689CE49437009ADBB175B9F335</DefaultWinRmCertificateThumbprint>
</HostedService>
";

        private const string sampleVirtualMachineInfo = @"<PersistentVMRole>
    <RoleName>myservice</RoleName>
    <OsVersion/>
    <RoleType>PersistentVMRole</RoleType>
    <ConfigurationSets>
        <ConfigurationSet>
            <ConfigurationSetType>NetworkConfiguration</ConfigurationSetType>
            <InputEndpoints>
                <InputEndpoint>
                    <LocalPort>80</LocalPort>
                    <Name>HTTP</Name>
                    <Port>80</Port>
                    <Protocol>tcp</Protocol>
                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                </InputEndpoint>
                <InputEndpoint>
                    <LocalPort>5004</LocalPort>
                    <Name>HTTP 5004</Name>
                    <Port>5004</Port>
                    <Protocol>tcp</Protocol>
                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                </InputEndpoint>
                <InputEndpoint>
                    <LocalPort>8080</LocalPort>
                    <Name>Raven</Name>
                    <Port>8080</Port>
                    <Protocol>tcp</Protocol>
                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                </InputEndpoint>
                <InputEndpoint>
                    <LocalPort>3389</LocalPort>
                    <Name>Remote Desktop</Name>
                    <Port>3389</Port>
                    <Protocol>tcp</Protocol>
                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                </InputEndpoint>
                <InputEndpoint>
                    <LocalPort>5986</LocalPort>
                    <Name>WinRM</Name>
                    <Port>5986</Port>
                    <Protocol>tcp</Protocol>
                    <EnableDirectServerReturn>false</EnableDirectServerReturn>
                </InputEndpoint>
            </InputEndpoints>
            <SubnetNames>
                <SubnetName>Subnet-1</SubnetName>
            </SubnetNames>
        </ConfigurationSet>
    </ConfigurationSets>
    <ResourceExtensionReferences>
        <ResourceExtensionReference>
            <ReferenceName>Microsoft.EnterpriseCloud.Monitoring.MicrosoftMonitoringAgent</ReferenceName>
            <Publisher>Microsoft.EnterpriseCloud.Monitoring</Publisher>
            <Name>MicrosoftMonitoringAgent</Name>
            <Version>1.*</Version>
            <ResourceExtensionParameterValues>
                <ResourceExtensionParameterValue>
                    <Key>PublicConfiguration</Key>
                    <Value>eyJXb3Jrc3BhY2VJZCI6ImVkMjg4N2Y4LWY2MDktNDliNC05NzU0LWE3NzUyYmZhYzg2ZiJ9</Value>
                    <Type>Public</Type>
                </ResourceExtensionParameterValue>
            </ResourceExtensionParameterValues>
            <State>Enable</State>
        </ResourceExtensionReference>
        <ResourceExtensionReference>
            <ReferenceName>Microsoft.Compute.BGInfo</ReferenceName>
            <Publisher>Microsoft.Compute</Publisher>
            <Name>BGInfo</Name>
            <Version>1.*</Version>
            <State>Enable</State>
        </ResourceExtensionReference>
    </ResourceExtensionReferences>
    <DataVirtualHardDisks/>
    <OSVirtualHardDisk>
        <HostCaching>ReadWrite</HostCaching>
        <DiskName>myservice-myservice-os-1445207070064</DiskName>
        <MediaLink>https://myservice.blob.core.windows.net/vhds/myservice-myservice-os-1445207070064.vhd</MediaLink>
        <SourceImageName>a699494373c04fc0bc8f2bb1389d6106__WindowsServer_en-us_TP3_Container_VHD_Azure-20150901.vhd</SourceImageName>
        <OS>Windows</OS>
        <IOType>Provisioned</IOType>
    </OSVirtualHardDisk>
    <RoleSize>Standard_DS2</RoleSize>
    <DefaultWinRmCertificateThumbprint>64484D1BC98BB4689CE49437009ADBB175B9F335</DefaultWinRmCertificateThumbprint>
    <ProvisionGuestAgent>true</ProvisionGuestAgent>
</PersistentVMRole>";

        private string sampleStorageKeysInfo = @"<StorageService>
    <Url>storage-service-url</Url>
    <StorageServiceKeys>
      <Primary>primary-key</Primary>
      <Secondary>secondary-key</Secondary>
    </StorageServiceKeys>
  </StorageService>";
        
        private JObject GenerateSingleVMTemplate()

        {
            FakeAsmRetriever fakeAsmRetriever;
            TemplateGenerator templateGenerator;
            TestHelper.SetupObjects(out fakeAsmRetriever, out templateGenerator);

            var asmCloudServiceXml = new XmlDocument();
            asmCloudServiceXml.LoadXml(sampleCloudServiceInfo);
            var info = new Hashtable();
            info["name"] = "myservice";
            fakeAsmRetriever.SetResponse("CloudService", info, asmCloudServiceXml.SelectNodes("HostedService"));

            var asmVMXml = new XmlDocument();
            asmVMXml.LoadXml(sampleVirtualMachineInfo);
            info = new Hashtable();
            info["cloudservicename"] = "myservice";
            info["deploymentname"] = "myservice";
            info["virtualmachinename"] = "myservice";
            fakeAsmRetriever.SetResponse("VirtualMachine", info, asmVMXml.ChildNodes);

            var storageKeysXml = new XmlDocument();
            storageKeysXml.LoadXml(sampleStorageKeysInfo);
            info = new Hashtable();
            info["name"] = "myservice";
            fakeAsmRetriever.SetResponse("StorageAccountKeys", info, storageKeysXml.SelectNodes("StorageService"));

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
