using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funq;
using ServiceStack.Host;
using ServiceStack.Web;

namespace ServiceStackIssues
{
    class AppHost : AppSelfHostBase
    {
        public const int PORT = 5222;

        public AppHost()
            : base("Test Service", typeof(TestService).Assembly)
        {

        }

        public override void Configure(Container container)
        {
            
        }

        public override IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ActionContext actionContext)
        {
            return new AppServiceRunner<TRequest>(this, actionContext);
        }

        public void Start()
        {
            this.Init().Start("http://*:{0}/".Fmt(PORT));
        }

        public string URL { get { return "http://localhost:{0}/".Fmt(PORT); } }
    }

    public class AppServiceRunner<TRequest> : ServiceRunner<TRequest>
    {
        public AppServiceRunner(IAppHost appHost, ActionContext actionContext) : base(appHost, actionContext)
        {
        }
        public override object OnAfterExecute(IRequest requestContext, object response)
        {
            var request = requestContext.Dto as OrderRequestDto;

            if (request != null && request.Compressed && (response != null) && !(response is CompressedResult))
                response = requestContext.ToOptimizedResult(response);

            return base.OnAfterExecute(requestContext, response);
        }
    }
}
