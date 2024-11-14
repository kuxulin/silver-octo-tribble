param (
    [string]$ResourceGroupName,
    [string]$StorageAccountName
)

$key = (Get-AzStorageAccountKey -ResourceGroupName $ResourceGroupName -AccountName $StorageAccountName).Value[0]

Write-Host "##vso[task.setvariable variable=StorageKey]$key"
