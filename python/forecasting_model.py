import pandas as pd
from prophet import Prophet

def forecast_sales(forecast_period, sales_data=None):
    if sales_data is None:
        df = pd.read_csv('sales_data.csv')
    else:
        df = pd.DataFrame(sales_data)

    df['ds'] = pd.to_datetime(df['Date'])
    df['y'] = df['SalesAmount']

    model = Prophet()
    model.fit(df[['ds', 'y']])

    future = model.make_future_dataframe(periods=forecast_period, freq='ME')
    forecast = model.predict(future)

    count = len(df)
    future_forecast = forecast.iloc[forecast.index >= count]

    # 🔥 THIS LINE IS VERY IMPORTANT
    future_forecast['ds'] = future_forecast['ds'].dt.strftime('%Y-%m-%d')

    return future_forecast[['ds', 'yhat']].to_dict(orient='records')


# sales_data=pd.read_csv('sales_data.csv')
# print(sales_data)
# forecast_period=3
# forecast_result = forecast_sales( forecast_period=forecast_period,sales_data=sales_data)
# print(forecast_result)