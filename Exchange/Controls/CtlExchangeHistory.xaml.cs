using Exchange.Data;

using FZCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Exchange.Controls;

/// <summary>
/// Representing the history data control.
/// </summary>
public partial class CtlExchangeHistory : UserControl
{
    const int BAR_WIDTH = 10;
    const int BAR_SPACE = 10;

    /// <summary>
    /// Creates a new <see cref="CtlExchangeHistory"/> instance.
    /// </summary>
    public CtlExchangeHistory()
    {
        InitializeComponent();
    }

    private static bool CurrenciesLoaded()
    {
        return ExchangeReport.LatestReport is not null;
    }

    #region Metrics' methods

    private void ClearCanvas()
    {
        canvas.Children.Clear();
        return;
    }

    private void CanvasApplyMargins()
    {
        canvas.Margin = new Thickness(0,
                                       0,
                                           SystemParameters.VerticalScrollBarWidth,
                                           SystemParameters.HorizontalScrollBarHeight);
    }

    private async Task<bool> CurrenciesInCZK()
    {
        if (CurrenciesLoaded() == false) return false;
        ClearCanvas();

        // display the currencies ina graph
        var currencies = ExchangeReport.LatestReport.Value.Currencies;

        List<Rectangle> rects = [];

        for (int x = 0; x < currencies.Count; x++)
        {
            CurrencyInfo currency = currencies[x];

            double value = ((double)currency.Rate / currency.Amount);
            Rectangle rect = new Rectangle
            {
                Fill = SystemColors.AccentColorBrush,
                Stroke = FZCore.Core.IsDarkModeEnabled() ? SystemColors.AccentColorLight2Brush : SystemColors.AccentColorDark2Brush,
                Width = BAR_WIDTH,
                Height = Math.Max(value, 1) * 5,
                ToolTip = string.Format("{0} ({1}) - {2:N2} CZK", currency.Currency, currency.Country, value),
            };

            rects.Add(rect);
        }

        canvas.Width = rects.Count * (BAR_SPACE + BAR_WIDTH);
        canvas.Height = rects.Max(x => x.Height);

        for (int x = 0; x < rects.Count; x++)
        {
            Rectangle rect = rects[x];
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, x * (BAR_WIDTH + BAR_SPACE));
            Canvas.SetBottom(rect, 0);
        }

        CanvasApplyMargins();

        App.MainWindow.SetStatusMessage(Messages.MetricApplied);
        return true;
    }

    private Dictionary<string, (string Name, Func<Task<bool>> Method)> _metrics => new Dictionary<string, (string, Func<Task<bool>>)>
    {
        { "unitinczk", ("Value in CZK", CurrenciesInCZK) }
    };

    private async Task LoadMetrics()
    {
        cbxMetric.Items.Clear();

        ComboBoxItem cbiFirst = new ComboBoxItem
        {
            Content = "(Select a metric)"
        };

        cbxMetric.Items.Add(cbiFirst);
        cbxMetric.Items.Add(new Separator());

        foreach (var metric in _metrics)
        {
            ComboBoxItem cbi = new ComboBoxItem
            {
                Uid = metric.Key,
                Content = metric.Value.Name
            };

            cbi.Selected += async (s, e) => await SelectMetricWrapper(metric.Key);

            cbxMetric.Items.Add(cbi);
        }

        if (cbxMetric.Items.Count > 0) cbxMetric.SelectedIndex = 0;
        return;
    }

    private async Task<bool> SelectMetric(string metric)
    {
        if (_metrics.ContainsKey(metric) == false) return false;
        return await _metrics[metric].Method();
    }

    private async Task SelectMetricWrapper(string metric)
    {
        if (await SelectMetric(metric) == false)
        {
            string message = string.Format(Messages.HistoryMetricError, metric);
            FZCore.Core.ErrorBox(message, App.Title);
        }
    }

    #endregion

    /// <summary>
    /// Asynchronously reloads the history data from the underlying data source.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the history was
    /// successfully reloaded; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This method loads all the past stored reports and displays various stats, including price development, etc.
    /// </remarks>
    public async Task<bool> ReloadHistory()
    {
        // clear the fields
        cbxCurrency.Items.Clear();

        // enum the currencies
        if (ExchangeReport.LatestReport is null)
        {
            if (await ExchangeReport.FetchAsync() == null)
            {
                // unable to fetch the report
                Log.Error("Unable to fetch the report.", nameof(ReloadHistory));
                return false;
            }
        }

        // latest report should not be null at this point
        foreach (CurrencyInfo currency in ExchangeReport.LatestReport.Value.Currencies)
        {
            ComboBoxItem cbi = new ComboBoxItem
            {
                Tag = currency,
                Content = $"{currency.Currency} ({currency.Country})"
            };

            cbxCurrency.Items.Add(cbi);
        }

        // select the top item
        if (cbxCurrency.Items.Count > 0) cbxCurrency.SelectedIndex = 0;

        App.MainWindow.SetStatusMessage($"History reloaded.");
        return true;
    }

    private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await LoadMetrics();

        if (await ReloadHistory() == false)
        {
            FZCore.Core.ErrorBox(Messages.ProcessingHistoryFailed, App.Title);
            App.MainWindow.SetStatusMessage(Messages.ProcessingHistoryFailed);
            return;
        }
    }
}
