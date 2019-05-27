using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IInvoiceService
    {
         Task<ProcessResult> CreateAsync(Invoice model);
         
         Task<ProcessResult<Invoice>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, Invoice model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult<List<Invoice>>> ListAsync(GetListViewModel<InvoiceFilter> listModel);
         
         Task<ProcessResult<int>> CountAsync(InvoiceFilter filter);
   
    }
}