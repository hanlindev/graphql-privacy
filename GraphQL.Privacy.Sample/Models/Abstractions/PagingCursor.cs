using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Models.Abstractions
{
    public abstract class PagingCursor<TFields, TTarget>
    {
        // The self-incrementing ID of the model
        public TFields Fields { get; set; }

        public bool IsValid
        {
            get
            {
                return Fields != null;
            }
        }

        public abstract Expression<Func<TTarget, bool>> Comparator { get; }

        public string Encode()
        {
            var json = JsonConvert.SerializeObject(Fields);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        public void Decode(string base64EncodedJsonCursor)
        {
            try
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedJsonCursor));
                var decoded = JsonConvert.DeserializeObject<TFields>(json);
                Fields = decoded;
            }
            catch
            {
                // Silently fail this decode
            }
        }

        public abstract void RestoreFromEntry(TTarget target);
    }
}
