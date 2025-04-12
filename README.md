# BigPurpleBank

## Overview

This project provisions and configures the **BigPurpleBankApp** using Terraform on Azure. The infrastructure includes an App Service, App Service Plan, Key Vault integration, and source control integration with a Git repository.

## Architecture

### Components
| Resource | Description |
|----------|-------------|
| `azurerm_resource_group` | Resource Group to group all related resources |
| `azurerm_service_plan`   | App Service Plan that hosts the App Service |
| `azurerm_app_service`    | Azure App Service to host the BigPurpleBank application |
| `azurerm_key_vault` + `azurerm_key_vault_secret` | Securely store and access the SQL connection string |
| `source_control` block in `azurerm_app_service` | Integrates App Service with a GitHub repository for CI/CD |

## Deployment Details

### App Service
- **Name**: `BigPurpleBankApp`
- **Environment**: Production
- **Identity**: SystemAssigned (Used for secure access to Key Vault)
- **App Settings**:
  - `ASPNETCORE_ENVIRONMENT`: `Production`
  - `ConnectionStrings__DefaultConnection`: Pulled securely from Key Vault

### Source Control
- **Repository URL**: `https://github.com/snehasissahoo95/BigPurpleBank`
- **Branch**: `master`
- **CI/CD**: Integrated via Azure App Service Source Control


## API Endpoints

The following endpoint is available once the application is deployed:

- **Get Accounts API**  
  `GET https://bigpurplebankapp-hrgfaqdjf8hhbag5.australiaeast-01.azurewebsites.net/banking/accounts`  
  Returns a list of user accounts.