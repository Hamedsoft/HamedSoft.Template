using HamedSoft.Template.Application.Contracts.Settings;
using HamedSoft.Template.Application.Features.Queries.Settings.GetSettingsByContext;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Domain.Settings;
using HamedSoft.Template.Web.Formatting;
using HamedSoft.Template.Web.Models.Settings;
using HamedSoft.Template.Web.Security;
using HamedSoft.Template.Web.ViewModels.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var settings = await _settingService.GetAllAsync(cancellationToken);

        var model = new SettingsIndexViewModel
        {
            Settings = settings
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string key, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest();

        var setting = await _settingService.GetAsync(key, cancellationToken);

        if (setting is null)
            return NotFound();

        var model = new SettingEditViewModel
        {
            Id = setting.Id,
            Key = setting.Key,
            Module = setting.Module,
            Feature = setting.Feature,
            Category = setting.Category,
            Value = setting.IsSecret ? string.Empty : setting.Value,
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
    public async Task<IActionResult> Edit(SettingEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (model.IsSecret && string.IsNullOrEmpty(model.Value))
            return RedirectToAction(nameof(Index));

        await _settingService.SetAsync(model.Key, model.Value, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Section(string? module, string? feature, string? category, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetSettingsByContextQuery(module, feature, category), cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result.Error);

        var viewModel = result.Value!
            .GroupBy(x => new
            {
                x.Module,
                x.Feature,
                x.Category
            })
            .Select(group => new SettingSectionViewModel
            {
                Module = group.Key.Module,
                Feature = group.Key.Feature,
                Category = group.Key.Category,

                Settings = group
                    .Select(x =>
                    {
                        var valueType = (SettingValueType)x.ValueType;
                        var displayValue = x.Value;
                        var displayExValue = x.Value;

                        switch (valueType)
                        {
                            case SettingValueType.DateTime:
                                if (DateTime.TryParse(
                                    x.Value,
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.RoundtripKind,
                                    out var dateTime))
                                {
                                    displayValue =
                                        PersianDateTimeFormatter.ToPersianDate(dateTime);

                                    displayExValue =
                                        PersianDateTimeFormatter.ToTime(
                                            dateTime,
                                            includeSeconds: false);
                                }

                                break;

                            case SettingValueType.TimeSpan:
                                if (TimeSpan.TryParse(
                                    x.Value,
                                    CultureInfo.InvariantCulture,
                                    out var timeSpan))
                                {
                                    displayExValue =
                                        PersianTimeSpanFormatter.ToTime(
                                            timeSpan,
                                            includeSeconds: false);
                                }

                                break;
                        }

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
                            Description = x.Description
                        };
                    })
                    .ToList()
            })
            .ToList();

        return PartialView("_SettingsPartial", viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Settings.Edit)]
    public async Task<IActionResult> Update(
    string key,
    string value,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest(new
            {
                succeeded = false,
                error = "کلید Setting نامعتبر است."
            });

        var setting = await _settingService.GetAsync(
            key,
            cancellationToken);

        if (setting is null)
            return NotFound(new
            {
                succeeded = false,
                error = "Setting موردنظر پیدا نشد."
            });

        if (setting.IsSecret && string.IsNullOrEmpty(value))
        {
            return Ok(new
            {
                succeeded = true
            });
        }

        var valueType = (SettingValueType)setting.ValueType;

        if (!SettingValueNormalizer.TryNormalize(
                valueType,
                value,
                out var normalizedValue,
                out var error))
        {
            return BadRequest(new
            {
                succeeded = false,
                error
            });
        }

        if (setting.IsRequired &&
            string.IsNullOrWhiteSpace(normalizedValue))
        {
            return BadRequest(new
            {
                succeeded = false,
                error = "وارد کردن این مقدار الزامی است."
            });
        }

        await _settingService.SetAsync(
            key,
            normalizedValue,
            cancellationToken);

        return Ok(new
        {
            succeeded = true
        });
    }
}