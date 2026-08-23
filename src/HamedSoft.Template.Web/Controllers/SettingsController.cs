using System.Globalization;
using System.Reflection;
using HamedSoft.Template.Application.Contracts.Settings;
using HamedSoft.Template.Application.Features.Queries.Settings.GetSettingsByContext;
using HamedSoft.Template.Domain.Settings;
using HamedSoft.Template.Web.Formatting;
using HamedSoft.Template.Web.Models.Settings;
using HamedSoft.Template.Web.ViewModels.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers;

public sealed class SettingsController : Controller
{
    private readonly ISettingService _settingService;
    private readonly ISender _sender;

    public SettingsController(ISettingService settingService, ISender sender)
    {
        _settingService = settingService;
        _sender = sender;
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

    [HttpGet]
    public async Task<IActionResult> Section(
        string module,
        string feature,
        string category,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetSettingsByContextQuery(
                module,
                feature,
                category),
            cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result.Error);

        var viewModel = new SettingSectionViewModel
        {
            Module = module,
            Feature = feature,
            Category = category,
            Settings = result.Value!
    .Select(x =>
    {
        var valueType =
            (Domain.Settings.SettingValueType)x.ValueType;

        var displayValue = valueType == SettingValueType.DateTime && DateTime.TryParse
                                        (x.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dateTime)
                                        ? PersianDateTimeFormatter.ToDate(dateTime) : x.Value;

        var displayExValue = valueType == SettingValueType.DateTime && DateTime.TryParse
                                        (x.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var Time)
                                        ? PersianDateTimeFormatter.ToTime(Time) : x.Value;

        return new SettingItemViewModel
        {
            Id = x.Id,
            Key = x.Key,
            Value = x.Value,
            DisplayValue = displayValue,
            DisplayExValue = displayExValue,
            InputValue = x.Value,
            ValueType = valueType,
            DefaultValue = x.DefaultValue,
            IsRequired = x.IsRequired,
            IsSensitive = x.IsSensitive,
            IsSecret = x.IsSecret,
            Description = x.Description,
        };
    })
    .ToList()
        };

        return PartialView(
            "_SettingsPartial",
            viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(
    string key,
    string value,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest();

        await _settingService.SetAsync(
            key,
            value,
            cancellationToken);

        return Ok();
    }
}