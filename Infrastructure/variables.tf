variable "client_id" {
  description = "Client ID for Azure Service Principal"
  type        = string
}

variable "tenant_id" {
  description = "Tenant ID for Azure Active Directory"
  type        = string
}

variable "subscription_id" {
  description = "Azure Subscription ID"
  type        = string
}

variable "location" {
  description = "The Azure region where the resources will be created"
  type        = string
  default = "AustraliaEast"
}

variable "sql_password" {
  description = "The password for the SQL Server administrator"
  type        = string
}

# You can add any other variables you need, such as resource names, etc.
