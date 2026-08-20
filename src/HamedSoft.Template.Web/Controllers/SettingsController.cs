using HamedSoft.Template.Application.Contracts.Settings;
using HamedSoft.Template.Web.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers;

public sealed class SettingsController : Controller
{
    private readonly ISettingService _settingService;

    public SettingsController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var settings = await _settingService
            .GetAllAsync(cancellationToken);

        var model = new SettingsIndexViewModel
        {
            Settings = settings
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(
        string key,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest();

        var setting = await _settingService.GetAsync(
            key,
            cancellationToken);

        if (setting is null)
            return NotFound();

        var model = new SettingEditViewModel
        {
            Id = setting.Id,
            Key = setting.Key,
            Module = setting.Module,
            Feature = setting.Feature,
            Category = setting.Category,
            Value = setting.IsSecret
                ? string.Empty
                : setting.Value,
            ValueType = (Domain.Settings.SettingValueType)setting.ValueType,
            DefaultValue = setting.DefaultValue,
            IsRequired = setting.IsRequired,
            IsSensitive = setting.IsSensitive,
            IsSecret = setting.IsSecret,
            Description = setting.Description
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        SettingEditViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Secret fields are not returned to the browser.
        // Empty value means "keep the existing secret".
        if (model.IsSecret && string.IsNullOrEmpty(model.Value))
            return RedirectToAction(
                nameof(Index));

        await _settingService.SetAsync(
            model.Key,
            model.Value,
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}