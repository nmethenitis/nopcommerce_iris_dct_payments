using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.IrisDCT;
public class IrisDCTSettings : ISettings{
    public string PaymentDescription { get; set; }
}
