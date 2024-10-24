from flask import Flask, request, jsonify
from forecasting_model import forecast_sales  # Import your model function

app = Flask(__name__)

@app.route('/forecast', methods=['POST'])
def forecast():
    data = request.json
    sales_data = data['sales_data']
    forecast_period = data['forecast_period']
    
    # Call your forecast_sales function
    forecast_result = forecast_sales(sales_data, forecast_period)
    return jsonify(forecast_result)

if __name__ == '__main__':
    app.run(debug=True, port=5000)
