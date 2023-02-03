using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Intelmatix.Modules.Sidebar.Primitives
{
    [Serializable]
    public class SidebarData
    {
        [SerializeField] private string title;
        [SerializeField] private string current_filter;
        [SerializeField] private List<ChartGroup> linecharts;
        [SerializeField] private List<ChartGroup> barcharts;
        [SerializeField] private List<DataTable> tables;
        [SerializeField] private List<FiltersData> filters;
        [SerializeField] private List<Decision> decisions;


        public string Title => title;
        public string CurrentFilter => current_filter;

        public List<FiltersData> Filters => filters;
        public List<DataTable> TableCharts => tables;
        public List<ChartGroup> LineCharts => linecharts;
        public List<ChartGroup> BarCharts => barcharts;
        public List<Decision> Decisions => decisions;

        [SerializeField]
        public class Decision
        {
            [SerializeField] private int id;
            [SerializeField] private string label;
            [SerializeField] private List<KPI> kpi;
            public int Id => id;
            public string Label => label;
            public List<KPI> KPI => kpi;
        }

        [SerializeField]
        public class KPI
        {
            [SerializeField] private int id;
            [SerializeField] private string kpi_value;
            [SerializeField] private string subtitle;

            public int Id => id;
            public string KPIValue => kpi_value;
            public string Subtitle => subtitle;
        }


        [Serializable]
        public class StoreData
        {

            [SerializeField] private string title;
            [SerializeField] private List<ChartGroup> linecharts;
            [SerializeField] private Table[] tables;
            public string Title => title;
            public Table[] Tables => tables;
            public List<ChartGroup> LineCharts => linecharts;
        }
        [Serializable]
        public class FiltersData
        {

            [SerializeField] private string filter;
            [SerializeField] private string path;
            public string Filter => filter;
            public string Path => path;

        }

        [Serializable]
        public class ChartGroup
        {

            [SerializeField] private string title;
            [SerializeField] private string subtitle;
            [SerializeField] private List<Chart> charts;
            public List<Chart> Charts => charts;
            public string Title => title;
            public string Subtitle => subtitle;
        }

        [Serializable]
        public class Table
        {
            [SerializeField] private string item;
            [SerializeField] private DataTable[] data;

            public string Item => item;
            public DataTable[] Data => data;
        }

        [Serializable]
        public class DataTable
        {
            [SerializeField] private string title;
            [SerializeField] private string table_type;
            [SerializeField] private List<RowInfo> rows;

            public string Title => title;
            public string TableType => table_type;
            public List<RowInfo> Rows => rows;

        }

        [Serializable]
        public class RowInfo
        {
            [SerializeField] private string title;
            [SerializeField] private RowsData data;
            public string Title => title;
            public RowsData Data => data;
        }

        [Serializable]
        public class RowsData
        {
            [SerializeField] private string unit;
            [SerializeField] private Extras extra;
            [SerializeField] private float value;
            public float Value => value;
            public string Unit => unit;
            public Extras Extra => extra;

        }
        [Serializable]
        public class Extras
        {
            [SerializeField] private float value;
            [SerializeField] private string type;
            public float Value => value;
            public string Type => type;

        }
        [Serializable]
        public class Chart
        {
            [SerializeField] private string item;
            [SerializeField] private AxisConfig axis_config;
            [SerializeField] private Data data;
            public string Item => item;
            public AxisConfig AxisConfig => axis_config;
            public Data Data => data;

            // ffffff -> rgb(255, 255, 255)
            // ffca00 -> rgb(255, 202, 0)
            // d5a7ff -> rgb(213, 167, 255)
            // ff00ff -> rgb(255, 0, 255)
            // 00b99a -> rgb(0, 185, 154)
            // ff6321 -> rgb(255, 99, 33)
            private Color[] colors = new Color[] {
                new Color(1, 1, 1),
                new Color(255 / 255f, 202 / 255f, 0 / 255f),
                new Color(213 / 255f, 167 / 255f, 255 / 255f),
                new Color(255 / 255f, 0 / 255f, 255 / 255f),
                new Color(0 / 255, 185 / 255f, 154 / 255f),
                new Color(255 / 255f, 99 / 255f, 33 / 255f)
            };

            public List<AwesomeCharts.LineDataSet> GetLineDataSets()
            {
                AwesomeCharts.LineEntry line = null;
                var index = 0;
                List<AwesomeCharts.LineDataSet> lineDataSets = new List<AwesomeCharts.LineDataSet>();
                foreach (var dataSet in data.DataSets)
                {
                    AwesomeCharts.LineDataSet lineDataSet = new AwesomeCharts.LineDataSet();

                    if (data.DataSets.Length > 2 && index < colors.Length)
                    {
                        lineDataSet.LineColor = colors[index++];
                    }
                    else
                    {
                        lineDataSet.LineColor = Color.white;

                        if (line != null && dataSet.Title.ToLower().Equals("predict"))
                        {
                            lineDataSet.Entries.Add(line);
                            lineDataSet.LineColor = new Color(0, 255 / 255f, 154 / 255f); // 00FF9A
                        }
                    }

                    lineDataSet.LineThickness = 1f;
                    lineDataSet.Title = dataSet.Title;
                    foreach (var entry in dataSet.Entries)
                    {
                        lineDataSet.Entries.Add(entry.ToLineEntry());
                        line = lineDataSet.Entries[lineDataSet.Entries.Count - 1];
                    }
                    lineDataSets.Add(lineDataSet);
                }
                return lineDataSets;
            }


            public List<AwesomeCharts.BarDataSet> GetBarDataSets()
            {
                List<AwesomeCharts.BarDataSet> barDataSets = new List<AwesomeCharts.BarDataSet>();
                var index = 0;
                foreach (var dataSet in data.DataSets)
                {
                    AwesomeCharts.BarDataSet barDataSet = new AwesomeCharts.BarDataSet();
                    if (data.DataSets.Length > 2 && index < colors.Length)
                    {
                        barDataSet.BarColors.Add(colors[index++]);
                    }
                    else
                    {
                        barDataSet.BarColors.Add(Color.white);
                    }
                    // barDataSet.BarColor = Color.white;
                    //barDataSet.BarColors = Color.white;
                    // lineDataSet.LineThickness = 1;
                    //barDataSet. = 1f;
                    barDataSet.Title = dataSet.Title;
                    foreach (var entry in dataSet.Entries)
                    {
                        barDataSet.Entries.Add(entry.ToBarEntry());
                    }
                    barDataSets.Add(barDataSet);
                }
                return barDataSets;
            }


            public void ApplyAxisConfiguration(AwesomeCharts.LineChart lineChartTemplate)
            {
                // bind axis config
                lineChartTemplate.AxisConfig.HorizontalAxisConfig.Bounds.Max = this.AxisConfig.HorizontalAxisconfig.Max;
                lineChartTemplate.AxisConfig.HorizontalAxisConfig.Bounds.Min = this.AxisConfig.HorizontalAxisconfig.Min;

                lineChartTemplate.AxisConfig.VerticalAxisConfig.Bounds.Max = this.AxisConfig.VerticalAxisConfig.Max;
                lineChartTemplate.AxisConfig.VerticalAxisConfig.Bounds.Min = this.AxisConfig.VerticalAxisConfig.Min;

                // bind custom values
                if (this.AxisConfig.HorizontalAxisconfig.CustomValues.Count > 0 && this.AxisConfig.HorizontalAxisconfig.CustomValues.Count < 7)
                {
                    lineChartTemplate.AxisConfig.HorizontalAxisConfig.ValueFormatterConfig.CustomValues = this.AxisConfig.HorizontalAxisconfig.CustomValues;
                    lineChartTemplate.AxisConfig.HorizontalAxisConfig.LabelsCount = this.AxisConfig.HorizontalAxisconfig.CustomValues.Count;
                }

                if (this.AxisConfig.VerticalAxisConfig.CustomValues.Count > 0)
                {
                    lineChartTemplate.AxisConfig.VerticalAxisConfig.LabelsCount = this.AxisConfig.VerticalAxisConfig.LabelsCount;
                    lineChartTemplate.AxisConfig.VerticalAxisConfig.ValueFormatterConfig.CustomValues = this.AxisConfig.VerticalAxisConfig.CustomValues;
                }

                lineChartTemplate.SetDirty();

                // if (this.AxisConfig.HorizontalAxisconfig.LabelsCount > 7)
                // {
                //     lineChartTemplate.AxisConfig.HorizontalAxisConfig.LabelsConfig.LabelColor = new Color(1, 1, 1, 0);
                // }
            }


            public void ApplyBarAxisConfiguration(AwesomeCharts.BarChart lineChartTemplate)
            {
                // bind axis config
                lineChartTemplate.AxisConfig.HorizontalAxisConfig.Bounds.Max = this.AxisConfig.HorizontalAxisconfig.Max;
                lineChartTemplate.AxisConfig.HorizontalAxisConfig.Bounds.Min = this.AxisConfig.HorizontalAxisconfig.Min;

                lineChartTemplate.AxisConfig.VerticalAxisConfig.Bounds.Max = this.AxisConfig.VerticalAxisConfig.Max;
                lineChartTemplate.AxisConfig.VerticalAxisConfig.Bounds.Min = this.AxisConfig.VerticalAxisConfig.Min;


                // bind custom values
                if (this.AxisConfig.HorizontalAxisconfig.CustomValues.Count > 0 && this.AxisConfig.HorizontalAxisconfig.CustomValues.Count < 7)
                {
                    lineChartTemplate.AxisConfig.HorizontalAxisConfig.ValueFormatterConfig.CustomValues = this.AxisConfig.HorizontalAxisconfig.CustomValues;
                    // lineChartTemplate.AxisConfig.HorizontalAxisConfig.LabelsCount = this.AxisConfig.HorizontalAxisconfig.CustomValues;
                }
                if (this.AxisConfig.VerticalAxisConfig.CustomValues.Count > 0)
                {
                    lineChartTemplate.AxisConfig.VerticalAxisConfig.ValueFormatterConfig.CustomValues = this.AxisConfig.VerticalAxisConfig.CustomValues;
                    lineChartTemplate.AxisConfig.VerticalAxisConfig.LabelsCount = this.AxisConfig.VerticalAxisConfig.LabelsCount;
                }

                lineChartTemplate.SetDirty();
                var length = this.data.DataSets[0].Entries.Length + 1;
                var width = lineChartTemplate.GetComponent<RectTransform>().sizeDelta.x;
                width = Mathf.Max(width, 518);
                lineChartTemplate.Config.BarSpacing = (int)(width / length);


                Debug.Log("BarSpacing width " + width + " length " + length + " spacing " + lineChartTemplate.Config.BarSpacing);
                // if (this.AxisConfig.HorizontalAxisconfig.LabelsCount > 7)
                // {
                //     lineChartTemplate.AxisConfig.HorizontalAxisConfig.LabelsConfig.LabelColor = new Color(1, 1, 1, 0);
                // }
            }

        }

        [Serializable]
        public class AxisConfig
        {
            [SerializeField] private DirectionalAxisConfig horizontal_axis_config;
            [SerializeField] private DirectionalAxisConfig vertical_axis_config;
            public DirectionalAxisConfig HorizontalAxisconfig => horizontal_axis_config;
            public DirectionalAxisConfig VerticalAxisConfig => vertical_axis_config;
        }

        [Serializable]
        public class DirectionalAxisConfig
        {
            [SerializeField] private float min;
            [SerializeField] private float max;
            [SerializeField] private int labels_count;
            [SerializeField] private List<string> custom_labels;
            public float Min => min;
            public float Max => max;
            public int LabelsCount => labels_count;
            public List<string> CustomValues => custom_labels;
        }
        [Serializable]
        public class Data
        {
            [SerializeField] private DataSet[] data_sets;
            public DataSet[] DataSets => data_sets;
        }
        [Serializable]
        public class DataSet
        {
            [SerializeField] private string title;
            [SerializeField] private Entrie[] entries;
            public string Title => title;
            public Entrie[] Entries => entries;

        }
        [Serializable]
        public class Entrie
        {
            [SerializeField] private float position;
            [SerializeField] private float value;
            public float Position => position;
            public float Value => value;

            public AwesomeCharts.LineEntry ToLineEntry()
            {
                return new AwesomeCharts.LineEntry(this.Position, this.Value);
            }
            public AwesomeCharts.BarEntry ToBarEntry()
            {
                return new AwesomeCharts.BarEntry((long)this.Position, this.Value);
            }
        }

    }
}
