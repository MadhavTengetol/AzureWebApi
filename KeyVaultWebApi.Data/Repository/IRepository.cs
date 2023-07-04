using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyVaultWebApi.Data.Repository
{
    public interface IRepository
    {
        public Task<Batch> CreateBatch(Batch batch);
        public Task<Batch> GetBatchById(Guid id);
    }
}
