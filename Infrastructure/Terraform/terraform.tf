terraform {
  backend "azurerm" {
    storage_account_name = var.storage_account_name
    container_name       = "tfcontainer"
    key                  = "terraform.tfstate" 
  }

  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "3.99.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~>3.0"
    }
  }
}