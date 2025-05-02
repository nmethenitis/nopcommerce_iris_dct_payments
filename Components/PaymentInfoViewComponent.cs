using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.IrisDCT.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.IrisDCT.Components;
public class PaymentInfoViewComponent : NopViewComponent {
    protected readonly IrisDCTSettings _irisDCTSettings;
    protected readonly ILocalizationService _localizationService;
    protected readonly IStoreContext _storeContext;
    protected readonly IWorkContext _workContext;

    public PaymentInfoViewComponent(IrisDCTSettings irisDCTSettings,
        ILocalizationService localizationService,
        IStoreContext storeContext,
        IWorkContext workContext) {
        _irisDCTSettings = irisDCTSettings;
        _localizationService = localizationService;
        _storeContext = storeContext;
        _workContext = workContext;
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData) {
        var store = await _storeContext.GetCurrentStoreAsync();

        var model = new IrisDCTInfoModel {
            DescriptionText = await _localizationService.GetLocalizedSettingAsync(_irisDCTSettings,
                x => x.PaymentDescription, (await _workContext.GetWorkingLanguageAsync()).Id, store.Id)
        };

        return View($"~/Plugins/{IrisDCTPaymentsDefault.PluginName}/Views/PaymentInfo.cshtml", model);
    }
}