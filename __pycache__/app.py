from flask import Flask, request, jsonify
from forecasting_model import forecast_sales

app = Flask(__name__)

@app.route('/forecast', methods=['POST'])
def forecast():
    data = request.json
    sales_data = data.get('sales_data')  # Safe access
    forecast_period = data['forecast_period']

    forecast_result = forecast_sales( forecast_period=forecast_period,sales_data=sales_data)
    return jsonify(forecast_result)

if __name__ == '__main__':
    app.run(debug=True, port=5000)
