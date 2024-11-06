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
  default     = "__SQLName__"
}

variable "admin_password" {
  type        = string
  description = "The administrator password of the SQL logical server."
  sensitive   = true
  default     = "__SQLPassword__"
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

variable tenant_id {
  type = string
  description = "Id of a current tenant"
  default = "__TenantId__"
}
