variable resource_group_location {
  type        = string
  description = "Location for all resources"
  default     = "__Location__"
}

variable resource_group_name {
  type        = string
  description = "Resource group name"
  default     = "__ResourceGroupName__"
}

variable storage_account_name {
  type = string
  description = "A name of storage account"
  default = "__StorageAccountName__"
}

variable storage_container_images_name {
  type = string
  description = "A name of a container for images"
  default = "__ImagesContainerName__"
}

variable key_vault_name {
  type = string
  description = "Key vault name"
  default = "__KeyVaultName__"
}

variable sql_server_name {
   type        = string
  description = "The name of the SQL Server"
  default     = "__SQLServerName__"
}

variable sql_db_name {
  type        = string
  description = "The name of the SQL Database"
  default     = "__DatabaseName__"
}

variable admin_username {
  type        = string
  description = "The administrator username of the SQL logical server"
  default     = "__AdminUserName__"
}

variable database_admin_password_name {
  type        = string
  description = "The administrator password name of the SQL logical server"
  default     = "__DatabaseAdminPasswordName__"
}

variable database_admin_password_value {
  type        = string
  description = "The administrator password value of the SQL logical server"
  default     = "__DatabaseAdminPasswordValue__"
  sensitive = true
}

variable server_app_name {
  type = string
  description = "Name of the app for ASP NET server"
  default = "__ServerAppName__"
}

variable client_app_name {
  type = string
  description = "Name of the app for an Angular client"
  default = "__ClientAppName__"
}

variable jwt_key_name {
  type = string
  description = "Name of the key environment variable"
  default = "__JWTKeyName__"
}

variable jwt_key_value {
  type = string
  description = "Value of the key environment variable"
  default = "__JWTKeyValue__"
  sensitive = true
}

variable connection_string_name {
  type = string
  description = "Name of database connection string"
  default = "__AzureSQLConnectionStringName__"
}

variable connection_string_value {
  type = string
  description = "Value of database connection string"
  default = "__AzureSQLConnectionStringValue__"
  sensitive = true
}

variable default_ip_adress {
  type = string
  description = "Default IP address"
  default = "__DefaultIPAdress__"
}

variable storage_connection_name {
  type = string
  description = "A name of a account storage connection"
  default = "__StorageAccountConnectionStringName__"
}

variable storage_connection_value {
  type = string
  description = "A value of a account storage connection"
  default = "__StorageAccountConnectionStringValue__"
  sensitive = true
}

variable upload_azure_func_name {
  type = string
  description = "A name of an azure function to upload compressed images"
  default = "__AzureUploadFuncName__"
}

