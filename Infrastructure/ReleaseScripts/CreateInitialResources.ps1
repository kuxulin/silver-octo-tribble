param (
    [string]$Location,
    [string]$ResourceGroupName,
    [string]$StorageAccountName,
    [string]$DefaultContainerName
)

az group create --location $Location --name $ResourceGroupName

az storage account create --name $StorageAccountName --resource-group $ResourceGroupName --location $Location --sku Standard_LRS

az storage container create --name $DefaultContainerName --account-name $StorageAccountName
