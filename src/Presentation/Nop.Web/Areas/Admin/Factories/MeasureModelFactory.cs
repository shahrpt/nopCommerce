﻿using System;
using System.Linq;
using Nop.Core.Domain.Directory;
using Nop.Services.Directory;
using Nop.Web.Areas.Admin.Extensions;
using Nop.Web.Areas.Admin.Models.Directory;
using Nop.Web.Framework.Extensions;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the measures model factory implementation
    /// </summary>
    public partial class MeasureModelFactory : IMeasureModelFactory
    {
        #region Fields

        private readonly IMeasureService _measureService;
        private readonly MeasureSettings _measureSettings;

        #endregion

        #region Ctor

        public MeasureModelFactory(IMeasureService measureService,
            MeasureSettings measureSettings)
        {
            this._measureService = measureService;
            this._measureSettings = measureSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare measure search model
        /// </summary>
        /// <param name="model">Measure search model</param>
        /// <returns>Measure search model</returns>
        public virtual MeasureSearchModel PrepareMeasureSearchModel(MeasureSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare nested search models
            PrepareMeasureDimensionSearchModel(model.MeasureDimensionSearchModel);
            PrepareMeasureWeightSearchModel(model.MeasureWeightSearchModel);

            return model;
        }

        /// <summary>
        /// Prepare measure dimension search model
        /// </summary>
        /// <param name="model">Measure dimension search model</param>
        /// <returns>Measure dimension search model</returns>
        public virtual MeasureDimensionSearchModel PrepareMeasureDimensionSearchModel(MeasureDimensionSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare page parameters
            model.SetGridPageSize();

            return model;
        }

        /// <summary>
        /// Prepare paged measure dimension list model
        /// </summary>
        /// <param name="searchModel">Measure dimension search model</param>
        /// <returns>Measure dimension list model</returns>
        public virtual MeasureDimensionListModel PrepareMeasureDimensionListModel(MeasureDimensionSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get dimensions
            var dimensions = _measureService.GetAllMeasureDimensions();

            //prepare list model
            var model = new MeasureDimensionListModel
            {
                Data = dimensions.PaginationByRequestModel(searchModel).Select(dimension =>
                {
                    //fill in model values from the entity
                    var dimensionModel = dimension.ToModel();

                    //fill in additional values (not existing in the entity)
                    dimensionModel.IsPrimaryDimension = dimension.Id == _measureSettings.BaseDimensionId;

                    return dimensionModel;
                }),
                Total = dimensions.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare measure weight search model
        /// </summary>
        /// <param name="model">Measure weight search model</param>
        /// <returns>Measure weight search model</returns>
        public virtual MeasureWeightSearchModel PrepareMeasureWeightSearchModel(MeasureWeightSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare page parameters
            model.SetGridPageSize();

            return model;
        }

        /// <summary>
        /// Prepare paged measure weight list model
        /// </summary>
        /// <param name="searchModel">Measure weight search model</param>
        /// <returns>Measure weight list model</returns>
        public virtual MeasureWeightListModel PrepareMeasureWeightListModel(MeasureWeightSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get weights
            var weights = _measureService.GetAllMeasureWeights();

            //prepare list model
            var model = new MeasureWeightListModel
            {
                Data = weights.PaginationByRequestModel(searchModel).Select(weight =>
                {
                    //fill in model values from the entity
                    var weightModel = weight.ToModel();

                    //fill in additional values (not existing in the entity)
                    weightModel.IsPrimaryWeight = weight.Id == _measureSettings.BaseWeightId;

                    return weightModel;
                }),
                Total = weights.Count
            };

            return model;
        }

        #endregion
    }
}