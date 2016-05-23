Select-AzureSubscription -SubscriptionId a97a235d-55f9-4382-856a-e38f8b5b6d31

$services = Get-AzureService | Where-Object {$_.ServiceName.StartsWith("r")}
foreach ($svc in $services)
{
    try
    {
        Write-Host "Attempting to remove service: $($svc.ServiceName)."
        Remove-AzureService -ServiceName $svc.ServiceName -Force -DeleteAll
        Write-Host "Success removing the service: $($svc.ServiceName)."
    }
    catch
    {
        Write-Host "Error removing the service: $($svc.ServiceName). Following exception was caught $($_.Exception.Message)" -ForegroundColor "Red"
    }
}
