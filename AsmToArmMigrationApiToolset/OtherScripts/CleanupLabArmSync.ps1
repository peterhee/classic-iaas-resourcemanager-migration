Select-AzureRmSubscription -SubscriptionId a97a235d-55f9-4382-856a-e38f8b5b6d31

$rgs = Get-AzureRmResourceGroup | Where-Object {$_.ResourceGroupName.StartsWith("u") -and $_.ResourceGroupName.EndsWith("-Migrated")}
foreach ($resource in $rgs)
{
    try
    {
        Write-Host "Attempting to remove resource group: $($resource.ResourceGroupName)."
        Remove-AzureRmResourceGroup -Name $resource.ResourceGroupName -Force
        Write-Host "Success removing the resource group: $($resource.NamResourceGroupNamee)."
    }
    catch
    {
        Write-Host "Error removing the resource group: $($resource.ResourceGroupName). Following exception was caught $($_.Exception.Message)" -ForegroundColor "Red"
    }
}
