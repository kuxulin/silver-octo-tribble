data "azurerm_resource_group" "res-0" {
  name     = var.resource_group_name
}

data "azurerm_client_config" "current" {}

resource "azurerm_key_vault" "res-1" {
  location                  = var.resource_group_location
  name                      = var.key_vault_name
  resource_group_name       = var.resource_group_name
  sku_name                  = "standard"
  tenant_id                 = data.azurerm_client_config.current.tenant_id

}

resource "azurerm_key_vault_access_policy" "default" {
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id
  key_vault_id = azurerm_key_vault.res-1.id

  secret_permissions = [
    "Get", "List",  "Set", "Delete", "Recover", "Backup", "Restore"
  ]
}

resource "azurerm_key_vault_access_policy" "server_policy" {
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_windows_web_app.server.id
  key_vault_id = azurerm_key_vault.res-1.id

  secret_permissions = [
    "Get", 
  ]
}

resource "azurerm_key_vault_secret" "database_connection_string" {
  name         = var.connection_string_name
  value        = var.connection_string_value
  key_vault_id = azurerm_key_vault.res-1.id

  depends_on = [ azurerm_key_vault_access_policy.default ]
}

resource "azurerm_mssql_server" "res-2" {
  administrator_login          = var.admin_username
  administrator_login_password = var.database_admin_password_value
  location                      = var.resource_group_location
  name                          = var.sql_server_name
  resource_group_name           = var.resource_group_name
  version                       = "12.0"
}

resource "azurerm_mssql_firewall_rule" "res-42" {
  end_ip_address   = "0.0.0.0"
  name             = "AllowAllWindowsAzureIps"
  server_id        = azurerm_mssql_server.res-2.id
  start_ip_address = "0.0.0.0"
  depends_on = [
    azurerm_mssql_server.res-2,
  ]
}

resource "azurerm_mssql_firewall_rule" "res-44" {
  end_ip_address   = var.default_ip_adress
  name             = "default"
  server_id        = azurerm_mssql_server.res-2.id
  start_ip_address = var.default_ip_adress
  depends_on = [
    azurerm_mssql_server.res-2,
  ]
}

resource "azurerm_mssql_firewall_rule" "res-43" {
  end_ip_address   = var.default_ip_adress
  name             = "default"
  server_id        = azurerm_mssql_server.res-2.id
  start_ip_address = var.default_ip_adress
  depends_on = [
    azurerm_mssql_server.res-2,
  ]
}

resource "azurerm_mssql_database" "res-12" {
  name                 = var.sql_db_name
  server_id            = azurerm_mssql_server.res-2.id
}

resource "azurerm_service_plan" "res-45" {
  location            = var.resource_group_location
  name                = "ASP-${var.resource_group_name}-90ff"
  os_type             = "Windows"
  resource_group_name = var.resource_group_name
  sku_name            = "F1"
}

resource "azurerm_windows_web_app" "client" {
   app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY  = azurerm_application_insights.res-57.instrumentation_key
    WEBSITE_ENABLE_SYNC_UPDATE_SITE = "true"
    WEBSITE_RUN_FROM_PACKAGE        = "1"
  }
  client_affinity_enabled                        = true
  ftp_publish_basic_authentication_enabled       = false
  https_only                                     = true
  location                                       = var.resource_group_location
  name                                           = var.client-app-name
  resource_group_name                            = var.resource_group_name
  service_plan_id                                = azurerm_service_plan.res-45.id
  webdeploy_publish_basic_authentication_enabled = false
  
  site_config {
    always_on                         = false
    ftps_state                        = "FtpsOnly"
    ip_restriction_default_action = "Deny"
    virtual_application {
      physical_path = "site\\wwwroot\\client\\browser"
      preload       = false
      virtual_path  = "/"
    }
    ip_restriction {
      ip_address = "${var.default_ip_adress}/32"
      priority   = 300
    }
  }
}

resource "azurerm_windows_web_app" "server" {
   app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.res-57.instrumentation_key
    AZURE_KEYVAULT_RESOURCEENDPOINT = azurerm_key_vault.res-1.vault_uri
  }

  client_affinity_enabled                        = true
  ftp_publish_basic_authentication_enabled       = false
  https_only                                     = true
  location                                       = var.resource_group_location
  name                                           = var.server-app-name
  resource_group_name                            = var.resource_group_name
  service_plan_id                                = azurerm_service_plan.res-45.id
  webdeploy_publish_basic_authentication_enabled = false

  connection_string {
    name  = var.connection_string_name
    type  = "SQLAzure"
    value = azurerm_key_vault_secret.database_connection_string.value
  }

  identity {
    type = "SystemAssigned"
  }

  site_config {
    always_on                         = false
    ftps_state                        = "FtpsOnly"
    ip_restriction_default_action = "Deny"
    virtual_application {
      physical_path = "site\\wwwroot"
      preload       = false
      virtual_path  = "/"
    }
    ip_restriction {
      ip_address = "${var.default_ip_adress}/32"
      priority   = 300
    }
  }
}

resource "azurerm_app_service_connection" "server_database" {
  name               = "server_database"
  app_service_id     = azurerm_windows_web_app.server.id
  target_resource_id = azurerm_mssql_database.res-12.id
  client_type = "dotnet"
  authentication {
    type = "secret"
    secret = var.connection_string_value
    name = var.connection_string_name
  }
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
}

resource "azurerm_application_insights" "res-57" {
  application_type    = "web"
  location            = var.resource_group_location
  name                = "app-insights"
  resource_group_name = var.resource_group_name
}