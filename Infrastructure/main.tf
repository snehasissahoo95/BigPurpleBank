terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.90"
    }
  }

  required_version = ">= 1.6"
}

provider "azurerm" {
  features {}
  use_oidc        = true
  client_id       = var.client_id
  tenant_id       = var.tenant_id
  subscription_id = var.subscription_id
}

variable "client_id" {}
variable "tenant_id" {}
variable "subscription_id" {}
variable "sql_password" {}
variable "location" {
  default = "AustraliaEast"
}

data "azurerm_client_config" "current" {}

# ------------------ RESOURCE GROUP ------------------
resource "azurerm_resource_group" "rg" {
  name     = "assignmentRG"
  location = var.location
}

# ------------------ KEY VAULT ------------------
resource "azurerm_key_vault" "vault" {
  name                        = "bigpurplebankkv"
  location                    = azurerm_resource_group.rg.location
  resource_group_name         = azurerm_resource_group.rg.name
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  sku_name                    = "standard"
  purge_protection_enabled    = false

  lifecycle {
    create_before_destroy = true
    prevent_destroy       = false # allow recreation if necessary
  }
}

resource "azurerm_key_vault_access_policy" "current_user" {
  key_vault_id = azurerm_key_vault.vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  secret_permissions = [
    "Get", "List", "Set", "Delete", "Recover", "Purge"
  ]
}

# ------------------ SQL SERVER ------------------
resource "azurerm_mssql_server" "sql" {
  name                         = "bigpurplebank"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "snehasis"
  administrator_login_password = var.sql_password

  lifecycle {
    create_before_destroy = true
    prevent_destroy       = false
  }
}

# ------------------ SQL DATABASE ------------------
resource "azurerm_mssql_database" "db" {
  name      = "BigPurpleBankDB"
  server_id = azurerm_mssql_server.sql.id
  sku_name  = "GP_S_Gen5_2"

  lifecycle {
    create_before_destroy = true
    prevent_destroy       = false
  }
}

# ------------------ KEY VAULT SECRET FOR CONNECTION STRING ------------------
resource "azurerm_key_vault_secret" "sql_connection_string" {
  name         = "SqlConnectionString"
  value        = "Server=tcp:${azurerm_mssql_server.sql.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.db.name};Persist Security Info=False;User ID=${azurerm_mssql_server.sql.administrator_login};Password=${var.sql_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.vault.id

  lifecycle {
    create_before_destroy = true
    prevent_destroy       = false # recreate secret on every deployment
  }
}

# ------------------ SERVICE PLAN ------------------
resource "azurerm_service_plan" "asp" {
  name                = "ASP-assignmentRG-b26b"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Windows"
  sku_name            = "F1"

  lifecycle {
    prevent_destroy = true # prevent deletion of app service plan
  }
}

# ------------------ APP SERVICE ------------------
data "azurerm_key_vault_secret" "sql_connection_string" {
  name         = azurerm_key_vault_secret.sql_connection_string.name
  key_vault_id = azurerm_key_vault.vault.id
}

resource "azurerm_app_service" "app" {
  name                = "BigPurpleBankApp"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_service_plan.asp.id

  app_settings = {
    "ASPNETCORE_ENVIRONMENT"             = "Production"
    "ConnectionStrings__DefaultConnection" = data.azurerm_key_vault_secret.sql_connection_string.value
  }

  identity {
    type = "SystemAssigned"
  }

  source_control {
    # You can configure source control here (e.g., GitHub, Azure Repos)
    # Example for GitHub integration
    repo_url            = "https://github.com/snehasissahoo95/BigPurpleBank"
    branch              = "master"
    manual_integration = false
  }

  lifecycle {
    prevent_destroy = true
  }
}

