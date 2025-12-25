using DataUploader_DadarToTaloja.Interfaces;
using DataUploader_DadarToTaloja.Models;
using Microsoft.AspNetCore.Mvc;

public class ExportController : Controller
{
    private readonly IUserDetailsExportService _userdetailsservice;
    private readonly IDepartmentDetailsExportService _departmentservice;
    private readonly IProjectDetailsExportService _projectservice;
    private readonly ILogger<ExportController> _logger;

    public ExportController(IUserDetailsExportService userdetailsservice, ILogger<ExportController> logger, IDepartmentDetailsExportService departmentservice, IProjectDetailsExportService projectservice)
    {
        _userdetailsservice = userdetailsservice;
        _departmentservice = departmentservice;
        _projectservice = projectservice;
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
            int DepartmentDetailsCount = await _departmentservice.ExportAsync();
            int ProjectDetailsCount = await _projectservice.ExportAsync();

            if (userDetailsCount > 0 || DepartmentDetailsCount > 0 || ProjectDetailsCount > 0)
            {
                ViewBag.UserDetailsMessage = $"Details exported successfully.";
            }
            else
            {
                ViewBag.UserDetailsMessage = "No records found to export.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while exporting Details.");
            ViewBag.UserDetailsMessage = "An error occurred while exporting Details. Please check logs for details.";
        }

        return View("UploadDetails");
    }
}
