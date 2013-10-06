using Microsoft.Practices.Unity;
using System.Web.Http.Dependencies;

namespace ConcatPdf.Web.Controllers
{
    public class IoCContainer : ScopeContainer, IDependencyResolver
    {
        public IoCContainer(IUnityContainer container)
            : base(container)
        {
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new ScopeContainer(child);
        }
    }
}