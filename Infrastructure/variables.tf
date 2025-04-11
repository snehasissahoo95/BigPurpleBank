variable "location" {
  description = "The Azure region to deploy resources into"
  type        = string
  default     = "AustraliaEast"
}

variable "client_id" {
  description = "The Client ID of the Azure Service Principal"
  type        = string
}

variable "tenant_id" {
  description = "The Tenant ID of the Azure Active Directory"
  type        = string
}

variable "subscription_id" {
  description = "The Subscription ID for Azure"
  type        = string
}
