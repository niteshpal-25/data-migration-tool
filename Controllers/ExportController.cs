using DataUploader_DadarToTaloja.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class ExportController : Controller
{
    private readonly IUserDetailsExportService _userdetailsservice;
    private readonly IDepartmentDetailsExportService _departmentservice;
    private readonly IProjectDetailsExportService _projectservice;
    private readonly ILogger<ExportController> _logger;

    public ExportController(
        IUserDetailsExportService userdetailsservice,
        IDepartmentDetailsExportService departmentservice,
        IProjectDetailsExportService projectservice,
        ILogger<ExportController> logger)
    {
        _userdetailsservice = userdetailsservice;
        _departmentservice = departmentservice;
        _projectservice = projectservice;
        _logger = logger;
    }

    // View Load
    [HttpGet]
    public IActionResult UploadDetails()
    {
        return View();
    }

    // ================= USER EXPORT =================
    [HttpPost]
    public async Task<IActionResult> ExportUsers()
    {
        try
        {
            int count = await _userdetailsservice.ExportAsync();

            return Json(new
            {
                success = true,
                message = $"User export completed. Records: {count}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User export failed");

            return Json(new
            {
                success = false,
                message = "User export failed"
            });
        }
    }

    // ================= DEPARTMENT EXPORT =================
    [HttpPost]
    public async Task<IActionResult> ExportDepartments()
    {
        try
        {
            int count = await _departmentservice.ExportAsync();

            return Json(new
            {
                success = true,
                message = $"Department export completed. Records: {count}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Department export failed");

            return Json(new
            {
                success = false,
                message = "Department export failed"
            });
        }
    }

    // ================= PROJECT EXPORT =================
    [HttpPost]
    public async Task<IActionResult> ExportProjects()
    {
        try
        {
            int count = await _projectservice.ExportAsync();

            return Json(new
            {
                success = true,
                message = $"Project export completed. Records: {count}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Project export failed");

            return Json(new
            {
                success = false,
                message = "Project export failed"
            });
        }
    }
}
