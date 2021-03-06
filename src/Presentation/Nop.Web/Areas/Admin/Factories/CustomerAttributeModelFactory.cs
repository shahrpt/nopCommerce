﻿using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Factories;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the customer attribute model factory implementation
    /// </summary>
    public partial class CustomerAttributeModelFactory : ICustomerAttributeModelFactory
    {
        #region Fields

        private readonly ICustomerAttributeService _customerAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public CustomerAttributeModelFactory(ICustomerAttributeService customerAttributeService,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IWorkContext workContext)
        {
            this._customerAttributeService = customerAttributeService;
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare customer attribute search model
        /// </summary>
        /// <param name="model">Customer attribute search model</param>
        /// <returns>Customer attribute search model</returns>
        public virtual CustomerAttributeSearchModel PrepareCustomerAttributeSearchModel(CustomerAttributeSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare page parameters
            model.SetGridPageSize();

            return model;
        }

        /// <summary>
        /// Prepare paged customer attribute list model
        /// </summary>
        /// <param name="searchModel">Customer attribute search model</param>
        /// <returns>Customer attribute list model</returns>
        public virtual CustomerAttributeListModel PrepareCustomerAttributeListModel(CustomerAttributeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get customer attributes
            var customerAttributes = _customerAttributeService.GetAllCustomerAttributes();

            //prepare list model
            var model = new CustomerAttributeListModel
            {
                Data = customerAttributes.PaginationByRequestModel(searchModel).Select(attribute =>
                {
                    //fill in model values from the entity
                    var attributeModel = attribute.ToModel();

                    //fill in additional values (not existing in the entity)
                    attributeModel.AttributeControlTypeName = attribute.AttributeControlType.GetLocalizedEnum(_localizationService, _workContext);

                    return attributeModel;
                }),
                Total = customerAttributes.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare customer attribute model
        /// </summary>
        /// <param name="model">Customer attribute model</param>
        /// <param name="customerAttribute">Customer attribute</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Customer attribute model</returns>
        public virtual CustomerAttributeModel PrepareCustomerAttributeModel(CustomerAttributeModel model,
            CustomerAttribute customerAttribute, bool excludeProperties = false)
        {
            Action<CustomerAttributeLocalizedModel, int> localizedModelConfiguration = null;

            if (customerAttribute != null)
            {
                //fill in model values from the entity
                model = model ?? customerAttribute.ToModel();

                //prepare nested search model
                PrepareCustomerAttributeValueSearchModel(model.CustomerAttributeValueSearchModel, customerAttribute);

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = customerAttribute.GetLocalized(entity => entity.Name, languageId, false, false);
                };
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        /// <summary>
        /// Prepare customer attribute value search model
        /// </summary>
        /// <param name="model">Customer attribute value search model</param>
        /// <param name="customerAttribute">Customer attribute</param>
        /// <returns>Customer attribute value search model</returns>
        public virtual CustomerAttributeValueSearchModel PrepareCustomerAttributeValueSearchModel(CustomerAttributeValueSearchModel model,
            CustomerAttribute customerAttribute)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (customerAttribute == null)
                throw new ArgumentNullException(nameof(customerAttribute));

            model.CustomerAttributeId = customerAttribute.Id;

            //prepare page parameters
            model.SetGridPageSize();

            return model;
        }

        /// <summary>
        /// Prepare paged customer attribute value list model
        /// </summary>
        /// <param name="searchModel">Customer attribute value search model</param>
        /// <param name="customerAttribute">Customer attribute</param>
        /// <returns>Customer attribute value list model</returns>
        public virtual CustomerAttributeValueListModel PrepareCustomerAttributeValueListModel(CustomerAttributeValueSearchModel searchModel,
            CustomerAttribute customerAttribute)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (customerAttribute == null)
                throw new ArgumentNullException(nameof(customerAttribute));

            //get customer attribute values
            var customerAttributeValues = _customerAttributeService.GetCustomerAttributeValues(customerAttribute.Id);

            //prepare list model
            var model = new CustomerAttributeValueListModel
            {
                //fill in model values from the entity
                Data = customerAttributeValues.PaginationByRequestModel(searchModel).Select(value => new CustomerAttributeValueModel
                {
                    Id = value.Id,
                    CustomerAttributeId = value.CustomerAttributeId,
                    Name = value.Name,
                    IsPreSelected = value.IsPreSelected,
                    DisplayOrder = value.DisplayOrder,
                }),
                Total = customerAttributeValues.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare customer attribute value model
        /// </summary>
        /// <param name="model">Customer attribute value model</param>
        /// <param name="customerAttribute">Customer attribute</param>
        /// <param name="customerAttributeValue">Customer attribute value</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Customer attribute value model</returns>
        public virtual CustomerAttributeValueModel PrepareCustomerAttributeValueModel(CustomerAttributeValueModel model,
            CustomerAttribute customerAttribute, CustomerAttributeValue customerAttributeValue, bool excludeProperties = false)
        {
            if (customerAttribute == null)
                throw new ArgumentNullException(nameof(customerAttribute));

            Action<CustomerAttributeValueLocalizedModel, int> localizedModelConfiguration = null;

            if (customerAttributeValue != null)
            {
                //fill in model values from the entity
                model = model ?? new CustomerAttributeValueModel
                {
                    Name = customerAttributeValue.Name,
                    IsPreSelected = customerAttributeValue.IsPreSelected,
                    DisplayOrder = customerAttributeValue.DisplayOrder
                };

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = customerAttributeValue.GetLocalized(entity => entity.Name, languageId, false, false);
                };
            }

            model.CustomerAttributeId = customerAttribute.Id;

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        #endregion
    }
}