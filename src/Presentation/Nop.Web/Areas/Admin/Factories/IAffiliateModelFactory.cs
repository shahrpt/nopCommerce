﻿using Nop.Core.Domain.Affiliates;
using Nop.Web.Areas.Admin.Models.Affiliates;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the affiliate model factory
    /// </summary>
    public partial interface IAffiliateModelFactory
    {
        /// <summary>
        /// Prepare affiliate search model
        /// </summary>
        /// <param name="model">Affiliate search model</param>
        /// <returns>Affiliate search model</returns>
        AffiliateSearchModel PrepareAffiliateSearchModel(AffiliateSearchModel model);

        /// <summary>
        /// Prepare paged affiliate list model
        /// </summary>
        /// <param name="searchModel">Affiliate search model</param>
        /// <returns>Affiliate list model</returns>
        AffiliateListModel PrepareAffiliateListModel(AffiliateSearchModel searchModel);

        /// <summary>
        /// Prepare affiliate model
        /// </summary>
        /// <param name="model">Affiliate model</param>
        /// <param name="affiliate">Affiliate</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Affiliate model</returns>
        AffiliateModel PrepareAffiliateModel(AffiliateModel model, Affiliate affiliate, bool excludeProperties = false);

        /// <summary>
        /// Prepare affiliated order search model
        /// </summary>
        /// <param name="model">Affiliated order search model</param>
        /// <param name="affiliate">Affiliate</param>
        /// <returns>Affiliated order search model</returns>
        AffiliatedOrderSearchModel PrepareAffiliatedOrderSearchModel(AffiliatedOrderSearchModel model, Affiliate affiliate);

        /// <summary>
        /// Prepare paged affiliated order list model
        /// </summary>
        /// <param name="searchModel">Affiliated order search model</param>
        /// <param name="affiliate">Affiliate</param>
        /// <returns>Affiliated order list model</returns>
        AffiliatedOrderListModel PrepareAffiliatedOrderListModel(AffiliatedOrderSearchModel searchModel, Affiliate affiliate);

        /// <summary>
        /// Prepare affiliated customer search model
        /// </summary>
        /// <param name="model">Affiliated customer search model</param>
        /// <param name="affiliate">Affiliate</param>
        /// <returns>Affiliated customer search model</returns>
        AffiliatedCustomerSearchModel PrepareAffiliatedCustomerSearchModel(AffiliatedCustomerSearchModel model,
            Affiliate affiliate);

        /// <summary>
        /// Prepare paged affiliated customer list model
        /// </summary>
        /// <param name="searchModel">Affiliated customer search model</param>
        /// <param name="affiliate">Affiliate</param>
        /// <returns>Affiliated customer list model</returns>
        AffiliatedCustomerListModel PrepareAffiliatedCustomerListModel(AffiliatedCustomerSearchModel searchModel, 
            Affiliate affiliate);
    }
}