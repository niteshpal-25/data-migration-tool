using DataUploader_DadarToTaloja.Interfaces;
using DataUploader_DadarToTaloja.Models;
using Microsoft.AspNetCore.Mvc;

public class ExportController : Controller
{
    private readonly IUserDetailsExportService _userdetailsservice;
    private readonly IDepartmentDetailsExportService _departmentservice;
    private readonly ILogger<ExportController> _logger;

    public ExportController(IUserDetailsExportService userdetailsservice, ILogger<ExportController> logger, IDepartmentDetailsExportService departmentservice)
    {
        _userdetailsservice = userdetailsservice;
        _departmentservice = departmentservice;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult UploadDetails()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUploadDetails()
    {
        try
        {
            int userDetailsCount = await _userdetailsservice.ExportAsync();

            if (userDetailsCount > 0)
            {
                ViewBag.UserDetailsMessage = $"User Details exported successfully: {userDetailsCount} records.";
            }
            else
            {
                ViewBag.UserDetailsMessage = "No User Details records found to export.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while exporting User Details.");
            ViewBag.UserDetailsMessage = "An error occurred while exporting User Details. Please check logs for details.";
        }

        return View("UploadDetails");
    }
}
