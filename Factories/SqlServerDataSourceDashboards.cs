using Reveal.Sdk.Dom;
using Reveal.Sdk.Dom.Data;
using Reveal.Sdk.Dom.Visualizations;
using System.Collections.Generic;

namespace Sandbox.Factories
{
    internal class SqlServerDataSourceDashboards
    {
        internal static RdashDocument CreateDashboard()
        {
            var document = new RdashDocument("My Dashboard");

            var sqlServerDataSourceItem = new SqlServerBuilder
                        ("", "devtest", "Orders")
                .SetTitle("SQL Server")
                .SetSubtitle("Orders")
                .SetFields(new List<IField>()
                {
                    new TextField("CustomerID"),
                    new NumberField("OrderID"),
                })
                .Build();

            document.Visualizations.Add
                    (new BarChartVisualization("SQL Server Bar Chart", sqlServerDataSourceItem)
                .SetLabel
                    ("CustomerID").SetValue(new NumberDataField("OrderID") 
                    { AggregationType = AggregationType.CountRows }));

            return document;
        }
    }
}
