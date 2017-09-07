﻿// <copyright file="MetricsFilterBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filtering;
using App.Metrics.Filters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring the <see cref="IFilterMetrics" /> used for filtering metrics when their values are fetched.
    /// </summary>
    public class MetricsFilterBuilder : IMetricsFilterBuilder
    {
        private readonly IFilterMetrics _filter;
        private readonly IMetricsBuilder _metricsBuilder;
        private readonly Action<IFilterMetrics> _metricsFilter;

        internal MetricsFilterBuilder(
            IMetricsBuilder metricsBuilder,
            Action<IFilterMetrics> metricsFilter)
        {
            _metricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _metricsFilter = metricsFilter ?? throw new ArgumentNullException(nameof(metricsFilter));
            _filter = new DefaultMetricsFilter();
        }

        /// <inheritdoc />
        public IMetricsBuilder ByIncludingOnlyContext(string context)
        {
            _filter.WhereContext(context);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ByIncludingOnlyTagKeyValues(TagKeyValueFilter tagKeyValues)
        {
            _filter.WhereMetricTaggedWithKeyValue(tagKeyValues);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ByIncludingOnlyTags(params string[] tagKeys)
        {
            _filter.WhereMetricTaggedWithKey(tagKeys);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ByIncludingOnlyTypes(params MetricType[] types)
        {
            _filter.WhereType(types);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder With(IFilterMetrics filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _metricsFilter(filter);

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder With(Action<IFilterMetrics> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            setupAction(_filter);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }
    }
}