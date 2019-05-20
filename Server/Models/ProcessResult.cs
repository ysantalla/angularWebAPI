using System.Collections.Generic;
using System.Linq;

namespace Server.Models
{
    public class ProcessResult
    {
        public bool Succeeded { get; private set; }
        public List<string> Errors { get; private set; }
        public ProcessResult(bool isSuccess, List<string> errors = null)
        {
            this.Succeeded = isSuccess;
            this.Errors = errors ?? new List<string>();
        }

        public static ProcessResult Ok()
        {
            return new ProcessResult(true);
        }

        public static ProcessResult Fail()
        {
            return new ProcessResult(false);
        }

        public static ProcessResult Fail(string error)
        {
            return new ProcessResult(false, new List<string>{error});
        }

        public static ProcessResult Fail(IEnumerable<string> errors)
        {
            return new ProcessResult(false, errors.ToList());
        }
    }

    public class ProcessResult<T>
    {
        public bool Succeeded { get; private set; }
        public List<string> Errors { get; private set; }
        public T Value { get; private set; }
        public int CountItems {get; private set;}
        public ProcessResult(bool isSuccess, List<string> errors = null, T data = default(T), int countItems = 0 )
        {
            this.Succeeded = isSuccess;
            this.Errors = errors ?? new List<string>();
            this.Value = data;
            this.CountItems = countItems;
        }

        public static ProcessResult<T> Ok(T result, int countItems = 0)
        {
            return new ProcessResult<T>(true, null, result, countItems);
        }

        public static ProcessResult<T> Fail()
        {
            return new ProcessResult<T>(false);
        }

        public static ProcessResult<T> Fail(string error)
        {
            return new ProcessResult<T>(false, new List<string>{error});
        }
        
        public static ProcessResult<T> Fail(IEnumerable<string> errors)
        {
            return new ProcessResult<T>(false, errors.ToList());
        }
    }
}