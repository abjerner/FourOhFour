using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Routing;
using umbraco.cms.businesslogic.web;

namespace FourOhFour.Routing {
    
    public class FourOhFourContentFinder : IContentFinder {

        public bool TryFindContent(PublishedContentRequest contentRequest) {

            // Get the current domain name
            string domainName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            // Get the root node id of the domain
            int rootNodeId = Domain.GetRootFromDomain(domainName);

            // Return if a root node couldn't be found
            if (rootNodeId <= 0) return false;

            // Find the root node from the ID
            IPublishedContent root = (rootNodeId > 0 ? UmbracoContext.Current.ContentCache.GetById(rootNodeId) : null);

            // Return FALSE if the root node wasn't found (AKA move on to the next content finder)
            if (root == null) return false;

            // Handle error pages for each site type
            switch (root.DocumentTypeAlias)  {
                
                case "MainSite": {

                    // Get the error page from the hardcoded ID
                    contentRequest.PublishedContent = UmbracoContext.Current.ContentCache.GetById(1059);

                    break;

                }

                case "InstitutionSite": {

                    int errorPageId = root.GetPropertyValue<int>("notFoundPage");

                    // Get the error page from the specified ID in the "notFoundPage" property
                    contentRequest.PublishedContent = UmbracoContext.Current.ContentCache.GetById(errorPageId);

                    break;

                }

                case "CampaignSite": {

                    // Find the first child with the "CampaignNotFound" document type
                    contentRequest.PublishedContent = root.Children.FirstOrDefault(x => x.DocumentTypeAlias == "CampaignNotFound");

                    break;

                }
           
            }

            // Return whether an error page was found
            return contentRequest.PublishedContent != null;

        }

    }

}
