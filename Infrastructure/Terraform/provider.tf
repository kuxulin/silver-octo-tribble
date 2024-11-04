provider "azurerm" {
  features {
  }
  subscription_id            = "16e81eeb-07e0-4b61-9063-3669ab2b3ad4"
  environment                = "public"
  use_msi                    = false
  use_cli                    = true
  use_oidc                   = false
  skip_provider_registration = true
}
