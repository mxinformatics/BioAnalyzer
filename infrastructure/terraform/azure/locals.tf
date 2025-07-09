
data "azuread_user" "dan_maxim" {
  user_principal_name = "dmaxim@mxinformatics.com"
}


locals {
  tags = {
    "environment" = var.environment
  }

  key_vault_secret_managers = {
    dan_maxim = {
      object_id = data.azuread_user.dan_maxim.object_id
    }
  }
  asb_topics = {
    document_downloaded = {
      name = "document-downloaded"
      queues = {
        extract_document_text = {
          name = "extract-document-text"
        }
        build_document_list = {
          name = "build-document-list"
        }
      }
    }
    text_extracted = {
      name = "text-extracted"
      queues = {
        process_extracted_text = {
          name = "process-extracted-text"
        }
      }

    }
  }
  # download_request = {
  #   name = "download-document-request"
  #   queues = [
  #     {
  #       name = "download-document"
  #     }
  #   ]
  # }

}