using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class GetListViewModel<T_FILTER>
    {
        public Paginator paginator { get; set; }
        
        public OrderBy orderBy { get; set; }

        public T_FILTER filter { get; set; }

    }
}