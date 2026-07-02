using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Renee.Application.DTOs.CguVersion;
using Renee.Application.Interfaces;
using Renee.Domain;

namespace Renee.UI.Components.Features.Admin.CguVersionManagement;

public partial class CguVersionManagement
{
    [Inject] ICguVersionService cguVersionService { get; set; } = null!;
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] private IFileService FileService { get; set; } = null!;

    private List<CguVersionDto>? _cguVersions;
    private CguVersionDto _cgu = new();
    private bool _isUploading = false;
    private string? _uploadMessage;
    private MemoryStream? _memoryStreamFile {  get; set; }
    private string _fileName = string.Empty;
    private string _fileExtension = string.Empty;


    protected override async Task OnInitializedAsync()
    {
        await GetCguVersionsAsync();   
    }

	private async Task GetCguVersionsAsync()
	{
		_cguVersions = (await cguVersionService.GetAllVersionsAsync()).Value!.ToList();
	}

	public static string GetFileExtension(string fileName) =>
    fileName.Split('.')[^1];


    public async Task OnChangeUpload(InputFileChangeEventArgs e)
	{
		var file = e.File;
		_fileExtension = GetFileExtension(file.Name).ToLowerInvariant();

		if (_fileExtension != "pdf")
		{
			_uploadMessage = Labels.Errors.BadUploadedFileExtension;
            _isUploading = false;
			return;
		}

		_uploadMessage = string.Empty;
		var memoryStream = new MemoryStream();

		await file.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
		memoryStream.Position = 0;
		_memoryStreamFile = memoryStream;
        _fileName = $"CGU_Renee_{_cgu.Version}-{DateTime.Now}.{_fileExtension}";
        _isUploading = true;
	}


    private async Task AddCguAsync()
    {
        if (string.IsNullOrWhiteSpace(_cgu.Version))
        {
            _uploadMessage = Labels.Errors.LoadCguVersionsError;
            return;
        }


        if (_isUploading && _memoryStreamFile != null)
        {
            _cgu.Label = _fileName;
            await FileService.UploadFileAsync($"{_fileName}", $"{_fileName}", _memoryStreamFile);
            var result = await cguVersionService.AddCGUVersion(_cgu);
            if (result.IsSuccess && result.Value)
            {
                _uploadMessage = Labels.VersionCGUAdded;
                _cgu.Version = string.Empty;
                _memoryStreamFile = null;
                _fileName = string.Empty;
                _isUploading = false;
            }
            else
                _uploadMessage = Labels.VersionCGUExists;

            await GetCguVersionsAsync();
		}
        
    }

    public async Task ViewFile(string? _blobName)
    {
        byte[]? fileData = null;
        string fileExtension = string.Empty;
        string fileName = string.Empty;
        if (!string.IsNullOrWhiteSpace(_blobName))
        {
            try
            {
                (_memoryStreamFile, fileName) = await FileService.DownloadFileForSynthesisAsync(_blobName);
                if (_memoryStreamFile != null)
                {
                    fileData = _memoryStreamFile.ToArray();
                    fileExtension = GetFileExtension(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
            }
        }

        if (fileData != null && fileName != null)
        {
            var base64 = Convert.ToBase64String(fileData);

            var mimeType = "application/octet-stream";
            if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)) mimeType = "application/pdf";

            await JSRuntime.InvokeVoidAsync("viewFile", base64, mimeType, _fileName);
        }
    }

}
