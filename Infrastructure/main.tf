provider "azurerm" {
  features {}
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
  soft_delete_enabled         = true
  purge_protection_enabled    = false
}

resource "azurerm_key_vault_access_policy" "current_user" {
  key_vault_id = azurerm_key_vault.vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  secret_permissions = [
    "Get", "List", "Set"
  ]
}

# ------------------ SECRET ------------------
data "azurerm_key_vault_secret" "sql_connection_string" {
  name         = "SqlConnectionString"
  key_vault_id = azurerm_key_vault.vault.id
}

# ------------------ APP SERVICE PLAN ------------------
resource "azurerm_app_service_plan" "asp" {
  name                = "ASP-assignmentRG-b26b"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku {
    tier = "Free"
    size = "F1"
  }
}

# ------------------ APP SERVICE ------------------
resource "azurerm_app_service" "app" {
  name                = "BigPurpleBankApp"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.asp.id

  app_settings = {
    "ASPNETCORE_ENVIRONMENT"      = "Production"
    "ConnectionStrings__DefaultConnection" = data.azurerm_key_vault_secret.sql_connection_string.value
  }
}

# ------------------ SQL SERVER ------------------
resource "azurerm_sql_server" "sql" {
  name                         = "bigpurplebank"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "snehasis"
  administrator_login_password = data.azurerm_key_vault_secret.sql_admin_password.value
}

# ------------------ SQL DATABASE ------------------
resource "azurerm_sql_database" "db" {
  name                = "BigPurpleBankDB"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  server_name         = azurerm_sql_server.sql.name
  sku_name            = "GP_S_Gen5_2"
}
