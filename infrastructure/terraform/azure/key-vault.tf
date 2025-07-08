data "azurerm_client_config" "current" {

}

data "azuread_client_config" "current" {}

resource "azurerm_key_vault" "bioanalyzer" {
  name                          = var.key_vault_name
  location                      = azurerm_resource_group.bioanalyzer.location
  resource_group_name           = azurerm_resource_group.bioanalyzer.name
  tenant_id                     = data.azuread_client_config.current.tenant_id
  sku_name                      = "standard"
  enabled_for_disk_encryption   = false
  soft_delete_retention_days    = 7
  enable_rbac_authorization     = true
  public_network_access_enabled = true

  tags = local.tags
}


resource "azurerm_role_assignment" "key_vault_role" {
  for_each           = local.key_vault_secret_managers
  scope              = azurerm_key_vault.bioanalyzer.id
  role_definition_id = "/subscriptions/${data.azurerm_client_config.current.subscription_id}/providers/Microsoft.Authorization/roleDefinitions/b86a8fe4-44ce-4948-aee5-eccb2c155cd7"
  principal_id       = each.value.object_id
}

resource "azurerm_role_assignment" "key_vault_encryption_officers" {
  for_each           = local.key_vault_secret_managers
  scope              = azurerm_key_vault.bioanalyzer.id
  role_definition_id = "/subscriptions/${data.azurerm_client_config.current.subscription_id}/providers/Microsoft.Authorization/roleDefinitions/14b46e9e-c2b7-41b4-b07b-48a6ebf60603"
  principal_id       = each.value.object_id
}

resource "azurerm_key_vault_secret" "bioanalyzer_storage_connection_string" {
  key_vault_id = azurerm_key_vault.bioanalyzer.id
  name         = "ResearchStorage--StorageConnectionString"
  value        = azurerm_storage_account.bioanalyzer.primary_connection_string
  depends_on   = [azurerm_key_vault.bioanalyzer]
}

# resource "azurerm_role_assignment" "key_vault_secret_reader" {
#   scope              = azurerm_key_vault.bioanalyzer.id
#   role_definition_id = "/subscriptions/${data.azurerm_client_config.current.subscription_id}/providers/Microsoft.Authorization/roleDefinitions/4633458b-17de-408a-b874-0445c86b69e6"
#   principal_id       = azuread_service_principal.admin_portal_dev.object_id
# }

# resource "azurerm_role_assignment" "key_vault_encryption_user" {
#   scope              = azurerm_key_vault.bioanalyzer.id
#   role_definition_id = "/subscriptions/${data.azurerm_client_config.current.subscription_id}/providers/Microsoft.Authorization/roleDefinitions/14b46e9e-c2b7-41b4-b07b-48a6ebf60603"
#   principal_id       = azuread_service_principal.admin_portal_dev.object_id
# }