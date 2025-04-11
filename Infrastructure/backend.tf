terraform {
  backend "azurerm" {
    resource_group_name  = "assignmentRG"
    storage_account_name = "terraformstatebpb"
    container_name       = "terraform-state"
    key                   = "terraform.tfstate"

    # Use Service Principal authentication here
    client_id            = var.ARM_CLIENT_ID
    client_secret        = var.ARM_CLIENT_SECRET
    tenant_id            = var.ARM_TENANT_ID
  }
}
