using System;
using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.Types;

namespace Adsboard.Common.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _context;

        public QueryDispatcher(IServiceProvider context)
        {
            _context = context;
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _context.GetService(handlerType);

            return await handler.HandleAsync((dynamic)query);
        }
    }
}
