# Use a Dynamic Blob Container in the Blazor File Manager

## Overview
 
This sample demonstrates how to use the Syncfusion [Blazor File Manager](https://www.syncfusion.com/blazor-components/blazor-file-manager) with Azure Blob Storage and dynamically configure the blob container used for file operations. Users can browse files stored in Azure Storage and download content directly through the File Manager interface without hard-coding a single storage container.
 
The sample illustrates a practical cloud-storage scenario in which the active Azure Blob container can be selected or configured dynamically. This approach is useful for multi-tenant applications, customer-specific storage locations, and environments where content is organized across multiple containers.

## Key Features

- Uses the Syncfusion Blazor File Manager component with custom AJAX endpoints configured through `FileManagerAjaxSettings`:
  - `Url="/api/Default/AzureFileOperations"`
  - `UploadUrl="/api/Default/AzureUpload"`
  - `DownloadUrl="/api/Default/AzureDownload"`
  - `GetImageUrl="/api/Default/AzureGetImage"`
- Demonstrates dynamic Azure Blob container selection by passing the container name from the client and configuring the storage path at runtime using `SetBlobContainer()`.
- Uses the `OnSend` File Manager event to attach a custom `Container_Name` request header for file operations such as read, create, upload, rename, copy, move, delete, search, and details retrieval.
- Uses the `BeforeDownload` event to pass the selected Azure Blob container name during file download requests.
- Uses the `BeforeImageLoad` event to append the container name when loading image previews from Azure Blob Storage.
- Implements a custom `AzureFileProvider` extending `AzureFileProviderBase` for Azure Blob Storage integration.
- Registers Azure Storage credentials through `RegisterAzure()` and manages storage paths using `SetBlobContainer()`.
- Supports File Manager CRUD operations
- Supports file uploads to Azure Blob Storage through `AzureUpload`.
- Supports image retrieval and preview using `GetImageAsync()` with Azure Blob streams.
- Handles duplicate file and folder scenarios with automatic name generation and conflict detection.
- Returns File Manager responses using camelCase JSON serialization for seamless Syncfusion component integration. 

## Prerequisites

Ensure the following requirements are met before running this project:
- - [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet) compatible with the project's target framework
- Azure Storage account and Blob container  
- Valid Azure Storage connection string  

## How to Run the Project
 
**Visual Studio 2022**
 
1. Clone the repository.
2. Open the solution file:

  `BlazorApp1.sln`

3. Restore NuGet packages.
4. Configure Azure Storage details such as connection strings and container names.
5. Build the project.
6. Run the application using `Ctrl+F5`.

**Visual Studio Code**
 
1. Clone the repository.
2. Open the repository folder in Visual Studio Code.
3. Navigate to the project directory.
 
```bash
cd Azure_fileManager
```
 
4. Restore dependencies.
 
```bash
dotnet restore
```
 
5. Run the application.
 
```bash
dotnet run
```
 
6. Open the application URL displayed in the terminal.

## Usage

Run the Blazor application to view the File Manager connected to Azure Blob Storage. The File Manager dynamically resolves and accesses blob containers, allowing users to browse and download files stored in Azure.

This approach is useful for:
- Multi‑tenant storage scenarios  
- Cloud‑based document management  
- Applications requiring flexible storage configuration  

## Configuration

The Azure Blob container is configured dynamically within the Blazor application. This allows the File Manager to adapt to different storage requirements without hard‑coding container names.

## Project Structure

`Controllers/DefaultController.cs` - ASP.NET Core API controller that exposes File Manager endpoints for Azure file operations, uploads, downloads, image retrieval, and dynamic container configuration.

`Models/AzureFileProvider.cs` - Custom Azure Blob Storage provider implementation extending `AzureFileProviderBase` and handling all File Manager operations including read, create, rename, delete, copy, move, search, upload, download, and image streaming.

`Pages/Index.razor` - Main Blazor page hosting the Syncfusion File Manager component and configuring AJAX endpoints and File Manager events.

## Support and Feedback
 
- For general product questions, visit the [Syncfusion Community Forum](https://www.syncfusion.com/forums) or [Syncfusion Support](https://www.syncfusion.com/support).
- To report an issue specific to this sample, open a GitHub issue in this repository.
- Official documentation: https://help.syncfusion.com/file-manager-sdk/blazor/overview
 
## License
 
This is a Syncfusion sample project provided to demonstrate product usage. Review the [Syncfusion license terms](https://www.syncfusion.com/sales/pricing?category=ui-components) before using Syncfusion components in your own applications.