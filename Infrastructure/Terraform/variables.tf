variable "resource_group_location" {
  type        = string
  description = "Location for all resources."
  default     = "northeurope"
}

variable "resource_group_name" {
  type        = string
  description = "Resource group name."
  default     = "ReenbitProject1"
}

variable "sql_server_name"{
   type        = string
  description = "The name of the SQL Server."
  default     = "reenbit-sql-server1"
}

variable "sql_db_name" {
  type        = string
  description = "The name of the SQL Database."
  default     = "Database"
}

variable "admin_username" {
  type        = string
  description = "The administrator username of the SQL logical server."
  default     = "maks"
}

variable "admin_password" {
  type        = string
  description = "The administrator password of the SQL logical server."
  sensitive   = true
  default     = "Pass2wor!d"
}

variable server-app-name {
  type = string
  description = "Name of the app for ASP NET server"
  default = "server"
}

variable client-app-name {
  type = string
  description = "Name of the app for an Angular client"
  default = "client"
}

variable tenant_id {
  type = string
  description = "Id of a current tenant"
  default = "9e00bf3c-d568-4597-a3a4-ecae26153172"
}

variable storage_account_name {
  type = string
  description = "Name of a storage account"
  default = "storage-account"
}