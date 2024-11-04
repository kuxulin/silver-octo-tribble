terraform {
  backend "local" {}

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