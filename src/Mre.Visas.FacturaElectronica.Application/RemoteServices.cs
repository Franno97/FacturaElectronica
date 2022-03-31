using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application
{
  public class RemoteServicesRoot
  {
    public RemoteServices RemoteServices { get; set; }
  }
  public class RemoteServices
  {
    public Idp idp { get; set; }
    public Ocelot ocelot { get; set; }
    public Pago pago { get; set; }
    public class Pago
    {
      public string BaseUrl { get; set; }
    }
    public class Idp
    {
      public string BaseUrl { get; set; }
    }
    public class Ocelot
    {
      public string BaseUrl { get; set; }
    }
  }
}
