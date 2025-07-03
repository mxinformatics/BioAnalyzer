

resource "azurerm_storage_account" "bioanalyzer" {
  name                     = var.azure_storage_account_name
  resource_group_name      = azurerm_resource_group.bioanalyzer.name
  location                 = azurerm_resource_group.bioanalyzer.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags = local.tags
}

resource "azurerm_storage_container" "bioanalyzer" {
  name                  = "literature"
  storage_account_id = azurerm_storage_account.bioanalyzer.id
  container_access_type = "private"

  
}