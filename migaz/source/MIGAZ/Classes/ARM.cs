using System.Collections;
using System.Collections.Generic;
using System;

namespace MIGAZ.Classes.ARM
{
    public class VirtualNetworkGateway : Resource
    {
        public VirtualNetworkGateway()
        {
            type = "Microsoft.Network/virtualNetworkGateways";
            apiVersion = "2015-06-15";
        }
    }

    public class VirtualNetworkGateway_Properties
    {
        public List<IpConfiguration> ipConfigurations;
        public VirtualNetworkGateway_Sku sku;
        public string gatewayType; // VPN or ER
        public string vpnType; // RouteBased or PolicyBased
        public string enableBgp = "false";
        public VPNClientConfiguration vpnClientConfiguration;
    }

    public class VirtualNetworkGateway_Sku
    {
        public string name;
        public string tier;
    }

    public class LocalNetworkGateway : Resource
    {
        public LocalNetworkGateway()
        {
            type = "Microsoft.Network/localNetworkGateways";
            apiVersion = "2015-06-15";
        }
    }

    public class LocalNetworkGateway_Properties
    {
        public AddressSpace localNetworkAddressSpace;
        public string gatewayIpAddress;
    }

    public class GatewayConnection : Resource
    {
        public GatewayConnection()
        {
            type = "Microsoft.Network/connections";
            apiVersion = "2015-06-15";
        }
    }

    public class GatewayConnection_Properties
    {
        public Reference virtualNetworkGateway1;
        public Reference localNetworkGateway2;
        public string connectionType;
        public long routingWeight = 10;
        public string sharedKey;
    }

    public class VPNClientConfiguration
    {
        public AddressSpace vpnClientAddressPool;
        public List<VPNClientCertificate> vpnClientRootCertificates;
        public List<VPNClientCertificate> vpnClientRevokedCertificates;
    }

    public class VPNClientCertificate
    {
        public string name;
        public VPNClientCertificate_Properties properties;
    }

    public class VPNClientCertificate_Properties
    {
        public string PublicCertData;
        public string Thumbprint;
    }

    public class RouteTable : Resource
    {
        public RouteTable()
        {
            type = "Microsoft.Network/routeTables";
            apiVersion = "2015-06-15";
        }
    }

    public class RouteTable_Properties
    {
        public List<Route> routes;
    }

    public class Route
    {
        public string name;
        public Route_Properties properties;
    }

    public class Route_Properties
    {
        public string addressPrefix;
        public string nextHopType;
        public string nextHopIpAddress;
    }

    public class NetworkSecurityGroup : Resource
    {
        public NetworkSecurityGroup()
        {
            type = "Microsoft.Network/networkSecurityGroups";
            apiVersion = "2015-06-15";
        }
    }

    public class NetworkSecurityGroup_Properties
    {
        public List<SecurityRule> securityRules;
    }

    public class SecurityRule
    {
        public string name;
        public SecurityRule_Properties properties;
    }

    public class SecurityRule_Properties
    {
        public string description;
        public string protocol;
        public string sourcePortRange;
        public string destinationPortRange;
        public string sourceAddressPrefix;
        public string destinationAddressPrefix;
        public string access;
        public long priority;
        public string direction;
    }

    public class VirtualNetwork : Resource
    {
        public VirtualNetwork()
        {
            type = "Microsoft.Network/virtualNetworks";
            apiVersion = "2015-06-15";
        }
    }

    public class AddressSpace
    {
        public List<string> addressPrefixes;
    }

    public class VirtualNetwork_Properties
    {
        public AddressSpace addressSpace;
        public List<Subnet> subnets;
        public VirtualNetwork_dhcpOptions dhcpOptions;
    }

    public class Subnet
    {
        public string name;
        public Subnet_Properties properties;
    }

    public class Subnet_Properties
    {
        public string addressPrefix;
        public Reference networkSecurityGroup;
        public Reference routeTable;
    }

    public class VirtualNetwork_dhcpOptions
    {
        public List<string> dnsServers;
    }

    public class StorageAccount : Resource
    {
        public StorageAccount()
        {
            type = "Microsoft.Storage/storageAccounts";
            apiVersion = "2015-06-15";
        }
    }

    public class StorageAccount_Properties
    {
        public string accountType;
    }

    public class LoadBalancer : Resource
    {
        public LoadBalancer()
        {
            type = "Microsoft.Network/loadBalancers";
            apiVersion = "2015-06-15";
        }
    }

    public class LoadBalancer_Properties
    {
        public List<FrontendIPConfiguration> frontendIPConfigurations;
        public List<Hashtable> backendAddressPools;
        public List<InboundNatRule> inboundNatRules;
        public List<LoadBalancingRule> loadBalancingRules;
        public List<Probe> probes;
    }

    public class FrontendIPConfiguration
    {
        public string name = "default";
        public FrontendIPConfiguration_Properties properties;
    }

    public class FrontendIPConfiguration_Properties
    {
        public Reference publicIPAddress;
        public string privateIPAllocationMethod;
        public string privateIPAddress;
        public Reference subnet;
    }

    public class InboundNatRule
    {
        public string name;
        public InboundNatRule_Properties properties;
    }

    public class InboundNatRule_Properties
    {
        public long frontendPort;
        public long backendPort;
        public string protocol;
        public Reference frontendIPConfiguration;
    }

    public class LoadBalancingRule
    {
        public string name;
        public LoadBalancingRule_Properties properties;
    }

    public class LoadBalancingRule_Properties
    {
        public Reference frontendIPConfiguration;
        public Reference backendAddressPool;
        public Reference probe;
        public string protocol;
        public long frontendPort;
        public long backendPort;
        public long idleTimeoutInMinutes = 15;
        public string loadDistribution = "SourceIP";
        public bool enableFloatingIP = false;
    }

    public class Probe
    {
        public string name;
        public Probe_Properties properties;
    }

    public class Probe_Properties
    {
        public string protocol;
        public long port;
        public long intervalInSeconds = 15;
        public long numberOfProbes = 2;
        public string requestPath;
    }

    public class PublicIPAddress : Resource
    {
        public PublicIPAddress()
        {
            type = "Microsoft.Network/publicIPAddresses";
            apiVersion = "2015-06-15";
        }
    }

    public class PublicIPAddress_Properties
    {
        public string publicIPAllocationMethod = "Dynamic";
        public Hashtable dnsSettings;
    }

    public class NetworkInterface : Resource
    {
        public NetworkInterface()
        {
            type = "Microsoft.Network/networkInterfaces";
            apiVersion = "2015-06-15";
        }
    }

    public class NetworkInterface_Properties
    {
        public List<IpConfiguration> ipConfigurations;
        public bool enableIPForwarding = false;
        public Reference NetworkSecurityGroup;
    }

    public class IpConfiguration
    {
        public string name;
        public IpConfiguration_Properties properties;
    }

    public class IpConfiguration_Properties
    {
        public string privateIPAllocationMethod = "Dynamic";
        public string privateIPAddress;
        public Reference publicIPAddress;
        public Reference subnet;
        public List<Reference> loadBalancerBackendAddressPools;
        public List<Reference> loadBalancerInboundNatRules;
    }

    public class AvailabilitySet : Resource
    {
        public AvailabilitySet()
        {
            type = "Microsoft.Compute/availabilitySets";
            apiVersion = "2015-06-15";
        }
    }

    public class VirtualMachine : Resource
    {
        public List<Resource> resources;
        public VirtualMachine()
        {
            type = "Microsoft.Compute/virtualMachines";
            apiVersion = "2015-06-15";
        }
    }

    public class VirtualMachine_Properties
    {
        public HardwareProfile hardwareProfile;
        public Reference availabilitySet;
        public OsProfile osProfile;
        public StorageProfile storageProfile;
        public NetworkProfile networkProfile;
        public DiagnosticsProfile diagnosticsProfile;
    }

    public class HardwareProfile
    {
        public string vmSize;
    }

    public class OsProfile
    {
        public string computerName;
        public string adminUsername;
        public string adminPassword;
    }

    public class StorageProfile
    {
        public ImageReference imageReference;
        public OsDisk osDisk;
        public List<DataDisk> dataDisks;
    }

    public class ImageReference
    {
        public string publisher;
        public string offer;
        public string sku;
        public string version;
    }

    public class OsDisk
    {
        public string name;
        public string osType;
        public Vhd vhd;
        public string caching;
        public string createOption;
    }

    public class DataDisk
    {
        public string name;
        public Vhd vhd;
        public string caching;
        public string createOption;
        public long diskSizeGB;
        public long lun;
    }

    public class Vhd
    {
        public string uri;
    }

    public class NetworkProfile
    {
        public List<NetworkProfile_NetworkInterface> networkInterfaces;
    }

    public class NetworkProfile_NetworkInterface
    {
        public string id;
        public NetworkProfile_NetworkInterface_Properties properties;
    }

    public class NetworkProfile_NetworkInterface_Properties
    {
        public bool primary = true;
    }

    public class DiagnosticsProfile
    {
        public BootDiagnostics bootDiagnostics;
    }

    public class BootDiagnostics
    {
        public bool enabled;
        public string storageUri;
    }

    public class Reference
    {
        public string id;
    }

    public class Resource
    {
        public string type;
        public string apiVersion;
        public string name;
        public string location = "[resourceGroup().location]";
        public Dictionary<string, string> tags;
        public List<string> dependsOn;
        public object properties;

        public Resource()
        {
            if (app.Default.AllowTag)
            {
                tags = new Dictionary<string, string>();
                tags.Add("migAz", app.Default.ExecutionId);
            }
        }
    }

    public class Parameter
    {
        public string type;
    }

    public class Template
    {
        public string schemalink = "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
        public string contentVersion = "1.0.0.0";
        public Dictionary<string, Parameter> parameters;
        public Dictionary<string, string> variables;
        public List<Resource> resources;
    }
}
