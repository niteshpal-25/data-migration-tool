using DataUploader_DadarToTaloja.Interfaces;
using DataUploader_DadarToTaloja.Services;
using Microsoft.AspNetCore.Mvc;

public class ExportController : Controller
{
    private readonly IPIHoldExportService _piholdservice;
    private readonly IUserDetailsExportService _userdetailsservice;

    public ExportController(IPIHoldExportService piholdservice, IUserDetailsExportService userdetailsservice)
    {
        _piholdservice = piholdservice;
        _userdetailsservice = userdetailsservice;
    }

    [HttpGet]
    public IActionResult UploadDetails()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUploadDetails()
    {
        int PIHoldcount = await _piholdservice.ExportAsync();
        int UserDetailscount = await _userdetailsservice.ExportAsync();
        ViewBag.PIHoldMessage = $"PI Hold exported: {PIHoldcount} records.";
        ViewBag.UserDetailsMessage = $"User Details exported: {UserDetailscount} records.";
        return View("PIHold");
    }
}
