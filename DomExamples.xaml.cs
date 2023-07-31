using Reveal.Sdk;
using Reveal.Sdk.Dom.Visualizations;
using Sandbox.Helpers;
using Newtonsoft.Json.Linq;
using Reveal.Sdk;
using Reveal.Sdk.Dom;
using Reveal.Sdk.Dom.Visualizations;
using Reveal.Sdk.Dom.Visualizations.Settings;
using Sandbox.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Sandbox.Factories;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using MessageBox = System.Windows.Forms.MessageBox;

namespace JasonSandbox
{
    /// <summary>
    /// Interaction logic for DomExamples.xaml
    /// </summary>
    public partial class DomExamples : Window
    {
        static readonly string _dashboardFilePath = Path.Combine(Environment.CurrentDirectory, "Dashboards");
        readonly string _readFilePath = Path.Combine(_dashboardFilePath, DashboardFileNames.Sales);
        readonly string _saveJsonToPath = Path.Combine(_dashboardFilePath, "MyDashboard.json");
        readonly string _saveRdashToPath = Path.Combine(_dashboardFilePath, DashboardFileNames.MyDashboard);

        ObservableCollection<IVisualization> _visualizations = new ObservableCollection<IVisualization>();
        ObservableCollection<string> _fields = new ObservableCollection<string>();
        ObservableCollection<string> _details = new ObservableCollection<string>();

        // seconds
        ObservableCollection<IVisualization> _visualizations1 = new ObservableCollection<IVisualization>();
        //ObservableCollection<string> _fields1 = new ObservableCollection<string>();
        //ObservableCollection<string> _details1 = new ObservableCollection<string>();
        public DomExamples()
        {
            InitializeComponent();
            RevealSdkSettings.AuthenticationProvider = new AuthenticationProvider();

            lstTitles.ItemsSource = _visualizations;
            lstVizs.ItemsSource = _visualizations;
            lstFields.ItemsSource = _fields;
            lstDetails.ItemsSource = _details;

            // seconds
            lstTitles1.ItemsSource = _visualizations1;
            lstVizs1.ItemsSource = _visualizations1;
            //lstFields1.ItemsSource = _fields1;
            //lstDetails1.ItemsSource = _details1;
        }

        private async void RevealView_SaveDashboard(object sender, DashboardSaveEventArgs e)
        {
            //var json = _revealView.Dashboard.ExportToJson();
            var path = Path.Combine(Environment.CurrentDirectory, $"Dashboards/{e.Name}.rdash");
            var data = await e.Serialize();
            var json = _revealView.Dashboard.ExportToJson();
            using (var output = File.Open(path, FileMode.Open))
            {
                output.Write(data, 0, data.Length);
            }

            e.SaveFinished();
        }

        private void Load_Dashboard(object sender, RoutedEventArgs e)
        {
            _revealView2.Dashboard = new RVDashboard(_readFilePath);
        }

        private void Clear_Dashboard(object sender, RoutedEventArgs e)
        {
            _revealView.Dashboard = new RVDashboard();
            _revealView1.Dashboard = new RVDashboard();

        }

        private async void Read_Dashboard(object sender, RoutedEventArgs e)
        {
            //var document = RdashDocument.Load(_readFilePath);
            //var json = document.ToJsonString();
            //_revealView3.Dashboard = await RVDashboard.LoadFromJsonAsync(json);
        }

        private async void Create_Dashboard(object sender, RoutedEventArgs e)
        {
            //var document = MarketingDashboard.CreateDashboard();
            //var document = SalesDashboard.CreateDashboard();
            //var document = CampaignsDashboard.CreateDashboard();
            //var document = HealthcareDashboard.CreateDashboard();
            //var document = ManufacturingDashboard.CreateDashboard();
            var document = CustomDashboard.CreateDashboard();
            //var document = RestDataSourceDashboards.CreateDashboard();
            //var document = SqlServerDataSourceDashboards.CreateDashboard();

            //document.Save(_saveRdashToPath);
            var json = document.ToJsonString();
            //json.Save(_saveJsonToPath);
            _revealView4.Dashboard = await RVDashboard.LoadFromJsonAsync(json);
        }

        private async void Update_Marketing_Dashboard(object sender, RoutedEventArgs e)
        {
            var _localFilePath = Path.Combine(_dashboardFilePath, DashboardFileNames.Marketing);
            var document = RdashDocument.Load(_localFilePath);

            var viz = document.Visualizations[3] as ColumnChartVisualization;

            //var x = viz.DataDefinition.Fields[0].FieldName;
            //Console.WriteLine(x);

            // Update field formatting
            var df = viz.Values[0].DataField;
            df.Formatting = new NumberFormatting() { FormatType = NumberFormattingType.Number, DecimalDigits = 3 };

            // Change the values of a selected field
            viz.Values[0].DataField.FieldLabel = "New Average Selected";

            // Change the value of a field in the fields list
            var f = viz.DataDefinition.Fields[0];
            f.FieldLabel = "New Average Field";


            foreach (var field in viz.DataDefinition.Fields)
            {
                string fieldName = field.FieldName;
                string fieldLabel = field.FieldLabel;
                Console.WriteLine($"FieldName: {fieldName}, FieldLabel: {fieldLabel}");
            }

            // Save document
            //document.Save(_saveRdashToPath);

            var json = document.ToJsonString();
            // save JSON
            //json.Save(_saveJsonToPath);
            _revealView5.Dashboard = await RVDashboard.LoadFromJsonAsync(json);
        }

        private async void Create_Sql_Dashboard(object sender, RoutedEventArgs e)
        {
            var document = SqlServerDataSourceDashboards.CreateDashboard();
            var json = document.ToJsonString();
            //json.Save(_saveJsonToPath);
            _revealView2.Dashboard = await RVDashboard.LoadFromJsonAsync(json);
        }

        private async void Read_Fields(object sender, RoutedEventArgs e)
        {
            lstFields2.Items.Clear();

            var document = RdashDocument.Load(_readFilePath);
            var json = document.ToJsonString();

            JObject data = JObject.Parse(json);
            JArray fields = (JArray)data["Widgets"][0]["DataSpec"]["Fields"];

            List<string> fieldNames = new List<string>();
            foreach (JObject field in fields)
            {
                string fieldName = (string)field["FieldName"];
                string userCaption = (string)field["userCaption"];
                string fieldLabel = (string)field["FieldLabel"];

                // Add items to the list
                lstFields2.Items.Add("Field Name: " + fieldName);
                lstFields2.Items.Add("Field Label: " + fieldLabel);
                lstFields2.Items.Add("User Caption: " + userCaption);
                lstFields2.Items.Add("");
            }
            _revealView1.Dashboard = await RVDashboard.LoadFromJsonAsync(json);
        }

        private void Iterate_Objects(object sender, RoutedEventArgs e)
        {
            _fields.Clear();
            _details.Clear();
            _visualizations.Clear();

            var document = RdashDocument.Load(_readFilePath);

            foreach (var viz in document.Visualizations)
            {
                _visualizations.Add(viz);
                Console.WriteLine($"Title: {viz.Title}");
            }
        }

        private void Visualization_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _fields.Clear();
            _details.Clear();

            var viz = e.AddedItems[0] as IVisualization;
            _revealView.Dashboard = new RVDashboard(_readFilePath);
            _revealView.SingleVisualizationMode = true;
            _revealView.ShowMenu = false;
            _revealView.ShowFilters = false;
            _revealView.MaximizedVisualization = _revealView.Dashboard.Visualizations.GetByTitle(viz.Title);

            if (viz is IVisualization<VisualizationSettings, TabularDataDefinition> iViz)
            {
                foreach (var field in iViz.DataDefinition.Fields)
                {
                    _fields.Add(field.FieldName);
                }
            }

            _details.Add(viz.GetType().Name);
            _details.Add("ID: " + viz.Id);
            _details.Add("Title: " + viz.Title);

            if (viz is ILabels iLabels)
            {
                foreach (var label in iLabels.Labels)
                {
                    _details.Add($"Label: {label.DataField.FieldName}");
                }
            }

            if (viz is IValues iValues)
            {
                foreach (var value in iValues.Values)
                {
                    _details.Add($"Value: {value.DataField.FieldName}, Label: {value.DataField.FieldLabel}");
                }
            }

            if (viz is IRows iRows)
            {
                foreach (var value in iRows.Rows)
                {
                    _details.Add($"Row: {value.DataField.FieldName}");
                }
            }

            if (viz is ITargets iTargets)
            {
                foreach (var value in iTargets.Targets)
                {
                    _details.Add($"Target: {value.DataField.FieldName}");
                }
            }
        }

        private void Iterate_Objects_Long_Way(object sender, RoutedEventArgs e)
        {
            _visualizations1.Clear();
            var _localFilePath = Path.Combine(_dashboardFilePath, DashboardFileNames.Marketing);
            var document = RdashDocument.Load(_localFilePath);
            foreach (var viz in document.Visualizations)
            {
                _visualizations1.Add(viz);
            }
        }

        private void Visualization_SelectionChanged1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            lstDetails1.Items.Clear();
            lstFields1.Items.Clear();

            var _localFilePath = Path.Combine(_dashboardFilePath, DashboardFileNames.Marketing);
            var document = RdashDocument.Load(_localFilePath);

            var selectedItem = e.AddedItems[0] as IVisualization;
            foreach (var viz in document.Visualizations)
            {
                if (viz.Id.ToString().Trim() == selectedItem.Id.ToString().Trim()) 
                {
                    {
                        _revealView6.Dashboard = new RVDashboard(_localFilePath);
                        _revealView6.SingleVisualizationMode = true;
                        _revealView6.ShowMenu = false;
                        _revealView6.ShowFilters = false;
                        _revealView6.MaximizedVisualization = _revealView6.Dashboard.Visualizations.GetByTitle(selectedItem.Title.ToString().Trim());

                        _visualizations1.Add(viz);

                        if (viz is IVisualization<VisualizationSettings, TabularDataDefinition> iViz)
                        {
                            foreach (var field in iViz.DataDefinition.Fields)
                            {
                                string fieldName = field.FieldName;
                                string fieldLabel = field.FieldLabel;
                                lstFields1.Items.Add($"FieldName: {fieldName}, FieldLabel: {fieldLabel}");
                            }
                        }
                    }

                    // Column Chart
                    if (viz is ColumnChartVisualization columnChartViz)
                    {
                        lstDetails1.Items.Add("Column Chart");
                        lstDetails1.Items.Add("ID: " + viz.Id.ToString());
                        lstDetails1.Items.Add("Title: " + viz.Title.ToString());

                        foreach (var l in columnChartViz.Labels)
                        {
                            lstDetails1.Items.Add($"Label: {l.DataField.FieldName}");
                        }

                        foreach (var v in columnChartViz.Values)
                        {
                            lstDetails1.Items.Add($"Value: {v.DataField.FieldName}, " +
                                $"Label: {v.DataField.FieldLabel}");
                        }
                    }

                    // Line Chart
                    if (viz is LineChartVisualization lineChartViz)
                    {
                        lstDetails1.Items.Add("Line Chart");
                        lstDetails1.Items.Add("ID: " + viz.Id.ToString());
                        lstDetails1.Items.Add("Title: " + viz.Title.ToString());

                        foreach (var l in lineChartViz.Labels)
                        {
                            lstDetails1.Items.Add($"Label: {l.DataField.FieldName}");
                        }

                        foreach (var v in lineChartViz.Values)
                        {
                            lstDetails1.Items.Add($"Value: {v.DataField.FieldName}, " +
                                $"Label: {v.DataField.FieldLabel}");
                        }
                    }

                    // KPI Target Chart
                    if (viz is KpiTargetVisualization kpiTargetViz)
                    {
                        lstDetails1.Items.Add("KPI Target");
                        lstDetails1.Items.Add("ID: " + viz.Id.ToString());
                        lstDetails1.Items.Add("Title: " + viz.Title.ToString());

                        foreach (var l in kpiTargetViz.Targets)
                        {
                            lstDetails1.Items.Add($"Targets: {l.DataField.FieldName}");
                        }

                        foreach (var v in kpiTargetViz.Values)
                        {
                            lstDetails1.Items.Add($"Value: {v.DataField.FieldName}, " +
                                $"Label: {v.DataField.FieldLabel}");
                        }
                    }

                    // Pie Chart
                    if (viz is PieChartVisualization pieChartViz)
                    {
                        lstDetails1.Items.Add("Pie Chart");
                        lstDetails1.Items.Add("ID: " + viz.Id.ToString());
                        lstDetails1.Items.Add("Title: " + viz.Title.ToString());

                        foreach (var l in pieChartViz.Labels)
                        {
                            lstDetails1.Items.Add($"Label: {l.DataField.FieldName}");
                        }

                        foreach (var v in pieChartViz.Values)
                        {
                            lstDetails1.Items.Add($"Value: {v.DataField.FieldName}, " +
                                $"Label: {v.DataField.FieldLabel}");
                        }
                    }

                    // Funnel Chart
                    if (viz is FunnelChartVisualization funnelChartViz)
                    {
                        lstDetails1.Items.Add("Funnel Chart");
                        lstDetails1.Items.Add("ID: " + viz.Id.ToString());
                        lstDetails1.Items.Add("Title: " + viz.Title.ToString());

                        foreach (var l in funnelChartViz.Labels)
                        {
                            lstDetails1.Items.Add($"Label: {l.DataField.FieldName}");
                        }

                        foreach (var v in funnelChartViz.Values)
                        {
                            lstDetails1.Items.Add($"Value: {v.DataField.FieldName}, " +
                                $"Label: {v.DataField.FieldLabel}");
                        }
                    }

                    // Pivot Grid
                    if (viz is PivotVisualization pivotViz)
                    {
                        lstDetails1.Items.Add("Pivot Grid");
                        lstDetails1.Items.Add("ID: " + viz.Id.ToString());
                        lstDetails1.Items.Add("Title: " + viz.Title.ToString());

                        foreach (var l in pivotViz.Rows)
                        {
                            lstDetails1.Items.Add($"Rows: {l.DataField.FieldName}");
                        }

                        foreach (var v in pivotViz.Values)
                        {
                            lstDetails1.Items.Add($"Value: {v.DataField.FieldName}, " +
                                $"Label: {v.DataField.FieldLabel}");
                        }
                    }
                }
            }
        }
    }
}
