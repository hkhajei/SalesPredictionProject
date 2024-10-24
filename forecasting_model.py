import pandas as pd
from prophet import Prophet

def forecast_sales(sales_data, forecast_period):
    # Convert your sales data into a Pandas DataFrame
    df = pd.DataFrame(sales_data)
    df['ds'] = pd.to_datetime(df['Date'])  # Prophet requires date as 'ds'
    df['y'] = df['SalesAmount']            # Prophet requires the target as 'y'

    # Initialize Prophet model
    model = Prophet()
    model.fit(df[['ds', 'y']])

    # Make a DataFrame for future dates
    future = model.make_future_dataframe(periods=forecast_period, freq='M')

    # Predict the future sales
    forecast = model.predict(future)

    count = len(sales_data)  # Find the last date in historical data
    future_forecast = forecast.iloc[forecast.index>= count]  # Filter to only get points after the last historical date

    
    # Return relevant forecast columns (e.g., ds and yhat)
    return future_forecast[['ds', 'yhat']].to_dict(orient='records')

# sales_data = [{'Date': '2022-01-01', 'SalesAmount': 15000},
#               {'Date': '2022-02-01', 'SalesAmount': 20000},
#               {'Date': '2022-03-01', 'SalesAmount': 18000}]

# forecast = forecast_sales(sales_data, forecast_period=3)
# print(forecast)