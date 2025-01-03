data "azurerm_resource_group" "res-0" {
  name     = var.resource_group_name
}

data "azurerm_client_config" "current" {}

data "azurerm_storage_account" "storage" {
  resource_group_name = data.azurerm_resource_group.res-0.name
  name = var.storage_account_name
}

locals  {
  default_ip_adress_start = "${var.default_ip_adress}.0"
  default_ip_adress_end = "${var.default_ip_adress}.255"
}

resource "azurerm_storage_container" "images" {
  name = var.storage_container_images_name
  storage_account_name = data.azurerm_storage_account.storage.name
}

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
  object_id    = azurerm_windows_web_app.server.identity[0].principal_id
  key_vault_id = azurerm_key_vault.res-1.id

  secret_permissions = [
    "Get", 
  ]
}

resource "azurerm_key_vault_access_policy" "image_funcs_policy" {
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_windows_function_app.images_azure_funcs.identity[0].principal_id
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

resource "azurerm_key_vault_secret" "jwt_symmetric_key" {
  name         = "JWTKey"
  value        = var.jwt_key_value
  key_vault_id = azurerm_key_vault.res-1.id

  depends_on = [ azurerm_key_vault_access_policy.default ]
}

resource "azurerm_key_vault_secret" "storage_connection_string" {
  name         = var.storage_connection_name
  value        = var.storage_connection_value
  key_vault_id = azurerm_key_vault.res-1.id

  depends_on = [ azurerm_key_vault_access_policy.default ]
}

resource "azurerm_key_vault_secret" "web_jobs_storage_connection_string" {
  name =   var.azure_jobs_storage_name
  value = var.azure_jobs_storage_value
  key_vault_id = azurerm_key_vault.res-1.id
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
  start_ip_address = local.default_ip_adress_start
  end_ip_address   = local.default_ip_adress_end
  name             = "default"
  server_id        = azurerm_mssql_server.res-2.id
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
  sku_name            = "B1"
}

resource "azurerm_windows_web_app" "client" {
  app_settings = {
    WEBSITE_ENABLE_SYNC_UPDATE_SITE = "true"
    WEBSITE_RUN_FROM_PACKAGE        = "1"
    ServerName  = "${azurerm_windows_web_app.server.name}.azurewebsites.net"
    APPINSIGHTS_INSTRUMENTATIONKEY =  azurerm_application_insights.res-57.instrumentation_key
    APPLICATIONINSIGHTS_CONNECTION_STRING =  azurerm_application_insights.res-57.connection_string
  }

  client_affinity_enabled                        = true
  ftp_publish_basic_authentication_enabled       = false
  https_only                                     = true
  location                                       = var.resource_group_location
  name                                           = var.client_app_name
  resource_group_name                            = var.resource_group_name
  service_plan_id                                = azurerm_service_plan.res-45.id
  webdeploy_publish_basic_authentication_enabled = false
  
  site_config {
    always_on                         = false
    ftps_state                        = "FtpsOnly"
    virtual_application {
      physical_path = "site\\wwwroot\\client\\browser"
      preload       = false
      virtual_path  = "/"
    }
  }

  depends_on = [ azurerm_windows_web_app.server ]
}

resource "azurerm_windows_web_app" "server" { 
  app_settings = {
    ClientName = "${var.client_app_name}.azurewebsites.net"
    ServerName  = "${var.server_app_name}.azurewebsites.net"
    "${var.jwt_key_name}" = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault.res-1.vault_uri}secrets/${azurerm_key_vault_secret.jwt_symmetric_key.name}/${azurerm_key_vault_secret.jwt_symmetric_key.version})"
    ImagesContainerName = var.storage_container_images_name
    APPINSIGHTS_INSTRUMENTATIONKEY =  azurerm_application_insights.res-57.instrumentation_key
    APPLICATIONINSIGHTS_CONNECTION_STRING =  azurerm_application_insights.res-57.connection_string
    XDT_MicrosoftApplicationInsights_Mode           = "recommended"
    ApplicationInsightsAgent_EXTENSION_VERSION      = "~2"
    APPINSIGHTS_PROFILERFEATURE_VERSION             = "1.0.0"
    APPINSIGHTS_SNAPSHOTFEATURE_VERSION             = "1.0.0"
  }

  client_affinity_enabled                        = true
  ftp_publish_basic_authentication_enabled       = false
  https_only                                     = true
  location                                       = var.resource_group_location
  name                                           = var.server_app_name
  resource_group_name                            = var.resource_group_name
  service_plan_id                                = azurerm_service_plan.res-45.id
  webdeploy_publish_basic_authentication_enabled = false

  connection_string {
    name  = var.connection_string_name
    type  = "SQLAzure"
    value = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault.res-1.vault_uri}secrets/${azurerm_key_vault_secret.database_connection_string.name}/${azurerm_key_vault_secret.database_connection_string.version})"
  }

  connection_string {
    name = var.storage_connection_name
    type = "Custom"
    value = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault.res-1.vault_uri}secrets/${azurerm_key_vault_secret.storage_connection_string.name}/${azurerm_key_vault_secret.storage_connection_string.version})"
  }

  identity {
    type = "SystemAssigned"
  }

  site_config {
    always_on                         = false
    ftps_state                        = "FtpsOnly"
    virtual_application {
      physical_path = "site\\wwwroot"
      preload       = false
      virtual_path  = "/"
    }
  }
}

resource "azurerm_app_service_connection" "server_key_vault" {
  name               = "server_keyvault"
  app_service_id     = azurerm_windows_web_app.server.id
  target_resource_id = azurerm_key_vault.res-1.id
  client_type = "dotnet"
  authentication {
    type = "systemAssignedIdentity"
  }
}

resource "azurerm_app_service_connection" "server_database" {
  name               = "server_database"
  app_service_id     = azurerm_windows_web_app.server.id
  target_resource_id = azurerm_mssql_database.res-12.id
  client_type = "dotnet"
  authentication {
    type = "secret"
    secret = var.database_admin_password_value
    name = var.admin_username
  }
  depends_on = [ azurerm_app_service_connection.server_key_vault ]
}

resource "azurerm_app_service_connection" "server_storage" {
  name               = "server_storage"
  app_service_id     = azurerm_windows_web_app.server.id
  target_resource_id = data.azurerm_storage_account.storage.id
  client_type = "dotnet"
  authentication {
    type = "secret"
  }
  depends_on = [ azurerm_app_service_connection.server_key_vault ]
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

resource "azurerm_windows_function_app" "images_azure_funcs" {
  app_settings = {
    AzureWebJobsSecretStorageType          = "files"
    ImagesContainerName                    = var.storage_container_images_name
    WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED = "1"
    "${var.azure_jobs_storage_name}" = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault.res-1.vault_uri}secrets/${azurerm_key_vault_secret.web_jobs_storage_connection_string.name}/${azurerm_key_vault_secret.web_jobs_storage_connection_string.version})"
  }
  client_certificate_mode                  = "Required"
  ftp_publish_basic_authentication_enabled = false
  location                                 = var.resource_group_location
  name                                     = var.upload_azure_func_name
  resource_group_name                      = var.resource_group_name
  service_plan_id                          = azurerm_service_plan.res-45.id
  storage_account_name = data.azurerm_storage_account.storage.name
  webdeploy_publish_basic_authentication_enabled = false

  identity {
    type = "SystemAssigned"
  }

  connection_string {
    name = var.storage_connection_name
    type = "Custom"
    value = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault.res-1.vault_uri}secrets/${azurerm_key_vault_secret.storage_connection_string.name}/${azurerm_key_vault_secret.storage_connection_string.version})"
  }

  site_config {
    application_insights_connection_string = azurerm_application_insights.res-57.connection_string
    ftps_state                             = "FtpsOnly"
    use_32_bit_worker                      = false
    cors {
      allowed_origins = ["https://portal.azure.com"]
    }
  }
  depends_on = [
    azurerm_application_insights.res-57
  ]
}

resource "azurerm_app_service_connection" "azure_upload_func_key_vault" {
  name               = "images_azure_funcs_keyvault"
  app_service_id     = azurerm_windows_function_app.images_azure_funcs.id
  target_resource_id = azurerm_key_vault.res-1.id
  client_type = "dotnet"
  authentication {
    type = "systemAssignedIdentity"
  }
}
