terraform {
  backend "azurerm" {
    storage_account_name = "__StorageAccountName__"
    container_name       = "__ContainerName__"
    key                  = "__KeyName__" 
    access_key = "__StorageKey__"
  }

  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "3.99.0"
    }
  }
}