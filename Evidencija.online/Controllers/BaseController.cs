using Microsoft.AspNetCore.Mvc;

namespace Evidencija.online.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger _logger;

        protected BaseController(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
            _logger.LogInformation($"Success message set: {message}");
        }

        protected void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
            _logger.LogWarning($"Error message set: {message}");
        }

        protected void SetWarningMessage(string message)
        {
            TempData["WarningMessage"] = message;
            _logger.LogWarning($"Warning message set: {message}");
        }

        protected string GetCurrentUserEmail()
        {
            return User.Identity?.Name ?? string.Empty;
        }

        protected IActionResult HandleException(Exception ex, string action)
        {
            _logger.LogError($"Exception in {action}", ex);
            SetErrorMessage($"Dogodila se greška tijekom {action}. Molimo pokušajte ponovno.");
            return RedirectToAction("Index");
        }
    }
}
