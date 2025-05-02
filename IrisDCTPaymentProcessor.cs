using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.IrisDCT.Components;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;

namespace Nop.Plugin.Payments.IrisDCT;
public class IrisDCTPaymentProcessor : BasePlugin, IPaymentMethod {

    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly IOrderTotalCalculationService _orderTotalCalculationService;
    protected readonly ISettingService _settingService;
    protected readonly IWebHelper _webHelper;
    protected readonly IPaymentService _paymentService;
    protected readonly ICustomerService _customerService;
    protected readonly IWorkContext _workContext;
    protected readonly IStoreContext _storeContext;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IOrderService _orderService;

    #endregion


    #region Ctor

    public IrisDCTPaymentProcessor(ILocalizationService localizationService, IOrderTotalCalculationService orderTotalCalculationService, ISettingService settingService, IWebHelper webHelper, IPaymentService paymentService, ICustomerService customerService, IWorkContext workContext, IStoreContext storeContext, IHttpContextAccessor httpContextAccessor, IOrderService orderService) {
        _localizationService = localizationService;
        _orderTotalCalculationService = orderTotalCalculationService;
        _settingService = settingService;
        _webHelper = webHelper;
        _paymentService = paymentService;
        _customerService = customerService;
        _workContext = workContext;
        _storeContext = storeContext;
        _httpContextAccessor = httpContextAccessor;
        _orderService = orderService;
    }

    #endregion

    public override async Task InstallAsync() {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string> {
            [$"Plugins.{IrisDCTPaymentsDefault.PluginName}.Payment.Status"] = "Payment status"
        });
        await base.InstallAsync();
    }

    public override async Task UninstallAsync() {
        await _localizationService.DeleteLocaleResourcesAsync(IrisDCTPaymentsDefault.PluginName);
        await base.UninstallAsync();
    }

    public bool SupportCapture => false;

    public bool SupportPartiallyRefund => false;

    public bool SupportRefund => false;

    public bool SupportVoid => false;

    public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

    public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

    public bool SkipPaymentInfo => false;

    public override string GetConfigurationPageUrl() {
        return $"{_webHelper.GetStoreLocation()}Admin/IrisDCT/Configure";
    }

    public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest) {
        return Task.FromResult(new CancelRecurringPaymentResult { Errors = new[] { "Cancel recurring method not supported" } });
    }

    public Task<bool> CanRePostProcessPaymentAsync(Order order) {
        return Task.FromResult(true);
    }

    public Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest) {
        return Task.FromResult(new CapturePaymentResult { Errors = new[] { "Capture method not supported" } });
    }

    public Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart) {
        return Task.FromResult(decimal.Zero);
    }

    public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form) {
        return Task.FromResult(new ProcessPaymentRequest());
    }

    public async Task<string> GetPaymentMethodDescriptionAsync() {
        return await _localizationService.GetResourceAsync($"Plugins.{IrisDCTPaymentsDefault.PluginName}.Payment.Info.Description");
    }

    public Type GetPublicViewComponent() {
        return typeof(PaymentInfoViewComponent);
    }

    public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart) {
        return Task.FromResult(false);
    }

    public async Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest) {
        try {
            //Create and return the RF code
        } catch (Exception ex) {
            throw new NopException($"Error: {ex.Source} - {ex.Message}");
        }
    }

    public Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest) {
        return Task.FromResult(new ProcessPaymentResult());
    }

    public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest) {
        return Task.FromResult(new ProcessPaymentResult { Errors = new[] { "Method not supported" } });
    }

    public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest) {
        return Task.FromResult(new RefundPaymentResult { Errors = new[] { "Method not supported" } });
    }

    public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form) {
        return Task.FromResult<IList<string>>(new List<string>());
    }

    public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest) {
        return Task.FromResult(new VoidPaymentResult { Errors = new[] { "Method not supported" } });
    }
}