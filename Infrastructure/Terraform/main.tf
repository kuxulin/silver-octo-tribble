resource "azurerm_resource_group" "res-0" {
  location = var.resource_group_location
  name     = var.resource_group_name
}

resource "random_integer" "ri" {
  min = 10000
  max = 99999
}

resource "azurerm_key_vault" "res-1" {
  enable_rbac_authorization = true
  location                  = var.resource_group_location
  name                      = "default-key-vault1"
  resource_group_name       = var.resource_group_name
  sku_name                  = "standard"
  tenant_id                 = var.tenant_id
  depends_on = [azurerm_resource_group.res-0]
}

resource "azurerm_mssql_server" "res-2" {
  administrator_login          = var.admin_username
  administrator_login_password = var.admin_password
  location                      = var.resource_group_location
  name                          = "${var.sql_server_name}"
  resource_group_name           = var.resource_group_name
  version                       = "12.0"
  depends_on = [azurerm_resource_group.res-0]
}

resource "azurerm_mssql_database" "res-12" {
  name                 = var.sql_db_name
  server_id            = azurerm_mssql_server.res-2.id
  depends_on = [azurerm_resource_group.res-0]
}

resource "azurerm_service_plan" "res-45" {
  location            = var.resource_group_location
  name                = "ASP-${var.resource_group_name}-90ff"
  os_type             = "Windows"
  resource_group_name = var.resource_group_name
  sku_name            = "F1"
  depends_on = [azurerm_resource_group.res-0]
}

resource "azurerm_windows_web_app" "client" {
  client_affinity_enabled                        = true
  ftp_publish_basic_authentication_enabled       = false
  https_only                                     = true
  location                                       = var.resource_group_location
  name                                           = "${var.client-app-name}-${random_integer.ri.result}"
  public_network_access_enabled                  = false
  resource_group_name                            = var.resource_group_name
  service_plan_id                                = azurerm_service_plan.res-45.id
  webdeploy_publish_basic_authentication_enabled = false
  
  site_config {
    always_on                         = false
    ftps_state                        = "FtpsOnly"
    virtual_application {
      physical_path = "site\\wwwroot"
      preload       = false
      virtual_path  = "/"
    }
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.res-57.instrumentation_key
  }
  depends_on = [azurerm_resource_group.res-0]
}

resource "azurerm_windows_web_app" "server" {
  client_affinity_enabled                        = true
  ftp_publish_basic_authentication_enabled       = false
  https_only                                     = true
  location                                       = var.resource_group_location
  name                                           = "${var.server-app-name}-${random_integer.ri.result}"
  public_network_access_enabled                  = false
  resource_group_name                            = var.resource_group_name
  service_plan_id                                = azurerm_service_plan.res-45.id
  webdeploy_publish_basic_authentication_enabled = false

  site_config {
    always_on                         = false
    ftps_state                        = "FtpsOnly"
    virtual_application {
      physical_path = "site\\wwwroot"
      preload       = false
      virtual_path  = "/"
    }
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.res-57.instrumentation_key
  }
  depends_on = [azurerm_resource_group.res-0]
}

resource "azurerm_monitor_action_group" "res-56" {
  name                = "Application Insights Smart Detection"
  resource_group_name = var.resource_group_name
  short_name          = "SmartDetect"
  arm_role_receiver {
    name                    = "Monitoring Contributor"
    role_id                 = "749f88d5-cbae-40b8-bcfc-e573ddc772fa"
    use_common_alert_schema = true
  }
  arm_role_receiver {
    name                    = "Monitoring Reader"
    role_id                 = "43d0d8ad-25c7-4714-9337-8ba259a9fe05"
    use_common_alert_schema = true
  }
  depends_on = [azurerm_resource_group.res-0]
}

resource "azurerm_application_insights" "res-57" {
  application_type    = "web"
  location            = var.resource_group_location
  name                = "app-insights"
  resource_group_name = var.resource_group_name
  depends_on = [azurerm_resource_group.res-0]
}

# resource "azurerm_storage_account" "example" {
#   name                     = var.storage_account_name
#   resource_group_name      = var.resource_group_name
#   location                 = var.resource_group_location
#   account_tier             = "Standard"
#   account_replication_type = "GRS"
#   depends_on = [azurerm_resource_group.res-0]
# }