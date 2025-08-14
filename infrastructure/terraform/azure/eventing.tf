resource "azurerm_servicebus_namespace" "bioanalzyer" {
  name                = join("-", [var.namespace, var.environment, var.location_abbreviation])
  location            = azurerm_resource_group.bioanalyzer.location
  resource_group_name = azurerm_resource_group.bioanalyzer.name
  sku                 = "Standard"

  tags = local.tags
}

# Service Bus Queue for download-document processing
resource "azurerm_servicebus_queue" "download_document" {
  name         = "download-document"
  namespace_id = azurerm_servicebus_namespace.bioanalzyer.id


  max_size_in_megabytes = 1024

  # Message settings

  max_delivery_count = 10

  # Dead letter settings
  dead_lettering_on_message_expiration = true


}

# Service Bus Topic for download-document-request
resource "azurerm_servicebus_topic" "download_document_request" {
  name         = "download-document-request"
  namespace_id = azurerm_servicebus_namespace.bioanalzyer.id


  max_size_in_megabytes = 1024




}

# Service Bus Subscription that forwards to the queue
resource "azurerm_servicebus_subscription" "download_document_request_to_queue" {
  name     = "forward-to-download-document-queue"
  topic_id = azurerm_servicebus_topic.download_document_request.id

  max_delivery_count = 10

  # Forward messages to the download-document queue
  forward_to = azurerm_servicebus_queue.download_document.name

  # Auto-delete subscription if idle for 5 minutes
  auto_delete_on_idle = "PT5M"


}


resource "azurerm_servicebus_namespace_authorization_rule" "publisher" {
  name         = "bioanalyzer-publish"
  namespace_id = azurerm_servicebus_namespace.bioanalzyer.id

  listen = false
  send   = true
  manage = false
}

resource "azurerm_servicebus_namespace_authorization_rule" "consumer" {
  name         = "bioanalyzer-consume"
  namespace_id = azurerm_servicebus_namespace.bioanalzyer.id

  listen = true
  send   = false
  manage = false
}

resource "azurerm_servicebus_namespace_authorization_rule" "pub_sub" {
  name         = "bioanalyzer-pub-sub"
  namespace_id = azurerm_servicebus_namespace.bioanalzyer.id

  listen = true
  send   = true
  manage = false
}


resource "azurerm_servicebus_topic" "bioanalyzer_topic" {
  for_each     = local.asb_topics
  name         = each.value.name
  namespace_id = azurerm_servicebus_namespace.bioanalzyer.id

  max_size_in_megabytes = 1024
}


locals {
  topic_subscriptions = flatten([
    for topic_key, topic in local.asb_topics : [
      for queue_key, queue in topic.queues : {
        name     = queue.name
        topic_id = azurerm_servicebus_topic.bioanalyzer_topic[topic_key].id
      }
    ]
  ])
}

resource "azurerm_servicebus_queue" "bioanalyzer_queues" {
  for_each     = { for queue in local.topic_subscriptions : queue.name => queue }
  name         = each.value.name
  namespace_id = azurerm_servicebus_namespace.bioanalzyer.id

  max_size_in_megabytes = 1024

  # Message settings
  max_delivery_count = 10

  # Dead letter settings
  dead_lettering_on_message_expiration = true
}

resource "azurerm_servicebus_subscription" "bioanalyzer_subscriptions" {
  for_each           = { for queue in local.topic_subscriptions : queue.name => queue }
  name               = "forward-to-${each.value.name}"
  topic_id           = each.value.topic_id
  max_delivery_count = 10
  forward_to         = azurerm_servicebus_queue.bioanalyzer_queues[each.value.name].name
}