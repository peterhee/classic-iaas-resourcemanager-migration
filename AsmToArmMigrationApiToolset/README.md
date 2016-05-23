ASM to ARM Migration API Test Toolset


Background

This document describes a validation and testing approach to ensure a customer’s virtual network and contained VMs can cleanly migrate from ASM to ARM using the new ARM Migration API.


Overview

This document will cover the steps and tools for validating and testing customer’s actual data to ensure a smooth migration to ARM.  There’s a series of tools involved that can be used for either of the two purposes below:
1.	Validate the metadata from the customer’s virtual network and identify any migration problem areas.
2.	Use the customer’s metadata to create and build a simulated test environment in a different Azure subscription to test the migration API prior to migration. 


Summary of the Tools and Workflow

The high level workflow is as follows:
•	Collect Metadata: A customer will do the following 3 things to obtain the necessary metadata from their production ASM environment to be migrated:
1.	Run a PowerShell script called MetadataExtract.ps1, which will extract all of the ASM cloud service deployments in a subscription to XML. This is the most important required step and needed to identify any potential incompatibility areas. Steps 2 and 3 below are only needed if a simulated test environment will be hydrated using the metadata for actual testing of the ARM Migration API.
2.	Export out the network configuration for the subscription that contains the virtual networks for migration. This step must be done using the Azure portal. There is no cmdlet to export the network configuration.
3.	Run a PowerShell script called NSG-Bindings.ps1 to extract the NSG’s (network security groups and rules) for the virtual network.
•	Parse Metadata: Using the XML output from 1 above from MetadataExtract, run an application called AsmMetadataParser.exe with the metadata xml and the name of the virtual network to migrate.  This app will generate a flattened .CSV from the metadata with the most critical data needed for building a test environment. It will also generate a report with any compatibility concerns flagged. The C# source code is included for this tool.
•	Simulation Test: To build and test the Migration API from the metadata, the following 4 steps are performed:
1.	Build the virtual network. Import the network config to recreate the vNet, subnets, and IP address space just like the customer environment. Note: Remove any gateways before importing the network config as gateways must be removed for migrating.
2.	Run the HydrateNSG.ps1 PowerShell script to create the NSGs, rules, and associate the NSGs to any subnets in the virtual network.
3.	Run the HydrateLab.ps1 PowerShell script to create the simulation environment from the .CSV generated from AsmMetadataParser.exe.  
4.	Now run the Move-AzureVirtualNetwork PowerShell scripts that are part of the Migration API to migrate the test environment to ARM.
	While migrating, another PowerShell script from this toolset called GetMigrationStatus.ps1 can be executed in a different PowerShell session to check the status of migration.
	Here is the blog to that explains how to use the new ARM Migration API.  https://azure.microsoft.com/en-us/blog/iaas-migration-classic-resource-manager/


The following is the list of contained tools in this toolset.

MetadataExtract.ps1:	Extract the deployment metadata from a production Azure subscription running ASM virtual networks. Output is a single XML file.
NSG-Bindings.ps1:	Extract the Network Security Groups and rules for an ASM virtual network.
AsmMetadataParser.exe:	Parse the metadata from MetadataExtract, and build a flattened CSV and a compatibility report.
HydrateNSG.ps1:	Recreate the associated NSGs and rules in a simulated lab virtual network that matches the production virtual network. The vNet is built from the exported network configuration file, and this scripts add NSGs and rules to the vNet. 
HydrateLab.ps1:	Use the CSV to hydrate a simulated ASM environment with the same metadata for running/testing the ARM Migration API.
GetMigrationStatus.ps1:	Retrieves the status of an actual ARM migration using the Migration API Move-AzureVirtualNetwork cmdlet.  This will show each cloud service as its being prepared and committed to ARM.
ShutdownLab.ps1, CleanupLabASM.ps1, CleanupLabARM.ps1.	Useful scripts to shut down and clean-up a test lab. 


Toolset Execution Details

The following contains details for running each tool in the process.

Metadata Extract
Send the customer the MetadataExtract.zip file containing the PowerShell scripts. You may not be able to send this over email given it contains scripts, so better to have them download it. Here are the steps to run this script.
1.	Copy MetadataExtract.zip to a folder on your machine that can run PowerShell.
2.	Unzip to some folder (doesn’t matter where)
3.	Open an instance of PowerShell ISE
4.	Add-AzureAccount (login with credentials). You will need to be a co-admin on the subscriptions you export.
5.	Change directory over to the unzipped files.
6.	Execute Install-ARMModule.ps1.  
a.	.\Install-ARMModule.ps1
7.	Execute MetadataExtract.ps1 for the subscription to export (you may get re-prompted for credentials…that’s expected).
a.	Example  .\MetadataExtract.ps1 -subscriptionID subidguid
8.	Grab the generated xml file for each run.  The filename will be metadata_subcriptionid.xml. It may take a couple of min to run for each subscription.  
a.	IMPORTANT. If the generated XML file contains an empty set of <deployments> elements, run the script again. The second time you run it from the same PS session, you should not be re-prompted for credentials, and it should run successfully and dump out all of the production deployments.
9.	The exported file will have all the necessary metadata to analyze.  The other Migration API test tools will require this file.

NSG Bindings
To extract NSG’s and rules, first modify the script (NSG-Bindings.ps1) with the subnets from the vNet to be migrated. Follow the example in the PowerShell script. There is no api to retrieve the ASM subnets, and the subnet names are needed to query and see which NSG’s are associated with each subnet. The subnet names can be obtained from the exported network config file.

AsmMetadataParser
Run this utility app to parse through the xml extract the import data to a CSV.  The CSV will be used later by HydrateLab.ps1 to build a simulated test environment that replicates the vNet to be migrated. The source code is included, so additional fields can be pulled or other changes can be made.  
To use the utility, just run “asmmetadataparser.exe vnetName xmlFile” from a command window.  The app will output two files, the CSV and also a compatibility report text file. The report will list out potential problem areas that will need to be resolved before ARM migration. The following list are typical compatibility problems that the parser will identify.
•	Availability Sets. To be migrated to ARM, an ASM cloud service’s contained VMs must all be in one availability set, or the VMs must all not be in any availability set. More than one availability set in the cloud service is incompatible.  Additionally, there cannot be some VMs in an availability set, and some VMs not in an availability set.  
•	Web/Worker Role deployments.  Cloud Services containing web and worker roles cannot migrate to ARM. The web/worker roles must first be removed from the vNet.  A typical solution is to move web/worker role instances to a separate ASM vNet that is also linked to an ExpressRoute circuit, or to migrate these to App Service (this discussion is beyond the scope of this document). 
•	Cloud Services that contain some VMs with just one network interface card (NIC), and some VMs with more than one NIC.  It is possible to have cloud services with both types of VMs, and these must first be moved to either all single or multi NIC.  The CSV has a column (mixedmodenics) to identify when this is the case. If any VM has the mixedmodenics field set as “true”, the cloud service will not migrate until the NIC issue is resolved.  

HydrateNSG
The first step to build a simulation test environment is to create the vNet in a test Azure subscription. Import the networkconfig.xml file (exported from the production vNet to be migrated). Once the vNet is created, now create NSGs and rules. The HydrateNSG.ps1 script will take NSG metadata and recreate the NSGs and rules.  You will need to create a top level script to execute HydrateNSG.ps1 for each subnet in the vNet.  Example below of a top level script that calls HydrateNSG.ps1 for a subnet with the NSG named “Prod_SG.”
.\hydrateNSG.ps1 -SubscriptionID zzzzzzzz-55f9-4382-856a-e38f8b5b6d31 -VirtualNetworkName Network1 -SubnetName prod_app -region "East US" -NSG_Name Prod_SG -FilePath "C:\Users\cc\Prod_SG_zzzzzzzz-59c6-4d9c-94e5-344c8bd454ab.csv"

HydrateLab
This is the PowerShell script that will take the CSV as input and build the lab. The script will build all of the VMs in the CSV as described by the metadata. Make sure the test subscription has the defined capacity in the region for testing before running the script (both cores and cloud services). HydrateLab will handle single NIC, multi NIC, custom VM extension, internal load balancers, external load balancers, various VM sizes, data disks, etc.  To make it do something additional, just modify the C# AsmMetadataParser source code and/or the HydrateLab.ps1 script.   
Expect each VM will take between 5-10 minutes to create and start.  Depending on the number of VMs in the CSV, a best practice is to break the CSV into multiple files.  The CSV is organized by cloud service, so if breaking the CSV into multiple files, keep each cloud service in the same file to avoid a contention issue.  With multiple CSV files, multiple sessions of HydrateLab.ps1 can be executed in parallel, and the lab can hydrate much faster.  Works well.

GetMigrationStatus 
Once the simulation environment is created, it can be migrated to ARM using the Move-AzureVirtualNetwork cmdlets. When “prepare” and “commit” are executed with this cmdlet, status can be checked using the GetMigrationStatus.ps1 script.  This script calls the REST api and is asynchronous.  It must be executed in a different PowerShell session because Move-AzureVirtualNetwork cmdlet is synchronous and will not end until prepare/commit/abort is completed.  Hence, GetMigrationStatus is very useful to see the progress of each cloud service.  The status will change from “Not Prepared” to “Preparing” to “Prepared.”  During the Commit phase, the status will change from “Prepared” to “Committing.”  Once commit is completed, there will not be any migration jobs occurring (expected), and it may return an exception (ignore this).  
To tune this script, look in the source for the TODO comment and modify the script to just return the cloud services being migrated (rather than all cloud services in the subscription).  

Other Scripts
After migrating a vNet to ARM, a bunch of VMs are now running that need to be validated then deleted. A useful script called DeleteResourceGroups.ps1 is included that calls the REST api asynchronously to delete each migrated ARM resource group. The script needs to be tweaked for each usage.  Look at the source.
A few other scripts are included for deleting the hydrated ASM test lab (if it isn’t migrated to ARM), and/or shutdown a test lab.
