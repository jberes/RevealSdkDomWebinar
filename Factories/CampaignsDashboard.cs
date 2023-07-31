using Reveal.Sdk.Dom;
using Reveal.Sdk.Dom.Data;
using Reveal.Sdk.Dom.Filters;
using Reveal.Sdk.Dom.Visualizations;
using Sandbox.Helpers;

namespace Sandbox.Factories
{
    internal class CampaignsDashboard
    {
        internal static RdashDocument CreateDashboard()
        {
            var excelDataSourceItem = DataSourceFactory.GetMarketingDataSourceItem();

            var document = new RdashDocument()
            {
                Title = "Campaigns",
                Description = "I created this in code",
                Theme = Theme.TropicalIsland,
                UseAutoLayout = false,
            };

            document.Filters.Add(new DashboardDateFilter()
            {
                RuleType = DateRuleType.TrailingTwelveMonths
            });

            var campaignIdFilter = new DashboardDataFilter(excelDataSourceItem)
            {
                Title = "CampaignID",
                SelectedFieldName = "CampaignID",
                AllowMultipleSelection = true,
                AllowEmptySelection = true
            };
            document.Filters.Add(campaignIdFilter);


            var globalDateFilterBinding = new DashboardDateFilterBinding("Date");
            var territoryFilterBinding = new DashboardDataFilterBinding(campaignIdFilter);

            document.Visualizations.Add(CreateKpiTargetVisualization(excelDataSourceItem, territoryFilterBinding));
            document.Visualizations.Add(CreateIndicatorVisualization("Website Traffic", "Traffic", excelDataSourceItem, territoryFilterBinding));
            document.Visualizations.Add(CreateIndicatorVisualization("Conversions", "Conversions", excelDataSourceItem, territoryFilterBinding));
            document.Visualizations.Add(CreateIndicatorVisualization("New Seats", "New Seats", excelDataSourceItem, territoryFilterBinding));
            document.Visualizations.Add(CreateSplineAreaChartVisualization(excelDataSourceItem, globalDateFilterBinding, territoryFilterBinding));
            document.Visualizations.Add(CreateStackedColumnChartVisualization(excelDataSourceItem, globalDateFilterBinding, territoryFilterBinding));
            document.Visualizations.Add(CreateLineChartVisualization(excelDataSourceItem, globalDateFilterBinding, territoryFilterBinding));
            document.Visualizations.Add(CreateDoughnutChartVisualization(excelDataSourceItem, globalDateFilterBinding, territoryFilterBinding));

            return document;
        }

        private static Visualization CreateKpiTargetVisualization(DataSourceItem excelDataSourceItem, Binding territoryFilterBinding)
        {
            var visualization = new KpiTargetVisualization(excelDataSourceItem)
            {
                Title = "Spend vs Budget",
                ColumnSpan = 15,
                RowSpan = 13,
            };

            visualization.FilterBindings.Add(territoryFilterBinding);

            visualization.Date = new DimensionColumn()
            {
                DataField = new DateDataField("Date")
                {
                    AggregationType = DateAggregationType.Year,
                }
            };

            visualization.Values.Add(new MeasureColumn()
            {
                DataField = new NumberDataField("Spend")
            });

            visualization.Targets.Add(new MeasureColumn()
            {
                DataField = new NumberDataField("Budget")
            });

            return visualization;
        }

        private static Visualization CreateIndicatorVisualization(string title, string field, DataSourceItem excelDataSourceItem, Binding territoryFilterBinding)
        {
            var visualization = new KpiTimeVisualization(excelDataSourceItem)
            {
                Title = title,
                ColumnSpan = 15,
                RowSpan = 13,
            };

            visualization.FilterBindings.Add(territoryFilterBinding);

            visualization.Date = new DimensionColumn()
            {
                DataField = new DateDataField("Date")
                {
                    AggregationType = DateAggregationType.Year,
                }
            };
            
            visualization.Values.Add(new MeasureColumn()
            {
                DataField = new NumberDataField(field)
            });

            return visualization;
        }

        private static Visualization CreateSplineAreaChartVisualization(DataSourceItem excelDataSourceItem, params Binding[] filterBindings)
        {
            var visualization = new SplineAreaChartVisualization(excelDataSourceItem)
            {
                Title = "Actual Spend vs Budget",
                ColumnSpan = 30,
                RowSpan = 28,
            };

            visualization.FilterBindings.AddRange(filterBindings);

            visualization.Labels.Add(new DimensionColumn()
            {
                DataField = new DateDataField("Date")
                {
                    AggregationType = DateAggregationType.Month
                }
            });

            visualization.Values.Add(new MeasureColumn()
            {
                DataField = new NumberDataField("Spend")
            });
            visualization.Values.Add(new MeasureColumn()
            {
                DataField = new NumberDataField("Budget")
            });

            return visualization;
        }

        private static Visualization CreateStackedColumnChartVisualization(DataSourceItem excelDataSourceItem, params Binding[] filterBindings)
        {
            var visualization = new StackedColumnChartVisualization(excelDataSourceItem)
            {
                Title = "Website Traffic Breakdown",
                ColumnSpan = 30,
                RowSpan = 28,
            };

            visualization.FilterBindings.AddRange(filterBindings);

            visualization.Labels.Add(new DimensionColumn()
            {
                DataField = new DateDataField("Date")
                {
                    AggregationType = DateAggregationType.Month
                }
            });

            visualization.Values.Add(new MeasureColumn()
            {
                DataField = new NumberDataField("Paid Traffic")
            });
            visualization.Values.Add(new MeasureColumn()
            {
                DataField = new NumberDataField("Organic Traffic")
            });
            visualization.Values.Add(new MeasureColumn()
            {
                DataField = new NumberDataField("Other Traffic")
            });

            return visualization;
        }

        private static Visualization CreateLineChartVisualization(DataSourceItem excelDataSourceItem, params Binding[] filterBindings)
        {
            var visualization = new LineChartVisualization(excelDataSourceItem)
            {
                Title = "Conversions",
                ColumnSpan = 45,
                RowSpan = 19,
            };

            visualization.FilterBindings.AddRange(filterBindings);

            visualization.Labels.Add(new DimensionColumn()
            {
                DataField = new DateDataField("Date")
                {
                    AggregationType = DateAggregationType.Month
                }
            });

            visualization.Values.Add(new MeasureColumn()
            {
                DataField = new NumberDataField("Conversions")
            });

            return visualization;
        }

        private static Visualization CreateDoughnutChartVisualization(DataSourceItem excelDataSourceItem, params Binding[] filterBindings)
        {
            var visualization = new DoughnutChartVisualization(excelDataSourceItem)
            {
                Title = "Conversions by Territory",
                ColumnSpan = 15,
                RowSpan = 19,
            };

            visualization.FilterBindings.AddRange(filterBindings);

            visualization.Labels.Add(new DimensionColumn()
            {
                DataField = new TextDataField("Territory")
            });

            visualization.Values.Add(new MeasureColumn()
            {
                DataField = new NumberDataField("Conversions")
            });

            return visualization;
        }
    }
}
