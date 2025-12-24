using Microsoft.AspNetCore.Mvc;
using DataUploader_DadarToTaloja.Interfaces;

public class ExportController : Controller
{
    private readonly IPIHoldExportService _service;

    public ExportController(IPIHoldExportService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult UploadDetails()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUploadDetails()
    {
        int count = await _service.ExportAsync();
        ViewBag.Message = $"{count} records exported successfully.";
        return View("PIHold");
    }
}
