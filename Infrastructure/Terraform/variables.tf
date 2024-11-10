variable "resource_group_location" {
  type        = string
  description = "Location for all resources."
  default     = "__Location__"
}

variable "resource_group_name" {
  type        = string
  description = "Resource group name."
  default     = "__ResourceGroupName__"
}
variable "key_vault_name" {
  type = string
  description = "Key vault name"
  default = "__KeyVaultName__"
}
variable "sql_server_name"{
   type        = string
  description = "The name of the SQL Server."
  default     = "__SQLServerName__"
}

variable "sql_db_name" {
  type        = string
  description = "The name of the SQL Database."
  default     = "__DatabaseName__"
}

variable "admin_username" {
  type        = string
  description = "The administrator username of the SQL logical server."
  default     = "__AdminUserName__"
}

variable "database_admin_password_name" {
  type        = string
  description = "The administrator password name of the SQL logical server."
  default     = "__DatabaseAdminPasswordName__"
}

variable "database_admin_password_value" {
  type        = string
  description = "The administrator password value of the SQL logical server."
  default     = "__DatabaseAdminPasswordValue__"
}

variable server-app-name {
  type = string
  description = "Name of the app for ASP NET server"
  default = "__ServerAppName__"
}

variable client-app-name {
  type = string
  description = "Name of the app for an Angular client"
  default = "__ClientAppName__"
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
}

variable default_ip_adress {
  type = string
  description = "Default IP address"
  default = "__DefaultIPAdress__"
}
