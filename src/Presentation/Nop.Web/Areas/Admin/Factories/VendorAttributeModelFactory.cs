﻿using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Services.Localization;
using Nop.Services.Vendors;
using Nop.Web.Areas.Admin.Extensions;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Factories;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the vendor attribute model factory implementation
    /// </summary>
    public partial class VendorAttributeModelFactory : IVendorAttributeModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IVendorAttributeService _vendorAttributeService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public VendorAttributeModelFactory(ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IVendorAttributeService vendorAttributeService,
            IWorkContext workContext)
        {
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
            this._vendorAttributeService = vendorAttributeService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare vendor attribute search model
        /// </summary>
        /// <param name="model">Vendor attribute search model</param>
        /// <returns>Vendor attribute search model</returns>
        public virtual VendorAttributeSearchModel PrepareVendorAttributeSearchModel(VendorAttributeSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare page parameters
            model.SetGridPageSize();

            return model;
        }

        /// <summary>
        /// Prepare paged vendor attribute list model
        /// </summary>
        /// <param name="searchModel">Vendor attribute search model</param>
        /// <returns>Vendor attribute list model</returns>
        public virtual VendorAttributeListModel PrepareVendorAttributeListModel(VendorAttributeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get vendor attributes
            var vendorAttributes = _vendorAttributeService.GetAllVendorAttributes();

            //prepare list model
            var model = new VendorAttributeListModel
            {
                Data = vendorAttributes.PaginationByRequestModel(searchModel).Select(attribute =>
                {
                    //fill in model values from the entity
                    var attributeModel = attribute.ToModel();

                    //fill in additional values (not existing in the entity)
                    attributeModel.AttributeControlTypeName = attribute.AttributeControlType.GetLocalizedEnum(_localizationService, _workContext);

                    return attributeModel;
                }),
                Total = vendorAttributes.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare vendor attribute model
        /// </summary>
        /// <param name="model">Vendor attribute model</param>
        /// <param name="vendorAttribute">Vendor attribute</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Vendor attribute model</returns>
        public virtual VendorAttributeModel PrepareVendorAttributeModel(VendorAttributeModel model,
            VendorAttribute vendorAttribute, bool excludeProperties = false)
        {
            Action<VendorAttributeLocalizedModel, int> localizedModelConfiguration = null;

            if (vendorAttribute != null)
            {
                //fill in model values from the entity
                model = model ?? vendorAttribute.ToModel();

                //prepare nested search model
                PrepareVendorAttributeValueSearchModel(model.VendorAttributeValueSearchModel, vendorAttribute);

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = vendorAttribute.GetLocalized(entity => entity.Name, languageId, false, false);
                };
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        /// <summary>
        /// Prepare vendor attribute value search model
        /// </summary>
        /// <param name="model">Vendor attribute value search model</param>
        /// <param name="vendorAttribute">Vendor attribute</param>
        /// <returns>Vendor attribute value search model</returns>
        public virtual VendorAttributeValueSearchModel PrepareVendorAttributeValueSearchModel(VendorAttributeValueSearchModel model,
            VendorAttribute vendorAttribute)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (vendorAttribute == null)
                throw new ArgumentNullException(nameof(vendorAttribute));

            model.VendorAttributeId = vendorAttribute.Id;

            //prepare page parameters
            model.SetGridPageSize();

            return model;
        }

        /// <summary>
        /// Prepare paged vendor attribute value list model
        /// </summary>
        /// <param name="searchModel">Vendor attribute value search model</param>
        /// <param name="vendorAttribute">Vendor attribute</param>
        /// <returns>Vendor attribute value list model</returns>
        public virtual VendorAttributeValueListModel PrepareVendorAttributeValueListModel(VendorAttributeValueSearchModel searchModel,
            VendorAttribute vendorAttribute)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (vendorAttribute == null)
                throw new ArgumentNullException(nameof(vendorAttribute));

            //get vendor attribute values
            var vendorAttributeValues = _vendorAttributeService.GetVendorAttributeValues(vendorAttribute.Id);

            //prepare list model
            var model = new VendorAttributeValueListModel
            {
                //fill in model values from the entity
                Data = vendorAttributeValues.PaginationByRequestModel(searchModel).Select(value => new VendorAttributeValueModel
                {
                    Id = value.Id,
                    VendorAttributeId = value.VendorAttributeId,
                    Name = value.Name,
                    IsPreSelected = value.IsPreSelected,
                    DisplayOrder = value.DisplayOrder,
                }),
                Total = vendorAttributeValues.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare vendor attribute value model
        /// </summary>
        /// <param name="model">Vendor attribute value model</param>
        /// <param name="vendorAttribute">Vendor attribute</param>
        /// <param name="vendorAttributeValue">Vendor attribute value</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Vendor attribute value model</returns>
        public virtual VendorAttributeValueModel PrepareVendorAttributeValueModel(VendorAttributeValueModel model,
            VendorAttribute vendorAttribute, VendorAttributeValue vendorAttributeValue, bool excludeProperties = false)
        {
            if (vendorAttribute == null)
                throw new ArgumentNullException(nameof(vendorAttribute));

            Action<VendorAttributeValueLocalizedModel, int> localizedModelConfiguration = null;

            if (vendorAttributeValue != null)
            {
                //fill in model values from the entity
                model = model ?? new VendorAttributeValueModel
                {
                    Name = vendorAttributeValue.Name,
                    IsPreSelected = vendorAttributeValue.IsPreSelected,
                    DisplayOrder = vendorAttributeValue.DisplayOrder
                };

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = vendorAttributeValue.GetLocalized(entity => entity.Name, languageId, false, false);
                };
            }

            model.VendorAttributeId = vendorAttribute.Id;

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        #endregion
    }
}