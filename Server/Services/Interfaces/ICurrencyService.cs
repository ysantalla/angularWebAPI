using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface ICurrencyService : ICrudService<Currency, CurrencyFilter>
    {
    }
}