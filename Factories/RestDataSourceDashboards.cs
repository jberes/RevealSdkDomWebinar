using Reveal.Sdk.Dom;
using Reveal.Sdk.Dom.Data;
using Reveal.Sdk.Dom.Visualizations;
using Sandbox.Helpers;

namespace Sandbox.Factories
{
    internal class RestDataSourceDashboards
    {
        internal static RdashDocument CreateDashboard()
        {
            var document = new RdashDocument("My Dashboard");

            //json - default
            var jsonDataSourceItem = new RestServiceBuilder("https://excel2json.io/api/share/6e0f06b3-72d3-4fec-7984-08da43f56bb9")
                .SetTitle("JSON Data Source")
                .SetSubtitle("Sales by Category")
                .SetFields(DataSourceFactory.GetSalesByCategoryFields())
                .Build();

            document.Visualizations.Add(new PieChartVisualization("JSON", jsonDataSourceItem)
                .SetLabel("CategoryName").SetValue("ProductSales"));

            //excel
            var excelDataSourceItem = new RestServiceBuilder("http://dl.infragistics.com/reportplus/reveal/samples/Samples.xlsx")
                .UseExcel()
                .SetTitle("Excel Data Source")
                .SetSubtitle("Marketing")
                .SetFields(DataSourceFactory.GetMarketingDataSourceFields())
                .Build();

            document.Visualizations.Add(new PieChartVisualization("Excel", excelDataSourceItem)
                .SetLabel("Territory").SetValue("Conversions"));

            //csv
            var csvDataSourceItem = new RestServiceBuilder("https://query.data.world/s/y32gtgblzpemyyvtig47dz7tedgkto")
                .UseCsv()
                .SetTitle("CSV Data Source")
                .SetSubtitle("Illinois School Info")
                .SetFields(DataSourceFactory.GetCsvDataSourceFields())
                .Build();

            document.Visualizations.Add(new ScatterMapVisualization("Scatter", csvDataSourceItem)
                .SetMap(Maps.NorthAmerica.UnitedStates.States.Illinois)
                .SetLongitude("X")
                .SetLatitude("Y")
                .SetLabel("School_Nm")
                .ConfigureSettings(settings =>
                {
                    settings.Zoom.Longitude = 1.38;
                    settings.Zoom.Latitude = 41.65;
                    settings.Zoom.DegreesLongitude = 1.04;
                    settings.Zoom.DegreesLatitude = 0.39;
                }));

            return document;
        }
    }
}
