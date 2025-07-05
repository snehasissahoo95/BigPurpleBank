terraform {
  backend "azurerm" {
    resource_group_name  = "demoRG"
    storage_account_name = "terraformstatebpb"
    container_name       = "terraform-state"
    key                   = "terraform.tfstate"
  }
}
