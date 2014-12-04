using FourOhFour.Routing;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace FourOhFour {

    public class Startup : ApplicationEventHandler {

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {

            // Add our custom content finder
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, FourOhFourContentFinder>();

        }

    }

}
