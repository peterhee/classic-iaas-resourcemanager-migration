Select-AzureSubscription -SubscriptionId a97a235d-55f9-4382-856a-e38f8b5b6d31

$services = Get-AzureService | Where-Object {$_.ServiceName.StartsWith("y")}
foreach ($svc in $services)
{
    try
    {
        $vms = Get-AzureVM -ServiceName $svc.ServiceName 
        foreach ($vm in $vms)
        {
            if ($vm.PowerState -ne "Stopped")
            {
                Write-Host "Stopping the vm: $($vm.Name)."
                Stop-AzureVM -ServiceName $svc.ServiceName -Name $vm.Name -Force
                Write-Host "Success stopping the vm: $($vm.Name)." -ForegroundColor "Green"
            }
        }
    }
    catch
    {
        Write-Host "Error stopping the vm: $($vm.Name). Following exception was caught $($_.Exception.Message)" -ForegroundColor "Red"
    }
}
